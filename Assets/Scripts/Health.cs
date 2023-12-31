using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Health : MonoBehaviour
{
    [Header("Prefabs")]
    [Space]
    public GameObject CourageEffectPrefab;
    public GameObject CombatTextPrefab;
    public GameObject DeathParticlePrefab;

    private float lastDamaged;
    private float healthRegenWaitTime;
    private int CurrencyAmountOnDeath;
    public event Action<float, float> OnHealthChanged;
    [Header("Health")]
    [Space]
    public float MaxHealth;
    public float currentHealth;

    [HideInInspector]
    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }
        set
        {
            currentHealth = Mathf.Clamp(value, 0, MaxHealth);
            if (currentHealth <= 0 && UnitData.TeamName == "Zombie") // if zombie dies, just remove it from board
            {
                Death();
            }
            else if (currentHealth <= 0 && UnitData.TeamName == "Plant") // if plant dies, give money and potion and remove it from board
            {
                Debug.Log(gameObject.name + " died");
                Events.SetMoney(Events.GetMoney() + (int)(CurrencyAmountOnDeath * CountdownTimer.Instance.currentTime / 60));
                MoneySound.Play();

                // show courage particle
                GameObject courageParticle = GameObject.Instantiate(CourageEffectPrefab, transform.position, Quaternion.identity, GameObject.Find("Canvas").transform);
                courageParticle.GetComponent<ParticleSystem>().Play();
                GameObject courage = GameObject.FindGameObjectWithTag("Courage").gameObject;
                var pos = Camera.main.ScreenToWorldPoint(courage.transform.position);
                TweenCallback tweenCallback = () => { Destroy(courageParticle.gameObject); };
                courageParticle.transform.DOMove(new Vector3(pos.x, pos.y, -10), 1.5f).OnComplete(tweenCallback);
                
                float rnd = UnityEngine.Random.Range(0f, 1f) + (CountdownTimer.Instance.currentTime/300);
                float rnd2 = UnityEngine.Random.Range(0f, 1f);
                //print(rnd+" "+ (CountdownTimer.Instance.currentTime / 300));
                if (rnd > 0.6)
                {
                    List<BoxCollider2D> startAreas = ZombieBuilder.Instance.startAreas;
                    BoxCollider2D closestStartArea = GetClosestStartArea(startAreas);
                    Vector2 spawnPosition = GetRandomPointInCollider(closestStartArea);

                    GameObject collectable = null;
                    if (rnd2 < 0.3)
                    {
                        collectable = GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[0], transform.position, Quaternion.identity, null);
                    }
                    else if (rnd2 < 0.6)
                    {
                        collectable = GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[1], transform.position, Quaternion.identity, null);
                    }
                    else
                    {
                        collectable = GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[2], transform.position, Quaternion.identity, null);
                    }
                    collectable.transform.DOMove(spawnPosition, 1f);
                }
                // spawn DeathParticle
                Death();

            }
            OnHealthChanged?.Invoke(currentHealth, MaxHealth);

            lastDamaged = Time.time;
        }
    }

    [Header("Other")]
    [Space]
    public UnitData UnitData;
    public AudioClipGroup DeathSound;
    public AudioClipGroup MoneySound;
    [Tooltip("Maximum size for plant prefab")]
    public Vector3 maxSize = new Vector3(2f, 2f, 2f);

    public void Awake()
    {
        // replace health data with unidata info
        MaxHealth = UnitData.MaxHealth;
        CurrencyAmountOnDeath = UnitData.CurrencyAmountOnDeath;

        currentHealth = MaxHealth;
        Events.OnAddMaxHealthValue += AddMaxHealth;
        lastDamaged = Time.time;
        healthRegenWaitTime = UnityEngine.Random.Range(8.0f, 15.0f);

        // Add characters to EntityController
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

    private BoxCollider2D GetClosestStartArea(List<BoxCollider2D> startAreas)
    {
        if (startAreas.Count > 0)
        {
            float minDistance = float.MaxValue;
            BoxCollider2D closestStartArea = null;

            foreach (BoxCollider2D startArea in startAreas)
            {
                float distance = Vector2.Distance(transform.position, startArea.bounds.center);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestStartArea = startArea;
                }
            }
            return closestStartArea;
        }

        return null;
    }

    private Vector2 GetRandomPointInCollider(BoxCollider2D collider)
    {
        // Get a random point within the collider's bounds
        float randomX = UnityEngine.Random.Range(collider.bounds.min.x, collider.bounds.max.x);
        float randomY = UnityEngine.Random.Range(collider.bounds.min.y, collider.bounds.max.y);
        Vector2 position = new Vector2(randomX, randomY);

        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(position);

        viewportPosition.x = Mathf.Clamp(viewportPosition.x - 10f / Screen.width, 0f, 1f);

        bool nearBorderX = Mathf.Abs(viewportPosition.x) < 10f / Screen.width;
        bool nearBorderY = Mathf.Abs(viewportPosition.y - 0.5f) < 5f / Screen.height;

        if (nearBorderX || nearBorderY)
        {
            // Bring the position closer to the right
            viewportPosition.x += 0.05f;
            viewportPosition.y = Mathf.Clamp01(viewportPosition.y);
        }

        // Convert the clamped position back to world space
        return Camera.main.ViewportToWorldPoint(viewportPosition);
    }
    /*
    public void ManualPotionSpawn(int potionIndex)
    {
        if (potionIndex == 0)
        {
            GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[potionIndex], new Vector2(-8, 2), Quaternion.identity, null);
        }
        if (potionIndex == 1)
        {
            GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[potionIndex], new Vector2(-8, 0), Quaternion.identity, null);
        }
        if (potionIndex == 2)
        {
            GameObject.Instantiate<GameObject>(UnitData.DroppablePotions[potionIndex], new Vector2(-8, -2), Quaternion.identity, null);
        }
    }
    */
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
        if (UnitData.TeamName == "Plant" && lastDamaged + healthRegenWaitTime < Time.time && transform.localScale.magnitude < maxSize.magnitude)
        {
            transform.localScale = new Vector3(transform.localScale.x + 0.05f, transform.localScale.y + 0.05f, transform.localScale.z + 0.05f);

            lastDamaged = Time.time;
            MaxHealth += 5;
            CurrentHealth += 5;
            CurrencyAmountOnDeath += 2;
            ShowHealthText(5);
            healthRegenWaitTime = UnityEngine.Random.Range(8.0f, 15.0f);
        }
    }

    public void ShowHealthText(float health)
    {
        GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
        EntityController.Instance.Other.Add(combatText);
        combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + health;
        combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
        combatText.transform.Find("HealthPos").gameObject.SetActive(true);
        TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
        combatText.transform.localScale = combatText.transform.localScale * 0.5f;
        combatText.transform.DOScale(combatText.transform.localScale * 1.5f, 1f).OnComplete(tweenCallback);
        EntityController.Instance.Other.Remove(combatText);
    }
    private void Death()
    {
        DeathSound.Play();
        GameObject deathParticle = GameObject.Instantiate(DeathParticlePrefab, transform.position, Quaternion.identity, null);
        deathParticle.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
}