using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using DG.Tweening;
using TMPro;

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
    public GameObject CombatTextPrefab;
    public AudioClipGroup AttackSound;

    private List<Health> targetsInRange = new List<Health>();
    private List<Health> currentTargets = new List<Health>();

    public Projectile ProjectilePrefab;
    public static Attacking Instance;

    private Particles particleEffects;

    private void Awake()
    {
        Instance = this;
        Events.OnAddDamageValue += AddDamage;
        Events.OnAddAttackSpeedValue += AddAttackSpeed;
        CurrentUnitAttackRange.OnEnemyEnteredAttackRange += AddEnemyToTargets;
        CurrentUnitAttackRange.OnEnemyExitedAttackRange += RemoveEnemyFromTargets;

        // this was in start before
        currentUnitHealth = GetComponent<Health>();
        if (currentUnitHealth.UnitData.TeamName != "Plant")
        {
            movement = GetComponent<Movement>();
        }

        AttackDamage = currentUnitHealth.UnitData.AttackDamage;
        AttackRangeSize = currentUnitHealth.UnitData.AttackRangeSize;
        AttackSpeed = currentUnitHealth.UnitData.AttackSpeed;

        CurrentUnitAttackRange.GetComponent<CircleCollider2D>().radius = AttackRangeSize;
        var attackRangeVisual = CurrentUnitAttackRange.transform.Find("AttackRangeVisual");
        // shown a little bigger because then it is more logical to player (they cant see pixels)
        attackRangeVisual.localScale = new Vector3(AttackRangeSize*2+0.3f, AttackRangeSize*2 + 0.3f, AttackRangeSize * 2 + 0.3f);
        MaxUnitsAttackingAtOnce = currentUnitHealth.UnitData.MaxUnitsAttackingAtOnce;
        NextAttackTime = Time.time;

        particleEffects = GetComponent<Particles>();
    }
    private void Start()
    {
        if (currentUnitHealth.UnitData.TeamName != "Plant")
        {
            direction = GetDirection();
        }
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
            GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + damage;
            combatText.transform.Find("AttackDamage").gameObject.SetActive(true);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
            TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
            combatText.transform.DOScale(combatText.transform.localScale * 0.5f, 0.5f).OnComplete(tweenCallback);
        }
    }

    void AddAttackSpeed(string unitName, float attackSpeed)
    {
        if (gameObject.name.StartsWith(unitName))
        {
            AttackSpeed += attackSpeed;
            GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + attackSpeed;
            combatText.transform.Find("Speed").gameObject.SetActive(true);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
            TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
            combatText.transform.DOScale(combatText.transform.localScale * 0.5f, 0.5f).OnComplete(tweenCallback);
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
                        if (ProjectilePrefab != null) // for ranged fighters
                        {
                            Projectile projectile = Instantiate<Projectile>(ProjectilePrefab);
                            projectile.SetShooter(gameObject, AttackDamage, target);
                            Vector3 firePoint = new Vector3((float)(gameObject.transform.position.x + 0.05), (float)(gameObject.transform.position.y + 0.244), 0);
                            projectile.transform.position = firePoint;
                            projectile.Target = target;
                            AttackSound.Play();
                        } 
                        else // for melee fighters
                        {
                            if (particleEffects != null) // for poison cloud plant
                            {
                                particleEffects.PlayParticles(transform.position);
                            }
                            target.CurrentHealth -= AttackDamage;
                            AttackSound.Play();
                            CombatDamageTexts(target);
                        }
                        if (target.UnitData.TeamName == "Plant" && target.transform.localScale.magnitude > minSize.magnitude)// if unit that takes damage is plant then change plant object size
                        {
                            target.transform.localScale = new Vector3(target.transform.localScale.x - 0.01f * AttackDamage, target.transform.localScale.y - 0.01f * AttackDamage, target.transform.localScale.z - 0.01f * AttackDamage);
                            var targetAttackRange = target.transform.Find("AttackRange");
                            targetAttackRange.localScale = new Vector3(targetAttackRange.localScale.x + 0.01f * AttackDamage, targetAttackRange.localScale.y + 0.01f * AttackDamage, targetAttackRange.localScale.z + 0.01f * AttackDamage);
                        
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

    public void CombatDamageTexts(Health target)
    {
        GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(target.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), target.transform.position.y + UnityEngine.Random.Range(-0.5f, 0.5f), target.transform.position.z), Quaternion.identity);
        combatText.transform.Find("combat_text").GetComponent<TextMeshPro>().text = "-" + AttackDamage;
        combatText.transform.Find("Health").gameObject.SetActive(true);
        combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.red;
        TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
        DOTween.Sequence().Append(combatText.transform.DOScale(combatText.transform.localScale * 0.5f, 0.5f)).Append(combatText.transform.DOJump(combatText.transform.position + new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), 0.2f, 0), UnityEngine.Random.Range(0.01f, 0.1f), 1, 0.5f, false)).OnComplete(tweenCallback);
        //combatText.transform.DOScale(combatText.transform.localScale*0.5f, 0.5f).OnComplete(tweenCallback);
    }

    private void OnMouseEnter()
    {
        var attackRangeVisual = CurrentUnitAttackRange.transform.Find("AttackRangeVisual");
        if(attackRangeVisual != null)
        {
            attackRangeVisual.gameObject.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        var attackRangeVisual = CurrentUnitAttackRange.transform.Find("AttackRangeVisual");
        if(attackRangeVisual != null)
        {
            attackRangeVisual.gameObject.SetActive(false);
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
