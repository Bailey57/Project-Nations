using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButton : MonoBehaviour
{

    /*
     * Scene number: Scene
     * 
     * 0: Main Menu 
     * 
     * 1: Main game
     * 
     * 2: Start Settings 
     * 
     * 
     * 
     */

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0);
    }


    public void LoadGameStartSettingsScene() 
    {
        SceneManager.LoadScene(2);
    }


    public void StartGame() 
    {
        SceneManager.LoadScene(1);

    }

    public void ExitGame() 
    {
        Application.Quit();
    
    }
}
