﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerPack : Pack
{

    // Update is called once per frame
    void Update()
    {
         MoveDir(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetMouseButton(0)) {
            Shoot(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        }

        if (Input.GetKey(KeyCode.Escape)) {
            SceneManager.LoadScene("StartMenu");
        }

        if (Input.GetKey(KeyCode.Space)) {
            if (formationSpread != 6f) {
                
                formationSpread = 6f;
                PackMove(true);
            }            
        } else if (formationSpread == 6f ) {
            formationSpread = 1.5f;
            PackMove(true);
        }

    }

    protected new void Death() {
        SceneManager.LoadScene("GameOver");
    }
}
