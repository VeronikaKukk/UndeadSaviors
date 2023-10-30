using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayLevel()
    {
        // TODO, kui mitu levelit valmis saavad:  SceneManager.LoadSceneAsync(leveli number vm);
        SceneManager.LoadSceneAsync("MainScene");
    }

    public void QuitGame() {
        Application.Quit();
    }

}
