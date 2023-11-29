using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer Instance;
    public TextMeshProUGUI TimerText;
    public float StartTime = 300.0f; // 5 minutes in seconds
    [Range(0, 300)]
    public float currentTime;

    public bool isGameRunning;

    public void Awake()
    {
        Instance = this;
        isGameRunning = true;
    }
    private void Start()
    {
        currentTime = StartTime;
        UpdateTimerDisplay();
        Events.OnEndLevel += OnEndLevel;

    }
    public void ResetTimer(float gameTime) {
        StartTime = gameTime;
        currentTime = StartTime;
        UpdateTimerDisplay();
        isGameRunning = true;
    }

    void OnEndLevel(bool isWin) {
        isGameRunning = false;
    }

    private void Update()
    {
        if (currentTime > 1 && isGameRunning) // Has to be 1
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }

    public void OnDestroy()
    {
        Events.OnEndLevel -= OnEndLevel;
    }
}
