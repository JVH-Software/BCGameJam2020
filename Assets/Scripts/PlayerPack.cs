using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPack : Pack
{
    new void Start()
    {
        base.Start();
    }

    new void Update()
    {
        base.Update();

        // Movement
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
