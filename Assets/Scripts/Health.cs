using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Health : MonoBehaviour
{
    public UnitData UnitData;

    public float MaxHealth;
    private int CurrencyAmountOnDeath;
    public event Action<float, float> OnHealthChanged;
    public GameObject CombatTextPrefab;

    public GameObject DeathParticlePrefab;

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
                // spawn DeathParticle
                GameObject deathParticle = GameObject.Instantiate(DeathParticlePrefab, transform.position, Quaternion.identity, null);
                deathParticle.GetComponent<ParticleSystem>().Play();
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
                // spawn DeathParticle
                GameObject deathParticle = GameObject.Instantiate(DeathParticlePrefab, transform.position, Quaternion.identity, null);
                deathParticle.GetComponent<ParticleSystem>().Play();
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
            ShowHealthText(health);
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
            ShowHealthText(2);
        }
    }

    private void ShowHealthText(float health) {
        GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
        combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + health;
        combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
        TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
        combatText.transform.DOScale(combatText.transform.localScale * 0.5f, 0.5f).OnComplete(tweenCallback);
    }
}
