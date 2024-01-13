using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class ScenarioController : MonoBehaviour
{
    public GameObject HideGamePanel;
    public GameObject PauseMenuPanel;
    public GameObject EndGamePanel;
    public GameObject UnitInfoPanel;
    public TextMeshProUGUI EndGameText;
    public Scene currentScene;
    private bool levelRunning = true;
    public Shop ZombieButtonPrefab;

    public GameObject ShopPanel;

    public List<GameObject> SpawnPoints;

    public bool levelPaused = false;
    public TextMeshProUGUI PauseButtonText;
    public Button pauseButton;
    public Button unitInfoButton; // for later use
    public LevelData currentLevelData;

    public bool StartingTheLevel = true;

    public static ScenarioController Instance;

    public GameObject DeathTrapPrefab;
    public AudioClipGroup LevelWinAudio;
    public AudioClipGroup LevelLoseAudio;

    private void Awake()
    {
        Instance = this;
        currentScene = SceneManager.GetActiveScene();
        Events.OnEndLevel += OnEndLevel;
        Events.OnStartLevel += OnStartLevel;
        PauseButtonText.text = "II"; // no need to change for anything else later
    }

    private void Start()
    {
        HideGamePanel.SetActive(false);
        PauseMenuPanel.SetActive(false);
        EndGamePanel.SetActive(false);
        unitInfoButton.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        Events.OnEndLevel -= OnEndLevel;
        Events.OnStartLevel -= OnStartLevel;
    }

    private void Update()
    {
        if (levelRunning && CountdownTimer.Instance.currentTime < 1 && !StartingTheLevel)
        {
            if (EntityController.Instance.PlantCharacters.Count >= 1)
            {
                levelRunning = false;
                Events.EndLevel(false);
            }
        }
        else if (levelRunning && !StartingTheLevel)
        {
            if (currentScene.name != "TutorialScene")
            {
                if (EntityController.Instance.PlantCharacters.Count < 1)
                {
                    levelRunning = false;
                    Events.EndLevel(true);
                }
            }
        }
    }



    public void PauseLevel(bool isRegularPause)
    {
        if (levelPaused && isRegularPause)
        {
            HideGamePanel.SetActive(false);
            PauseMenuPanel.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            unitInfoButton.gameObject.SetActive(true);
            Time.timeScale = 1;
            PauseButtonText.text = "II";
            levelPaused = false;
        }
        else if (!levelPaused && isRegularPause)
        {
            HideGamePanel.SetActive(true);
            PauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
            pauseButton.gameObject.SetActive(false);
            unitInfoButton.gameObject.SetActive(false);
            levelPaused = true;

            if (currentScene.name == "TutorialScene")
            {
                TutorialScenario.Instance.PauseLogic();
            }
        }
        else if (!isRegularPause && levelPaused)
        {
            UnitInfoPanel.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            unitInfoButton.GetComponentInChildren<TMP_Text>().text = "?";
            Time.timeScale = 1;
            PauseButtonText.text = "II";
            levelPaused = false;
        }
        else {
            UnitInfoPanel.SetActive(true);
            Time.timeScale = 0;
            unitInfoButton.GetComponentInChildren<TMP_Text>().text = "X";
            pauseButton.gameObject.SetActive(false);
            levelPaused = true;
        }
    }

    public void ToggleUnitInfoPanel() {//change this freely
        PauseLevel(false);
    }

    public void OnStartLevel(LevelData data)
    {
        StartingTheLevel = true;

        currentLevelData = data;
        Events.SetMoney(data.StartingMoney);
        CountdownTimer.Instance.StartTime = data.Gametime;

        for (int i = 0; i < ShopPanel.transform.childCount; i++)
        {
            Destroy(ShopPanel.transform.GetChild(i).gameObject);
        }

        foreach (var zombie in data.Zombies)
        {
            Shop btn = Instantiate<Shop>(ZombieButtonPrefab, ShopPanel.transform);
            btn.ShopData = zombie;
        }

        SpawnEnemies(data.Plants);
    }

    public void ResetLevel()
    {
        StartingTheLevel = true;
        Time.timeScale = 1;
        pauseButton.gameObject.SetActive(true);

        // remove all enemies and zombies and potions and potion effects from table
        foreach (var i in EntityController.Instance.PlantCharacters)
        {
            Destroy(i.gameObject);
        }
        foreach (var i in EntityController.Instance.ZombieCharacters)
        {
            Destroy(i.gameObject);
        }
        foreach (var i in EntityController.Instance.Potions)
        {
            Destroy(i.gameObject);
        }
        foreach (var i in EntityController.Instance.Other)
        {
            Destroy(i.gameObject);
        }
        EntityController.Instance.Reset();
        ZombieFactory.Instance.Awake();
        CountdownTimer.Instance.ResetTimer(currentLevelData.Gametime);
        Events.SetMoney(currentLevelData.StartingMoney);

        for (int i = 0; i < ShopPanel.transform.childCount; i++)
        {
            Destroy(ShopPanel.transform.GetChild(i).gameObject);
        }

        foreach (var zombie in currentLevelData.Zombies)
        {
            Shop btn = Instantiate<Shop>(ZombieButtonPrefab, ShopPanel.transform);
            btn.ShopData = zombie;
        }

        SpawnEnemies(currentLevelData.Plants);
    }

    public void SpawnEnemies(List<UnitData> plants)
    {
        List<GameObject> keys = new List<GameObject>();
        keys.AddRange(SpawnPoints);
        // spawn one random deathtrap
        if (currentScene.name != "TutorialScene")
        {
            var x = UnityEngine.Random.Range(10, 15);
            GameObject deathtrap = GameObject.Instantiate(DeathTrapPrefab, keys[x].transform.position, Quaternion.identity, keys[x].transform);
            EntityController.Instance.Other.Add(deathtrap);
            keys.Remove(keys[x]);
            var rnd = new System.Random();
            keys = keys.OrderBy(x => rnd.Next()).ToList();
        }
        for (int i = 0; i < plants.Count; i++)
        {
            GameObject spawn = keys[0];
            keys.Remove(spawn);
            GameObject plant = GameObject.Instantiate(plants[i].UnitPrefab, spawn.transform.position, Quaternion.identity, spawn.transform);
        }
        StartingTheLevel = false;
        levelRunning = true;
    }

    public void OnEndLevel(bool isWin)
    {
        //if (!levelRunning) return;

        //levelRunning = false;
        HideGamePanel.SetActive(true);
        EndGamePanel.SetActive(true);
        pauseButton.gameObject.SetActive(false);
        if (isWin)
        {
            EndGameText.text = "Victory!";
            LevelWinAudio.Play();
            print("levelwin playing");
            if (currentLevelData.LevelNumber >= PlayerPrefs.GetInt("levelsUnlocked")) {
                PlayerPrefs.SetInt("levelsUnlocked", currentLevelData.LevelNumber+1);
                print("Unlocked level" + PlayerPrefs.GetInt("levelsUnlocked"));
            }
        }
        else
        {
            EndGameText.text = "Defeat!";
            LevelLoseAudio.Play();
            print("levellose playing");

        }
        Time.timeScale = 0;
    }

    public void ReplayButton()
    {
        Time.timeScale = 1;
        HideGamePanel.SetActive(false);
        EndGamePanel.SetActive(false);
        PauseMenuPanel.SetActive(false);
        ResetLevel();

    }

    public void LevelChooserButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("StartScene");

    }
}