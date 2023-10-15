using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public static CountdownTimer Instance;
    public TextMeshProUGUI TimerText;
    public float StartTime = 300.0f; // 5 minutes in seconds
    public float currentTime;


    public void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        currentTime = StartTime;
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (currentTime > 1) // Has to be 1. With 0 will go into negative digits. Dunno why
        {
            currentTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            currentTime = 0;
            // Handle timer completion here
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        TimerText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
