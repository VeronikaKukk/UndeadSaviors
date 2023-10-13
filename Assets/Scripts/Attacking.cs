using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public float AttackDamage;
    public float AttackSpeed;
    public float AttackRange;

    private Movement movement;
    private Health health;

    private Vector2 direction;
    private bool isFighting;

    void Start() { 
        health = GetComponent<Health>();
        if (health.UnitData.TeamName != "Plant")
        {
            movement = GetComponent<Movement>();
            direction = GetDirection();
        }
    }


    public void Update()
    {
        if (movement != null && !isFighting)// non-attacking zombie moves to closest enemy
        {
            movement.Move(direction);
        }
    }

    private Vector2 GetDirection() {
        Health enemy = GetClosestEnemy();
        if (enemy != null)
        {
            return (enemy.transform.position - transform.position).normalized;
        }
        return Random.insideUnitCircle.normalized;
    }
    private Health GetClosestEnemy() 
    {
        Health closestEnemy = null;
        float minDist = 999999999999.0f;
        List<Health> enemies = null;
        if (health.UnitData.TeamName == "Zombie") {
            enemies = EntityController.Instance.PlantCharacters;
        } else if (health.UnitData.TeamName == "Plant") {
            enemies = EntityController.Instance.ZombieCharacters;
        }
        foreach (Health enemy in enemies) 
        {
            Debug.Log(enemy.UnitData.TeamName);
            if (enemy.CurrentHealth > 0) 
            {
                float dist = (enemy.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist) {
                    closestEnemy = enemy;
                    minDist = dist;
                }
            }
        }
        return closestEnemy;
    }

}
