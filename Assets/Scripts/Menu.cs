using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject LevelChooserObject;
    public GameObject MainMenuObject;
    public GameObject LevelPanel;
    public LevelCard LevelCardPrefab;
    public GameObject OptionsMenuObject;
    public GameObject InstructionsMenuObject;

    public List<LevelData> Levels;
    public LevelData selectedLevel;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        for (int i = 0; i < LevelPanel.transform.childCount; i++) {
            Destroy(LevelPanel.transform.GetChild(i).gameObject);
        }

        foreach (var level in Levels) {
            LevelCard card = Instantiate<LevelCard>(LevelCardPrefab, LevelPanel.transform);
            card.SetData(level);
            card.OnClicked += PlayLevel;
        }
    }

    public void PlayLevel(LevelData level)
    {
        selectedLevel = level;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(level.SceneName);
    }

    private void OnSceneLoaded(Scene _scene, LoadSceneMode _mode) {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        Events.StartLevel(selectedLevel);
        Destroy(gameObject);
    }
    public void QuitGame() {
        Application.Quit();
    }

    public void StartGameButton() {
        MainMenuObject.SetActive(false);
        LevelChooserObject.SetActive(true);
    }


    public void OptionsMenuButtonPressed()
    {
        MainMenuObject.SetActive(false);
        OptionsMenuObject.SetActive(true);

    }
    public void InstructionsMenuButtonPressed()
    {
        MainMenuObject.SetActive(false);
        InstructionsMenuObject.SetActive(true);
    }

    public void BackToMainMenuButtonPressed()
    {
        OptionsMenuObject.SetActive(false);
        LevelChooserObject.SetActive(false);
        InstructionsMenuObject.SetActive(false);
        MainMenuObject.SetActive(true);
    }

}