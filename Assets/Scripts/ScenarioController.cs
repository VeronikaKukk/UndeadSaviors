using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Linq;
using System;
using UnityEngine.EventSystems;

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

    private bool levelPaused = false;
    public TextMeshProUGUI PauseButtonText;
    public Button pauseButton;
    public Button unitInfoButton; // for later use
    public LevelData currentLevelData;

    public bool StartingTheLevel = true;

    public static ScenarioController Instance;

    public GameObject DeathTrapPrefab;
    public AudioClipGroup LevelWinAudio;
    public AudioClipGroup LevelLoseAudio;


    // FOR TUTORIAL
    public TextMeshProUGUI[] AllTexts;
    public Image[] AllArrows;
    private int index;
    private int indexArrow;
    private Button shopButton;
    private bool activateShopButton = false;
    private bool isPlantDead = false;
    public GameObject DroppablePotion;
    //

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

        if (currentScene.name == "TutorialScene")
        {
            TutorialStuff();
            AllTexts[0].gameObject.SetActive(true);
            index = 0;
            indexArrow = -1;
            pauseButton.interactable = false;
            shopButton = ShopPanel.GetComponentInChildren<Button>();
            shopButton.interactable = false;
        }
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
            if (EntityController.Instance.PlantCharacters.Count < 1)
            {
                levelRunning = false;
                Events.EndLevel(true);
            }
        }

        if (currentScene.name == "TutorialScene")
        {
            if (Input.GetMouseButtonDown(0) && !levelPaused)
            {
                Invoke("TutorialClicking", 0.2f);
            }
            if (!activateShopButton)
            {
                shopButton = ShopPanel.GetComponentInChildren<Button>();
                Debug.Log(shopButton + " " + shopButton.interactable);
                shopButton.interactable = false;
            }
            if (EntityController.Instance.PlantCharacters.Count < 1 && index == 12 && isPlantDead == false)
            {
                isPlantDead = true;
                AllTexts[index - 1].gameObject.SetActive(false);
                AllTexts[index].gameObject.SetActive(true);
                AllArrows[indexArrow].gameObject.SetActive(false);
                index += 1;
                Instantiate<GameObject>(DroppablePotion, new Vector2((float)-1.65, 0), Quaternion.identity, null);
            }
        }
    }

    public void TutorialStuff()
    {
        foreach (TextMeshProUGUI text in AllTexts)
        {
            text.gameObject.SetActive(false);
        }

        foreach (Image arrow in AllArrows)
        {
            arrow.gameObject.SetActive(false);
        }
    }

    // Selle loogikaga tegeles Andre. Kui midagi on katki siis öelge mulle, sellest aru saamine on peavalu kui pole ise teinud :D
    private void TutorialClicking()
    {
        if (index <= 11)
        {
            if (index <= 4 || index >= 6)
            {
                index += 1;
                if (index == 3 || index == 4 || index == 5 || index == 11)
                {
                    indexArrow += 1;
                }
                Debug.Log(index);
            }
            else if (index >= 5 && EntityController.Instance.ZombieCharacters.Count > 0)
            {
                index += 1;
                if (index == 6)
                {
                    indexArrow += 1;
                }
                Debug.Log(index);
            }

            if (index != 0)
            {
                AllTexts[index-1].gameObject.SetActive(false);
            }
            if (indexArrow > 0)
            {
                AllArrows[indexArrow-1].gameObject.SetActive(false);
            }

            if (index == 4) // poest ostmise tutorial message
            {
                activateShopButton = true;
            }

            if (index == 10) // siin on pausi nupu tutorial message
            {
                pauseButton.interactable = true;
            }

            AllTexts[index].gameObject.SetActive(true);
            if (index == 3 || index == 4 || index == 5 || index == 10 || index == 11)
            {
                AllArrows[indexArrow].gameObject.SetActive(true);
            }
        }

        if (isPlantDead && index > 11 && index <= 21) // sõnumid, mis tulevad alles siis kui Plant on surma saanud ja eelnevad messaged ära näidatud
        {
            AllTexts[index - 1].gameObject.SetActive(false); // loogika lihtsuse jaoks, esmakordsel sisenemisel on tarvis
            AllArrows[indexArrow - 1].gameObject.SetActive(false);

            index += 1;
            Debug.Log(index);
            AllTexts[index - 1].gameObject.SetActive(false);
            AllTexts[index].gameObject.SetActive(true);

            if (index == 13 || index == 14 || index == 15 || index == 16 || index == 19 || index == 22)
            {
                indexArrow += 1;
                if (AllArrows.Length - 1 < indexArrow)
                {
                    indexArrow = AllArrows.Length - 1;
                }
                AllArrows[indexArrow].gameObject.SetActive(true);
            } else
            {
                AllArrows[indexArrow].gameObject.SetActive(false);
            }
            if (index != 22)
            {
                AllArrows[indexArrow - 1].gameObject.SetActive(false);
            }
        }

        else if (index == 22) // viimase sõnumi kustutamine
        {
            AllTexts[index].gameObject.SetActive(false);
            AllArrows[indexArrow].gameObject.SetActive(false);
        }

    }

    public void PauseLevel()
    {
        if (levelPaused)
        {
            HideGamePanel.SetActive(false);
            PauseMenuPanel.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            Time.timeScale = 1;
            PauseButtonText.text = "II";
            levelPaused = false;
        }
        else
        {
            HideGamePanel.SetActive(true);
            PauseMenuPanel.SetActive(true);
            Time.timeScale = 0;
            pauseButton.gameObject.SetActive(false);
            levelPaused = true;

            index -= 1;
            if (index == 3 || index == 4 || index == 5 || index == 10 || index == 11 || index == 13 || index == 14 || index == 15 || index == 16 || index == 19 || index == 22)
            {
                indexArrow -= 1;
                AllArrows[indexArrow].gameObject.SetActive(false);
            }
            AllTexts[index].gameObject.SetActive(false);
        }
    }

    public void ToggleUnitInfoPanel() {//change this freely
        PauseLevel();
        UnitInfoPanel.SetActive(!UnitInfoPanel.activeSelf);
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
        var x = UnityEngine.Random.Range(10,15);
        GameObject deathtrap = GameObject.Instantiate(DeathTrapPrefab, keys[x].transform.position, Quaternion.identity, keys[x].transform);
        EntityController.Instance.Other.Add(deathtrap);
        keys.Remove(keys[x]);
        var rnd = new System.Random();
        keys = keys.OrderBy(x => rnd.Next()).ToList();
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