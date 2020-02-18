using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelMenu : MonoBehaviour
{
    public string sceneOne;

    public void Quit() {
        Application.Quit();
    }

    public void PlayGame() {
        GlobalData gd =  GameObject.Find("GlobalData").GetComponent<GlobalData>();
        gd.level++;
        SceneManager.LoadScene(sceneOne);
    }

    public void LoadMainMenu() {
        SceneManager.LoadScene("StartMenu");
    }
}
