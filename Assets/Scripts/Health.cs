using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public UnitData UnitData;
    public float MaxHealth;
    public float CurrencyAmount;
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
            // if health is <= 0 then do something here
        }
    }
    public void Awake()
    {
        currentHealth = MaxHealth;
    }
    // here check collision and if needed remove health
}
