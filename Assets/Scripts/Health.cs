using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public UnitData UnitData;

    public float MaxHealth;
    private int CurrencyAmountOnDeath;
    public event Action<float, float> OnHealthChanged;

    public Vector3 maxSize = new Vector3(2f, 2f, 2f);

    private float lastDamaged;

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
                Events.SetMoney(Events.GetMoney() + (int)(CurrencyAmountOnDeath * CountdownTimer.Instance.currentTime/60));
                
                float rnd = UnityEngine.Random.Range(0f, 1f);
                float rnd2 = UnityEngine.Random.Range(0f, 1f);

                if (rnd > 0.4) 
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
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            lastDamaged = Time.time;
        }
    }
    public void Awake()
    {
        // replace health data with unidata info
        MaxHealth = UnitData.MaxHealth;
        CurrencyAmountOnDeath = UnitData.CurrencyAmountOnDeath;

        currentHealth = MaxHealth;
        Events.OnAddMaxHealthValue += AddMaxHealth;
        lastDamaged = Time.time;
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
            CurrentHealth += health;
        }
    }

    public void Update()
    {
        if (UnitData.TeamName == "Plant" && lastDamaged + 7.0f < Time.time && transform.localScale.magnitude < maxSize.magnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z + 0.05f);
            lastDamaged = Time.time;
            MaxHealth += 2;
            CurrentHealth += 2;
            CurrencyAmountOnDeath += 2;
        }
    }
}
