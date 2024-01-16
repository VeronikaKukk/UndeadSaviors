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
    public ScalingAnimation OpeningAnimation;
    public ScalingAnimation ClosingAnimation;
    public GameObject HideGamePanel;

    public ScenarioController scenarioController;


    public void ResumeGame()
    {
        Close();
        scenarioController.PauseLevel(true);
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

    public void Open()
    {
        if (gameObject.activeSelf) return;
        HideGamePanel.SetActive(true);
        gameObject.SetActive(true);
        OpeningAnimation.enabled = true;
    }

    public void Close()
    {
        if (!gameObject.activeSelf) return;
        ClosingAnimation.enabled = true;
    }

    public void ClosingAnimationFinished()
    {
        gameObject.SetActive(false);
        HideGamePanel.SetActive(false);
    }


}
