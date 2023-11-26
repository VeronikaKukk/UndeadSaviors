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
        scenarioController.PauseLevel();
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        scenarioController.ReplayButton();
        ResumeGame();
    }

    public void MainMenu()
    {
        scenarioController.LevelChooserButton();
    }


}
