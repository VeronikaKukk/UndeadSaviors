using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public Button resumeButton;
    public Button quitButton;
    public Button mainMenuButton;
    public Button restartLevelButton;

    public ScenarioController scenarioController;


    public void ResumeGame()
    {
        Time.timeScale = 1;
        scenarioController.PauseLevel();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        scenarioController.ReplayButton();
    }

    public void MainMenu()
    {
        Time.timeScale = 1;
        scenarioController.LevelChooserButton();
    }


}
