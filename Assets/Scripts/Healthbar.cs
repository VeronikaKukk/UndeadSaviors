using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Image HealthBar;
    public Health Health;

    private void Awake()
    {
        Health.OnHealthChanged += SetHealth;
    }

    private void OnDestroy()
    {
        Health.OnHealthChanged -= SetHealth;
    }

    public void SetHealth(float health, float maxHealth)
    {
        HealthBar.fillAmount = health / (float)maxHealth;
    }
}
