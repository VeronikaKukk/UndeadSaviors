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
    private void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(clicked);
    }

    public void SetData(LevelData data) {
        this.data = data;
        NameText.text = data.LevelName;
    }

    private void clicked() {
        OnClicked?.Invoke(data);
    }
}
