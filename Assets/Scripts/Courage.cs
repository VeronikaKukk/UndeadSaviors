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
    private float passiveGenerationTimer = 5.0f; // Timer for passive money generation
    private float timeSinceLastGeneration = 0.0f;

    private void Awake()
    {
        Money = GameStartMoney;
        MoneyText.text = Money.ToString();
        Events.OnGetMoney += getMoney;
        Events.OnSetMoney += setMoney;
    }

    private void Update()
    {
        // Call PassiveMoneyGeneration after every 5 (passiveGenerationTimer) seconds and increase Money by 10 (PassiveIncomeMoney)
        timeSinceLastGeneration += Time.deltaTime;
        if (timeSinceLastGeneration >= passiveGenerationTimer)
        {
            Money += PassiveIncomeMoney;
            PassiveMoneyGeneration(Money);
            timeSinceLastGeneration = 0.0f;
        }
    }

    public void PassiveMoneyGeneration(int value)
    {
        MoneyText.text = value.ToString();
    }
    public void OnDestroy()
    {
        Events.OnGetMoney -= getMoney;
        Events.OnSetMoney -= setMoney;
    }
}
