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
    private bool levelRunning = true;
    public Shop ZombieButtonPrefab;

    public GameObject ShopPanel;

    public List<GameObject> SpawnPoints;

    private void Awake()
    {
        currentScene = SceneManager.GetActiveScene();
        Events.OnEndLevel += OnEndLevel;
        Events.OnStartLevel += OnStartLevel;

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
        for (int i = 0; i < plants.Count; i++) {
            GameObject plant = GameObject.Instantiate(plants[i].UnitPrefab, SpawnPoints[i].transform.position, Quaternion.identity, SpawnPoints[i].transform);
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
