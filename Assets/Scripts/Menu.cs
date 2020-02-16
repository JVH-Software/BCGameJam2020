using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string sceneOne;

    public void Quit() {
        Application.Quit();
    }

    public void PlayGame() {
        SceneManager.LoadScene(sceneOne);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("StartMenu");
    }
}
