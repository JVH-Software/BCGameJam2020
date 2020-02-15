using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPack : Pack
{

    public Transform movementTarget;
    public UnityEngine.Tilemaps.Tilemap tilemap;

    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        // Movement
        List<Node> path;
        Pathfinding.FindPath(new Vector2Int(Mathf.RoundToInt(transform.position.x -0.5f), Mathf.RoundToInt(transform.position.y - 0.5f)),
            new Vector2Int(Mathf.RoundToInt(movementTarget.position.x - 0.5f), Mathf.RoundToInt(movementTarget.position.y - 0.5f)), tilemap, out path);
        Vector2 targetNodePos = new Vector2(path[0].Position.x + 0.5f, path[0].Position.y + 0.5f);
        Vector2 moveDirection = (targetNodePos - new Vector2(transform.position.x, transform.position.y));
        moveDirection.Normalize();
        movementVector = moveDirection;
        

        /* Shoot
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }*/
    }
}
