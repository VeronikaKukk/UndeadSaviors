using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ScenarioController : MonoBehaviour
{
    public GameObject EndGamePanel;
    public TextMeshProUGUI EndGameText;
    public Scene currentScene;
    //private bool levelRunning;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        Events.OnEndLevel += OnEndLevel;

    }

    private void Start()
    {
        EndGamePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnEndLevel -= OnEndLevel;
    }

    private void Update()
    {
        if (CountdownTimer.Instance.currentTime < 1)
        {
            if (EntityController.Instance.PlantCharacters.Count >= 1)
            {
                OnEndLevel(false);
            }
        }
        else
        {
            if (EntityController.Instance.PlantCharacters.Count < 1)
            {
                OnEndLevel(true);
            }
        }
    }

    public void OnEndLevel(bool isWin)
    {
        //if (!levelRunning) return;

        //levelRunning = false;
        EndGamePanel.SetActive(true);
        if (isWin)
        {
            EndGameText.text = "Victory!";
        }
        else
        {
            EndGameText.text = "Defeat!";
        }
    }

        public void ReplayButton()
        {
            SceneManager.LoadScene(currentScene.name);
        }

        public void LevelChooserButton()
        {
            SceneManager.LoadScene("StartScene");

        }
}
