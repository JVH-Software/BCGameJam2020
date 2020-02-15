using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PackMember : MonoBehaviour
{

    public float health = 10f;
    public float maxHealth = 10f;
    public float speed = 1f;
    public Pack pack;

    public UnityEngine.Tilemaps.Tilemap tilemap;


    private bool dead = false;
    private Gun gun;

    protected UpgradeList upgrades;

    Rigidbody2D rbody;
    Animator anim;
    protected Vector2 movementVector = Vector2.zero;

    public void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        gun = GetComponent<Gun>();
    }

    public void FixedUpdate()
    {
        PerformMovement();
    }

    public void Update()
    {
        Move(GetClosestWalkableTile(RandomPointInCircle(5f, pack.packLeader.transform.position.x, pack.packLeader.transform.position.y)));
    }

    private void PerformMovement()
    {

        if (dead)
            return;

        /* Sample code for future animation
    
        if (movementVector != Vector2.zero)
        {
            anim.SetBool("Iswalking", true);
            anim.SetFloat("Input_x", movementVector.x);
            anim.SetFloat("Input_y", movementVector.y);
        }
        else
        {
            anim.SetBool("Iswalking", false);
        }
        */

        rbody.velocity = (rbody.velocity + movementVector * Time.deltaTime * speed * pack.speedMultiplier);
    }

    public void Shoot(Vector3 target, Pack pack)
    {
        if (dead)
            return;

        gun.Shoot(target, pack, this);

    }

    public void Hit(float damage, Vector2 knockback)
    {
        ModifyHealth(-damage);
        Knockback(knockback);
    }

    public void Knockback(Vector2 knockback)
    {
        rbody.velocity = (rbody.velocity + knockback);
    }

    private void ModifyHealth(float amount)
    {
        health += amount;
        if (health <= 0)
        {
            Death();
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }

        pack.UpdateHealth();
    }

    private void Death()
    {
        dead = true;
    }

    protected Vector2 GetClosestWalkableTile(Vector2 v)
    {
        return GetClosestWalkableTile((int)v.x, (int)v.y);
    }

    protected Vector2 GetClosestWalkableTile(int x, int y)
    {
        if (tilemap.GetTile(new Vector3Int(x, y, 0)))
            return new Vector2(x, y);

        List<TileBase> neighbours = new List<TileBase>();

        var level = 0;
        while (level < 10)
        { // arbirtaray stop condition to prevent it from spiraling out of control
            level++;        // no pun intended

            for (int i = x - level; i <= x + level; i++)
                for (int j = y - level; j <= y + level; j++)
                    // only check if this tile is on the edge of the ring
                    if ((i == x - level || i == x + level) || (j == y - level || j == y + level))
                    {
                        if (tilemap.GetTile(new Vector3Int(i, j, 0)))
                            return new Vector2(i, j);
                    }
        }
        // this shouldn't happen
        return new Vector2();
    }


    protected static Vector2 RandomPointInCircle(float radius, float x, float y)
    {
        Debug.Log(x + " " + y);

        //https://programming.guide/random-point-within-circle.html


        System.Random random = new System.Random();

        float angle = (float)random.NextDouble() * 2 * Mathf.PI;

        // 1 + (r-1) because I don't want the inner circle to be the target(impassable terrain)
        float r = 1 + (radius - 1 * Mathf.Sqrt((float)random.NextDouble()));

        return new Vector2(x + (r * Mathf.Cos(angle)), y + (r * Mathf.Sin(angle)));
    }

    protected void Move(Vector2 pos)
    {
        Move(pos.x, pos.y);
    }
    protected void Move(float x, float y)
    {

        // Movement
        List<Node> path;

        Pathfinding.FindPath(
                new Vector2Int(Mathf.RoundToInt(transform.position.x - 0.5f), Mathf.RoundToInt(transform.position.y - 0.5f)), // startpos
                new Vector2Int(Mathf.RoundToInt(x), Mathf.RoundToInt(y)), // target
                tilemap,
                out path);

        // stop endless stream of error messages
        if (path.Count <= 0) return;

        Vector2 targetNodePos = new Vector2(path[0].Position.x + 0.5f,
                                            path[0].Position.y + 0.5f);
        Vector2 moveDirection = targetNodePos - new Vector2(transform.position.x, transform.position.y);
        moveDirection.Normalize();
        movementVector = moveDirection;
    }
}
