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
            Events.SetMoney(Events.GetMoney() + PassiveIncomeMoney);
            PassiveMoneyGeneration(Events.GetMoney());
            timeSinceLastGeneration = 0.0f;
        }
    }

    public void PassiveMoneyGeneration(int value)
    {
        MoneyText.text = value.ToString();
    }

    public void DecreaseMoney(int amount)
    {
        Events.SetMoney(Events.GetMoney() - amount);
        MoneyText.text = Events.GetMoney().ToString();
    }

    public void IncreaseMoney(int amount)
    {
        Events.SetMoney(Events.GetMoney() + amount);
        MoneyText.text = Events.GetMoney().ToString();
    }

    public bool EnoughMoney(int amount)
    {
        if (Money >= amount)
            return true;
        return false;
    }

    public void OnDestroy()
    {
        Events.OnGetMoney -= getMoney;
        Events.OnSetMoney -= setMoney;
    }
}
