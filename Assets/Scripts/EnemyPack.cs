using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

public class EnemyPack : Pack
{

    public Transform movementTarget;
    public GameObject attackTarget;
    public float attackRange = 5f;
    public float agroRange = 7f;

    public Vector2 _staticTarget;

    // simulate global point array TODO: hook into point array
    private List<Vector2> points = new List<Vector2>() { new Vector2(-3f, 5f) };

    public UnityEngine.Tilemaps.Tilemap tilemap;


    public void Start() {
        var closestPoint = points.OrderBy(x => Vector2.Distance(transform.position, x)).First();

       _staticTarget = GetClosestWalkableTile(RandomPointInCircle(3f, closestPoint.x, closestPoint.y));

        base.Start();
    }

    private Vector2 GetClosestWalkableTile(Vector2 v) {
        return GetClosestWalkableTile((int)v.x, (int)v.y);
    }
    private Vector2 GetClosestWalkableTile(int x, int y) {
        if (tilemap.GetTile(new Vector3Int(x, y, 0)))
            return new Vector2(x, y);

        List<TileBase> neighbours = new List<TileBase>();

        var level = 0;
        while (level < 10) { // arbirtaray stop condition to prevent it from spiraling out of control
            level++;        // no pun intended

            for (int i = x - level; i <= x + level; i++)
                for (int j = y - level; j <= y + level; j++)
                    // only check if this tile is on the edge of the ring
                    if ((i == x - level || i == x + level) || (j == y - level || j == y + level)) {
                        if (tilemap.GetTile(new Vector3Int(i, j, 0)))
                            return new Vector2(i, j);
                    }
        }
        // this shouldn't happen
        return new Vector2();
    }
        

    private static Vector2 RandomPointInCircle(float radius, float x, float y) {
        Debug.Log(x + " " + y);

        //https://programming.guide/random-point-within-circle.html


        System.Random random = new System.Random();

        float angle = (float)random.NextDouble() * 2 * Mathf.PI;
        
        // 1 + (r-1) because I don't want the inner circle to be the target(impassable terrain)
        float r = 1 + (radius-1 * Mathf.Sqrt((float)random.NextDouble()));

        return new Vector2(x + (r * Mathf.Cos(angle)), y + (r * Mathf.Sin(angle)));
    }

    void Update()
    {


        // Priority 1: If player is in agro range
        if (Mathf.Abs(Vector3.Distance(transform.position, attackTarget.transform.position)) <= agroRange) {
            // move towards them
            Move(attackTarget.transform.position.x - 0.5f, attackTarget.transform.position.y - 0.5f) ;
        }
        // Priority 2: move to the closest point
        else if (points.Count > 0) {
            if (Mathf.Abs(Vector2.Distance(_staticTarget, new Vector2(transform.position.x, transform.position.y))) <= 2) {
                var closestPoint = points.OrderBy(x => Vector2.Distance(transform.position, x)).First();

                _staticTarget = GetClosestWalkableTile(RandomPointInCircle(3f, closestPoint.x, closestPoint.y));
            }

            Move(_staticTarget);
        }

        // If attack target is in attack range
        if (Mathf.Abs(Vector2.Distance(transform.position,attackTarget.transform.position)) <= attackRange)
        {
            Shoot(attackTarget.transform.position);
        }
    }
    void Move(Vector2 pos) {
        Move(pos.x, pos.y);
    }
    void Move(float x, float y) {

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
