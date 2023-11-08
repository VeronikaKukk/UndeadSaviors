using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Attacking : MonoBehaviour
{
    public float AttackDamage;
    public float AttackSpeed;
    public float AttackRangeSize;

    public Vector3 minSize = new Vector3(0.2f,0.2f,0.2f);

    private Movement movement;
    private Health currentUnitHealth;

    private Vector2 direction;
    private bool isFighting;

    private float NextAttackTime;

    private int MaxUnitsAttackingAtOnce;

    public AttackRange CurrentUnitAttackRange;

    private List<Health> targetsInRange = new List<Health>();
    private List<Health> currentTargets = new List<Health>();
    private void Awake()
    {
        Events.OnAddDamageValue += AddDamage;
        Events.OnAddAttackSpeedValue += AddAttackSpeed;
        CurrentUnitAttackRange.OnEnemyEnteredAttackRange += AddEnemyToTargets;
        CurrentUnitAttackRange.OnEnemyExitedAttackRange += RemoveEnemyFromTargets;

        // this was in start before
        currentUnitHealth = GetComponent<Health>();
        if (currentUnitHealth.UnitData.TeamName != "Plant")
        {
            movement = GetComponent<Movement>();
            direction = GetDirection();
        }

        AttackDamage = currentUnitHealth.UnitData.AttackDamage;
        AttackRangeSize = currentUnitHealth.UnitData.AttackRangeSize;
        AttackSpeed = currentUnitHealth.UnitData.AttackSpeed;
        CurrentUnitAttackRange.GetComponent<CircleCollider2D>().radius = AttackRangeSize;
        MaxUnitsAttackingAtOnce = currentUnitHealth.UnitData.MaxUnitsAttackingAtOnce;
        NextAttackTime = Time.time;
    }

    private void OnDestroy()
    {
        Events.OnAddDamageValue -= AddDamage;
        Events.OnAddAttackSpeedValue -= AddAttackSpeed;
        CurrentUnitAttackRange.OnEnemyEnteredAttackRange -= AddEnemyToTargets;
        CurrentUnitAttackRange.OnEnemyExitedAttackRange -= RemoveEnemyFromTargets;
    }

    void RemoveEnemyFromTargets(Health enemy) {
        if (enemy.UnitData.TeamName != currentUnitHealth.UnitData.TeamName)
        {
            targetsInRange.Remove(enemy);
        }
    }
    void AddEnemyToTargets(Health enemy)
    {
        if (enemy.UnitData.TeamName != currentUnitHealth.UnitData.TeamName)
        {
            targetsInRange.Add(enemy);
        }
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

    public void Update()
    {
        if (movement != null && !isFighting)// non-attacking zombie moves to closest enemy
        {
            movement.Move(direction);
        }
        else if (movement == null) 
        {
            currentTargets = GetNClosestTargets();
        }

        if (targetsInRange.Count > 0)
        {
            currentTargets = GetNClosestTargets();
            isFighting = true;
            if (NextAttackTime <= Time.time)
            {
                foreach (var target in currentTargets) {
                    if (target != null)
                    {
                        target.CurrentHealth -= AttackDamage;
                        if (target.UnitData.TeamName == "Plant" && target.transform.localScale.magnitude > minSize.magnitude)// if unit that takes damage is plant then change plant object size
                        {
                            target.transform.localScale = new Vector3(target.transform.localScale.x - 0.01f * AttackDamage, target.transform.localScale.y - 0.01f * AttackDamage, target.transform.localScale.z - 0.01f * AttackDamage);
                        }
                    }
                }               
                NextAttackTime = Time.time + 1/AttackSpeed; // the bigger the attackspeed the faster it hits
            }
        }
        else {
            isFighting = false;
            if (currentUnitHealth.UnitData.TeamName != "Plant")
            {
                direction = GetDirection();
            }
        }

    }

    private Vector2 GetDirection() {
        List<Health> closestEnemies = GetNClosestTargets();
        if (closestEnemies.Count > 0) {
            Health enemy = closestEnemies[0];
            if (enemy != null)
            {
                return (enemy.transform.position - transform.position).normalized;
            }
        }        
        return Vector2.zero;
    }

    private List<Health> GetNClosestTargets() {
        List<Health> nClosestTargets = new List<Health>();
        List<(Health,float)> targetsWithDist = new List<(Health,float)>();

        List<Health> enemies = null;
        if (currentUnitHealth.UnitData.TeamName == "Zombie")
        {
            enemies = EntityController.Instance.PlantCharacters;
        }
        else if (currentUnitHealth.UnitData.TeamName == "Plant")
        {
            enemies = EntityController.Instance.ZombieCharacters;
        }

        if (targetsInRange.Count > 0)
        {
            foreach (Health target in targetsInRange)
            {
                float dist = Vector2.Distance(target.transform.position, transform.position);
                targetsWithDist.Add((target, dist));
            }
        }
        else {
            foreach (Health enemy in enemies)
            {
                float dist = Vector2.Distance(enemy.transform.position, transform.position);
                targetsWithDist.Add((enemy, dist));
            }
        }
        targetsWithDist.Sort((x, y) => x.Item2.CompareTo(y.Item2));
        for (int i = 0; i < Math.Min(targetsWithDist.Count, MaxUnitsAttackingAtOnce); i++) {
            nClosestTargets.Add(targetsWithDist[i].Item1);
        }
        return nClosestTargets;
    }
}
