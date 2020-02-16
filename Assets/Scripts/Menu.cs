using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public string sceneOne;
    public int startingLevel = 0;

    public void Quit() {
        Application.Quit();
    }

    public void PlayGame() {
        GameObject.Find("GlobalData").GetComponent<GlobalData>().level = startingLevel;
        SceneManager.LoadScene(sceneOne);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("StartMenu");
    }
}
