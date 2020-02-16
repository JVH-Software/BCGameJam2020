using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class Pack : MonoBehaviour
{
    internal GameObject target;
    public float speedMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public float damageMultiplier = 1f;
    public float defenceMultiplier = 1f;
    public float knockbackMultiplier = 1f;
    public float fireRateMultiplier = 1f;

    public Transform respawnPoint;
    private List<PackMember> packMembers = new List<PackMember>();
    internal PackMember packLeader;
    public float attackRange = 5;

    public float health = 50f;
    public float maxHealth = 50f;

    public UpgradeList upgrades;
    private Pack attackTarget;
    public SimpleHealthBar healthBar;
    internal GameManager gameManager;


    public float formationSpread = 1.5f;

    // 0 = circle, 1 = box, 2 = shuffle
    public int formation = 0;

    public void Start() {

        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();

        upgrades = new UpgradeList(this);
        for(int i = 0; i < transform.childCount; i++)
        {
            packMembers.Add(transform.GetChild(i).GetComponent<PackMember>());
        }

        // Assign pack leader
        if (packLeader == null) packLeader = packMembers[0];

        // get initial positions
        PackMove();

        // Check health.
        ModifyHealth(0);
    }

    private void Update() {

        // Get closest pack
        TargetNearestPack();

        // Target pack leader
        target = attackTarget.packLeader.gameObject;

        // Get into range
        if (target != null) {
            Move(target.transform.position);
        }
        // Attack when in range
        if (target != null && Mathf.Abs(Vector2.Distance(packLeader.transform.position, target.transform.position)) <= attackRange) {
            Shoot(target.transform.position);
        }
    }

    private void TargetNearestPack()
    {
        float dist = -1;
        for (int i = 0; i < gameManager.packs.Length; i++)
        {
            if (!gameManager.packs[i].tag.Equals(this.tag))
            {
                float tempDist = Mathf.Abs(Vector2.Distance(gameManager.packs[i].packLeader.transform.position, transform.position));
                if (tempDist < dist || dist == -1)
                {
                    dist = tempDist;
                    attackTarget = gameManager.packs[i];
                }
            }
        }
    }

    protected void Move(Vector2 v) {
        Move(v.x, v.y);
    }

    // Movement
    protected void Move(float x, float y) {
        packLeader.Move(x, y);
        PackMove();
    }

    protected void MoveDir(Vector2 v) {
        MoveDir(v.x, v.y);
    }
    protected void MoveDir(float x, float y) {
        packLeader.MoveDir(x, y);
        PackMove();
    }

    protected void PackMove(bool force=false) {
        CircleFormation(force);
    }

    private string _lastLeaderPos = null;

    private void CircleFormation(bool force=false) {
        
        var counter = 1;
        var touchedTiles = "";

        // add our main characters pos to the nono list
        // + 0.5 to get the tile we are in (player position is in the center, tile is top left)
        touchedTiles += (int)(packLeader.transform.position.x+0.5f) + "," + (int)(packLeader.transform.position.y+0.5f) + "|";
        if (_lastLeaderPos == touchedTiles && !force) return;
        _lastLeaderPos = touchedTiles;

        foreach (PackMember packMember in packMembers) {
            if (packMember == packLeader) continue;

            // Get closest point that is valid to move to (in clock formation around player)
            var point = Utility.GetClosestWalkableTile(
                Utility.GetSpecificPointInCircle(formationSpread,
                ((2 * Mathf.PI) / packMembers.Count) * counter,
                packLeader.transform.position.x,
                packLeader.transform.position.y),
                gameManager.ground,
                touchedTiles);

            // add this to our nono list
            touchedTiles += (int)point.x + "," + (int)point.y + "|";

            // assign this target to the pack member
            packMember.SetTarget(point.x, point.y);

            counter++;
        }

    }

    public void Shoot(Vector3 target)
    {
        foreach (PackMember packMember in packMembers) {
            packMember.Shoot(target, this);
        }
    }

    public void Respawn()
    {
        health = maxHealth;
        var takenTiles = "";
        foreach (PackMember packMember in packMembers) {
            var pos = Utility.GetClosestWalkableTile(respawnPoint.position, gameManager.ground, takenTiles);
            takenTiles += pos.x + "," + pos.y + "|";
            packMember.transform.position = pos;
            packMember.dead = false;
        }
    }

    protected void Death() {
        foreach (PackMember packMember in packMembers) {
            packMember.MemberDeath();
        }
    }

    public void ModifyHealth(float amount, PackMember lastHit = null) {
        if(amount < 0 && upgrades.Contains(Upgrades.DefenceBoost))
        {
            amount /= 2;
        }
        health += amount;

        var deathUnit = maxHealth / packMembers.Count;
        var numDead = (int)((maxHealth- health) / deathUnit);
        
        if (health <= 0) {
            Death();
        } else if (health > maxHealth) {
            health = maxHealth;
        }

        int actualDead = 0;
        foreach (var pm in packMembers) {
            if (pm.dead) actualDead++;
        }

        if (actualDead > numDead) 
            foreach (var savedGuys in packMembers.Where(x => x.dead).Take(actualDead - numDead).AsEnumerable()) 
                savedGuys.MemberRessurect();
        else if (actualDead < numDead) {
            if (lastHit != null) { 
                // kill the last guy that got hit
                lastHit.MemberDeath();
                // count it
                actualDead += 1;
            }
            // if we still need to kill, do so
            if (actualDead < numDead) foreach (var doomedGuys in packMembers.Where(x => !x.dead).Take(numDead - actualDead).AsEnumerable()) 
                doomedGuys.MemberDeath();
        }

        // assign a new leader if need be
        if(packLeader.dead && numDead < packMembers.Count) {
            var counter = 0;
            while (counter < packMembers.Count) {
                if (packMembers[counter].dead != true) {
                    packLeader = packMembers[counter];
                    if(packLeader.tag.Equals("Player")) {
                        Camera.main.GetComponent<CameraTracking>().target = packLeader.gameObject;
                    }
                    break;
                }
                counter++;
            }
        }
        
        if (healthBar != null) {
            healthBar.UpdateBar(health, maxHealth);
        }

    }
}
