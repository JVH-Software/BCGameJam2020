using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pack : MonoBehaviour
{

    public float speedMultiplier = 1f;
    public float projectileSpeedMultiplier = 1f;
    public Transform respawnPoint;
    public List<PackMember> packMembers;
    public PackMember packLeader;
    public float attackRange = 5;
    protected UpgradeList upgrades;
    public GameObject attackTarget;
    public UnityEngine.Tilemaps.Tilemap tilemap;

    public void Start() {
        upgrades = new UpgradeList(this);
        if (packLeader == null) packLeader = packMembers[0];

        // get initial positions
        PackMove();
    }

    private void Update() {

        // Priority 1: If player is in agro range
        if (attackTarget != null) {
            Move(attackTarget.transform.position);
        }

        if (attackTarget != null && Mathf.Abs(Vector2.Distance(packLeader.transform.position, attackTarget.transform.position)) <= attackRange) {
            Shoot(attackTarget.transform.position);
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

    protected void PackMove() {

        var counter = 0;
        var touchedTiles = new List<Vector2>();

        // add our main characters pos to the nono list
        // + 0.5 to get the tile we are in (player position is in the center, tile is top left)
        touchedTiles.Add(new Vector2((int)(packLeader.transform.position.x), (int)(packLeader.transform.position.y)));

        foreach (PackMember packMember in packMembers) {
            if (packMember == packLeader) continue;

            // Get closest point that is valid to move to (in clock formation around player)
            var point = Utility.GetClosestWalkableTile(
                Utility.GetSpecificPointInCircle(1.5f,
                ((2 * Mathf.PI) / packMembers.Count) * counter,
                packLeader.transform.position.x,
                packLeader.transform.position.y),
                tilemap,
                touchedTiles);

            // add this to our nono list
            touchedTiles.Add(point);

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
        foreach (PackMember packMember in packMembers)
        {
            packMember.health = packMember.maxHealth;
            packMember.transform.position = respawnPoint.position;
            packMember.dead = false;
        }
    }
}
