using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public UnitData UnitData;
    public float MaxHealth;
    public int CurrencyAmount;
    private float currentHealth;
    [HideInInspector]
    public float CurrentHealth {
        get 
        {
            return currentHealth; 
        }
        set 
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0 && UnitData.TeamName == "Zombie") // if zombie dies, just remove it from board
            {
                Destroy(gameObject);
            } 
            else if (currentHealth <= 0 && UnitData.TeamName == "Plant") // if plant dies, give money and remove it from board
            {
                Debug.Log("Plant died");
                // give currencyamount to player and destroy plant
                Events.SetMoney(Events.GetMoney() + CurrencyAmount);
                Destroy(gameObject);
            }
        }
    }
    public void Awake()
    {
        currentHealth = MaxHealth;
    }

    public void Start() // Add characters to EntityController
    {
        if (UnitData.TeamName == "Zombie")
        {
            EntityController.Instance.ZombieCharacters.Add(this);
        }
        else if (UnitData.TeamName == "Plant")
        {
            EntityController.Instance.PlantCharacters.Add(this);
        }
    }

    public void OnDestroy()// Remove characters from EntityController
    {
        if (UnitData.TeamName == "Zombie")
        {
            EntityController.Instance.ZombieCharacters.Remove(this);
        }
        else if (UnitData.TeamName == "Plant")
        {
            EntityController.Instance.PlantCharacters.Remove(this);
        }
    }
}
