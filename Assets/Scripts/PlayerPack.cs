using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPack : Pack
{
    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }
}
