using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;

public class ScenarioController : MonoBehaviour
{
    public GameObject EndGamePanel;
    public TextMeshProUGUI EndGameText;
    public Scene currentScene;
    private bool levelRunning = true;
    public Shop ZombieButtonPrefab;

    public GameObject ShopPanel;

    public List<GameObject> SpawnPoints;

    private bool levelPaused = false;
    public TextMeshProUGUI PauseButtonText;
    public Button pauseButton;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        Events.OnEndLevel += OnEndLevel;
        Events.OnStartLevel += OnStartLevel;
        PauseButtonText.text = "II";
    }

    private void Start()
    {
        EndGamePanel.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnEndLevel -= OnEndLevel;
        Events.OnStartLevel -= OnStartLevel;

    }

    private void Update()
    {
        if (levelRunning && CountdownTimer.Instance.currentTime < 1)
        {
            if (EntityController.Instance.PlantCharacters.Count >= 1)
            {
                levelRunning = false;
                Events.EndLevel(false);
            }
        }
        else if(levelRunning)
        {
            if (EntityController.Instance.PlantCharacters.Count < 1)
            {
                levelRunning = false;
                Events.EndLevel(true);
            }
        }
    }

    public void PauseLevel()
    {
        if (levelPaused)
        {
            Time.timeScale = 1;
            PauseButtonText.text = "II";
            levelPaused = false;
        }
        else
        {
            Time.timeScale = 0;
            PauseButtonText.text = "►";
            levelPaused = true;
        }
    }

    public void OnStartLevel(LevelData data) {
        Events.SetMoney(data.StartingMoney);
        CountdownTimer.Instance.StartTime = data.Gametime;

        for (int i = 0; i< ShopPanel.transform.childCount; i++)
        {
            Destroy(ShopPanel.transform.GetChild(i).gameObject);
        }

        foreach (var zombie in data.Zombies) {
            Shop btn = Instantiate<Shop>(ZombieButtonPrefab, ShopPanel.transform);
            btn.ShopData = zombie;
        }

        SpawnEnemies(data.Plants);
    }
    public void SpawnEnemies(List<UnitData> plants) {
        List<GameObject> keys = SpawnPoints;
        var rnd = new System.Random();
        keys = keys.OrderBy(x => rnd.Next()).ToList();
        for (int i = 0; i < plants.Count; i++) {
            GameObject spawn = keys[0];
            keys.Remove(spawn);
            GameObject plant = GameObject.Instantiate(plants[i].UnitPrefab, spawn.transform.position, Quaternion.identity, spawn.transform);
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
