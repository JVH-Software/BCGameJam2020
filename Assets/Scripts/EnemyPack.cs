using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.Tilemaps;

public class EnemyPack : PackMember
{

    public Transform movementTarget;
    public GameObject attackTarget;
    public float attackRange = 5f;
    public float agroRange = 7f;

    public Vector2 _staticTarget;

    // simulate global point array TODO: hook into point array
    private List<Vector2> points = new List<Vector2>() { new Vector2(-3f, 5f) };

    public void Start() {
        var closestPoint = points.OrderBy(x => Vector2.Distance(transform.position, x)).First();

       _staticTarget = GetClosestWalkableTile(RandomPointInCircle(3f, closestPoint.x, closestPoint.y));

        base.Start();
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
            Shoot(attackTarget.transform.position, pack);
        }
    }
}
