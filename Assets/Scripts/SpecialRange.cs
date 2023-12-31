using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialRange : MonoBehaviour
{
    public string AbilityType;
    public float AbilityStrength;
    public float AbilitySpeed;

    private string teamName;
    public HashSet<Health> targetsInRange = new HashSet<Health>();

    private float nextAbilityTime;
    private void Start()
    {
        nextAbilityTime = Time.time + AbilitySpeed;
        teamName = GetComponentInParent<Health>().UnitData.TeamName;
    }
    void Update()
    {
        if (nextAbilityTime <= Time.time)
        {
            if (targetsInRange.Count > 0)
            {
                foreach (Health target in targetsInRange)
                {
                    if (target != null)
                    {
                        if (AbilityType == "Heal")
                        {
                            target.MaxHealth += AbilityStrength;
                            target.CurrentHealth += AbilityStrength;
                            target.ShowHealthText(AbilityStrength);
                        }
                    }
                }
                nextAbilityTime = Time.time + AbilitySpeed;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit"))
        { // if the collider is the units' own collider
            Health health = collision.GetComponent<Health>();
            if (health != null && health.UnitData.TeamName == teamName)
            {
                targetsInRange.Add(health);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit"))
        { // if the collider is the units' own collider
            Health health = collision.GetComponent<Health>();
            if (health != null && health.UnitData.TeamName == teamName)
            {
                targetsInRange.Remove(health);
            }
        }
    }



}
