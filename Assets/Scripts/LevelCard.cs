using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCard : MonoBehaviour
{
    public TextMeshProUGUI NameText;
    public LevelData data;
    public event Action<LevelData> OnClicked;
    private Button button;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(clicked);
    }

    public void SetData(LevelData data, bool isUnlocked) {
        this.data = data;
        NameText.text = data.LevelName;
        if (!isUnlocked) {
            GetComponent<Button>().interactable = false;
        }    
    }

    private void clicked() {
        OnClicked?.Invoke(data);
    }
}
