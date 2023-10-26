using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScenarioController : MonoBehaviour
{
    public GameObject EndGamePanel;
    public TextMeshProUGUI EndGameText;
    //private bool levelRunning;

    private void Awake()
    {
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

        List<Health> plants = EntityController.Instance.PlantCharacters;
        if (CountdownTimer.Instance.currentTime < 1)
        {
            if (plants.Count >= 1)
            {
                OnEndLevel(false);
            }
        }
        else
        {
            if (plants.Count < 1)
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
}
