using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPack : Pack
{
    new void Start()
    {
        base.Start();
    }

    void Update()
    {
        // Movement
        movementVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        // Shoot
        if (Input.GetMouseButtonDown(0))
        {
            Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene("StartMenu");
        }
    }
}
