﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string sceneOne;

    public void Quit() {
        Application.Quit();
    }

    public void StartClicked() {
        SceneManager.LoadScene(sceneOne);
    }
}
