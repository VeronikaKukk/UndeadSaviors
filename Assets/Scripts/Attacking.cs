using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Attacking : MonoBehaviour
{
    public float AttackDamage;
    public float AttackSpeed;
    public float AttackRangeSize;

    public Vector3 maxSize = new Vector3(3f,3f,3f);
    public Vector3 minSize = new Vector3(0.2f,0.2f,0.2f);

    private Movement movement;
    private Health health;

    private Vector2 direction;
    private bool isFighting;
    private Health enemyToFight;

    private float NextAttackTime;
    private float PrevAttackTime;

    private void Awake()
    {
        Events.OnAddDamageValue += AddDamage;
        Events.OnAddAttackSpeedValue += AddAttackSpeed;
    }

    private void OnDestroy()
    {
        Events.OnAddDamageValue -= AddDamage;
        Events.OnAddAttackSpeedValue -= AddAttackSpeed;
    }

    void AddDamage(string unitName,float damage)
    {
        if (gameObject.name.StartsWith(unitName))
        {
            AttackDamage += damage;
        }
    }

    void AddAttackSpeed(string unitName, float attackSpeed)
    {
        if (gameObject.name.StartsWith(unitName))
        {
            AttackSpeed += attackSpeed;
        }
    }


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
        else if (movement == null) 
        {
            GetClosestEnemy();
        }

        if (enemyToFight != null)
        {
            if (!isFighting && Mathf.Abs((transform.position - enemyToFight.transform.position).sqrMagnitude) <= AttackRangeSize)
            {
                isFighting = true;
                NextAttackTime = Time.time;
            }

            if (isFighting && NextAttackTime <= Time.time)
            {
                enemyToFight.CurrentHealth -= AttackDamage;
                // if unit that takes damage is plant then change plant object size
                if (enemyToFight.UnitData.TeamName == "Plant" && enemyToFight.transform.localScale.magnitude >  minSize.magnitude) 
                {
                    enemyToFight.transform.localScale = new Vector3(enemyToFight.transform.localScale.x - 0.1f, enemyToFight.transform.localScale.y - 0.1f, enemyToFight.transform.localScale.z - 0.1f);
                }
                PrevAttackTime = NextAttackTime;
                NextAttackTime = Time.time + 1/AttackSpeed; // the bigger the attackspeed the faster it hits
            }
            if (enemyToFight.CurrentHealth < 0)
            {
                isFighting = false;
                if (health.UnitData.TeamName != "Plant")
                {
                    direction = GetDirection();
                }
            }
        }
        else {
            isFighting = false;
            if (health.UnitData.TeamName != "Plant")
            {
                direction = GetDirection();
            }
        }

    }

    private Vector2 GetDirection() {
        Health enemy = GetClosestEnemy();
        if (enemy != null)
        {
            return (enemy.transform.position - transform.position).normalized;
        }
        return Vector2.zero;
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
            if (enemy.CurrentHealth > 0) 
            {
                float dist = (enemy.transform.position - transform.position).sqrMagnitude;
                if (dist < minDist) {
                    closestEnemy = enemy;
                    minDist = dist;
                }
            }
        }
        enemyToFight = closestEnemy;
        return closestEnemy;
    }

}
