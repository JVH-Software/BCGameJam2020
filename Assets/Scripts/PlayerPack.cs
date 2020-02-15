using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPack : PackMember
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        // Movement
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            pack.Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (health <= 0)
        {
            pack.Respawn();
        }
    }

}
