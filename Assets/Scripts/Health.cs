using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public UnitData UnitData;
    public float MaxHealth;
    public int CurrencyAmountOnDeath;

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
            else if (currentHealth <= 0 && UnitData.TeamName == "Plant") // if plant dies, give money and potion and remove it from board
            {
                Debug.Log(gameObject.name +" died");
                // give currencyamount to player and destroy plant
                // currenttime is made to minutes
                Events.SetMoney(Events.GetMoney() + (int)(CurrencyAmountOnDeath * CountdownTimer.Instance.currentTime/60));
                
                // drop potion
                float rnd = Random.Range(0f, 1f);
                float rnd2 = Random.Range(0f, 1f);

                if (rnd > 0.3) 
                {
                    GameObject collectable = null;
                    if (rnd2 < 0.3)
                    {
                        collectable = GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[0], transform.position, Quaternion.identity, null);

                    }
                    else if (rnd2 < 0.6) {
                        collectable = GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[1], transform.position, Quaternion.identity, null);

                    }
                    else
                    {
                        collectable = GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[2], transform.position, Quaternion.identity, null);
                    }
                }
                Destroy(gameObject);
            }
        }
    }
    public void Awake()
    {
        currentHealth = MaxHealth;
        Events.OnAddMaxHealthValue += AddMaxHealth;
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
        Events.OnAddMaxHealthValue -= AddMaxHealth;

        if (UnitData.TeamName == "Zombie")
        {
            EntityController.Instance.ZombieCharacters.Remove(this);
        }
        else if (UnitData.TeamName == "Plant")
        {
            EntityController.Instance.PlantCharacters.Remove(this);
        }
    }

    void AddMaxHealth(string unitName, float health)
    {
        if (gameObject.name.StartsWith(unitName))
        {
            MaxHealth += health;
        }
    }
}
