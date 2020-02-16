using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPack : Pack
{

    // Update is called once per frame
    void Update()
    {
        MoveDir(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0)) {
            Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }
    }
}
