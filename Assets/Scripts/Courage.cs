using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Courage : MonoBehaviour
{
    public int Money { get; private set; }
    private int getMoney() => Money;
    private void setMoney(int value) => Money = value;

    public int GameStartMoney = 100;
    public int PassiveIncomeMoney = 10;

    public TextMeshProUGUI MoneyText;
    public float PassiveGenerationTimer = 5.0f; // Timer for passive money generation
    private float timeSinceLastGeneration = 0.0f;

    private void Awake()
    {
        Money = GameStartMoney;
        Events.OnGetMoney += getMoney;
        Events.OnSetMoney += setMoney;
        MoneyText.text = Events.GetMoney().ToString();
    }

    private void Update()
    {
        // Call PassiveMoneyGeneration after every passiveGenerationTimer seconds and increase Money by PassiveIncomeMoney
        timeSinceLastGeneration += Time.deltaTime;
        MoneyText.text = Events.GetMoney().ToString();
        if (timeSinceLastGeneration >= PassiveGenerationTimer)
        {
            Events.SetMoney(Events.GetMoney() + PassiveIncomeMoney);
            timeSinceLastGeneration = 0.0f;
        }
    }

    public void OnDestroy()
    {
        Events.OnGetMoney -= getMoney;
        Events.OnSetMoney -= setMoney;
    }
}
