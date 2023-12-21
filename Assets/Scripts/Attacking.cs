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
    [Header("Everything related to attacking")]
    [Space]
    public float AttackDamage;
    public float AttackSpeed;
    [Tooltip("Attack range radius (it is a circle)")]
    public float AttackRangeSize;
    public AudioClipGroup AttackSound;

    [Header("Other")]
    [Space]
    public GameObject CombatTextPrefab;
    [Tooltip("Leave empty if unit has no projectile")]
    public Projectile ProjectilePrefab;
    [Tooltip("Minimum size for plant prefab")]
    public Vector3 minSize = new Vector3(0.2f,0.2f,0.2f);

    private Movement movement;
    private Vector2 direction;
    
    private Health currentUnitHealthComponent;
    
    private AttackRange CurrentUnitAttackRange;
    private bool isFighting;
    private float nextAttackTime;

    private int maxUnitsAttackingAtOnce;
    public List<Health> targetsInRange = new List<Health>();
    public List<Health> currentTargets = new List<Health>();

    private Particles particleEffects;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        Events.OnAddDamageValue += AddDamage;
        Events.OnAddAttackSpeedValue += AddAttackSpeed;
        Events.OnAddAttackRangeValue += AddAttackRange;

        CurrentUnitAttackRange = transform.Find("AttackRange").GetComponent<AttackRange>();
        CurrentUnitAttackRange.OnEnemyEnteredAttackRange += AddEnemyToTargets;
        CurrentUnitAttackRange.OnEnemyExitedAttackRange += RemoveEnemyFromTargets;

        // this was in start before
        currentUnitHealthComponent = GetComponent<Health>();
        if (currentUnitHealthComponent.UnitData.TeamName != "Plant")
        {
            movement = GetComponent<Movement>();
        }

        AttackDamage = currentUnitHealthComponent.UnitData.AttackDamage;
        AttackRangeSize = currentUnitHealthComponent.UnitData.AttackRangeSize;
        AttackSpeed = currentUnitHealthComponent.UnitData.AttackSpeed;

        CurrentUnitAttackRange.GetComponent<CircleCollider2D>().radius = AttackRangeSize;
        var attackRangeVisual = CurrentUnitAttackRange.transform.Find("AttackRangeVisual");
        // shown a little bigger because then it is more logical to player (they cant see pixels)
        attackRangeVisual.localScale = new Vector3(AttackRangeSize*2+0.3f, AttackRangeSize*2 + 0.3f, AttackRangeSize * 2 + 0.3f);
        maxUnitsAttackingAtOnce = currentUnitHealthComponent.UnitData.MaxUnitsAttackingAtOnce;
        nextAttackTime = Time.time;

        particleEffects = GetComponent<Particles>();// for poison cloud
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start()
    {
        if (currentUnitHealthComponent.UnitData.TeamName != "Plant")
        {
            direction = GetDirection();
            if (direction.x > 0)
            {
               transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x < 0)
            {
               transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        
        if(animator != null)
        {
            String currentStateName = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            float randomNormalizedTime = UnityEngine.Random.Range(0f, 1f);
            animator.Play(currentStateName, -1, randomNormalizedTime);
        }
    }

    private void OnDestroy()
    {
        Events.OnAddDamageValue -= AddDamage;
        Events.OnAddAttackSpeedValue -= AddAttackSpeed;
        Events.OnAddAttackRangeValue -= AddAttackRange;
        CurrentUnitAttackRange.OnEnemyEnteredAttackRange -= AddEnemyToTargets;
        CurrentUnitAttackRange.OnEnemyExitedAttackRange -= RemoveEnemyFromTargets;
    }

    void RemoveEnemyFromTargets(Health enemy) {
        if (enemy.UnitData.TeamName != currentUnitHealthComponent.UnitData.TeamName)
        {
            targetsInRange.Remove(enemy);
        }
    }
    void AddEnemyToTargets(Health enemy)
    {
        if (enemy.UnitData.TeamName != currentUnitHealthComponent.UnitData.TeamName)
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
            combatText.transform.localScale = combatText.transform.localScale * 0.5f;
            combatText.transform.DOScale(combatText.transform.localScale * 1.5f, 1f).OnComplete(tweenCallback);
        }
    }

    void AddAttackRange(string unitName, float rangesize) {
        if (gameObject.name.StartsWith(unitName))
        {
            AttackRangeSize += rangesize;
            ChangeAttackRange();
            GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + rangesize;
            combatText.transform.Find("AttackRange").gameObject.SetActive(true);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
            TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
            combatText.transform.localScale = combatText.transform.localScale * 0.5f;
            combatText.transform.DOScale(combatText.transform.localScale * 1.5f, 1f).OnComplete(tweenCallback);
        }
    }
    public void ChangeAttackRange() {
        CurrentUnitAttackRange.GetComponent<CircleCollider2D>().radius = AttackRangeSize;
        var attackRangeVisual = CurrentUnitAttackRange.transform.Find("AttackRangeVisual");
        // shown a little bigger because then it is more logical to player (they cant see pixels)
        attackRangeVisual.localScale = new Vector3(AttackRangeSize * 2 + 0.3f, AttackRangeSize * 2 + 0.3f, AttackRangeSize * 2 + 0.3f);
    }

    void AddAttackSpeed(string unitName, float attackSpeed)
    {
        if (gameObject.name.StartsWith(unitName))
        {
            AttackSpeed += attackSpeed;
            GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(transform.position.x + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.y + UnityEngine.Random.Range(-0.25f, 0.25f), transform.position.z), Quaternion.identity);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().text = "+" + attackSpeed;
            combatText.transform.Find("AttackSpeed").gameObject.SetActive(true);
            combatText.transform.GetChild(0).GetComponent<TextMeshPro>().color = Color.green;
            TweenCallback tweenCallback = () => { Destroy(combatText.gameObject); };
            combatText.transform.localScale = combatText.transform.localScale * 0.5f;
            combatText.transform.DOScale(combatText.transform.localScale * 1.5f, 1f).OnComplete(tweenCallback);
        }
    }

    public void Update()
    {
        int sortingOrder = 21-Mathf.RoundToInt(transform.position.y)+10;
        spriteRenderer.sortingOrder = sortingOrder;

        if ((movement != null && !isFighting) || (movement != null && targetsInRange.Count == 0))// non-attacking zombie moves to closest enemy
        {
            movement.Move(direction);
            if (animator != null && direction != Vector2.zero)
            {
                animator.SetBool("IsWalking", true);
                if (currentUnitHealthComponent.UnitData.TeamName == "Zombie")
                {
                    animator.SetBool("IsAttacking", false);
                }
            }
        }
        else if (movement != null && isFighting) {
            if (animator != null)
            {
                animator.SetBool("IsWalking", false);
                if (currentUnitHealthComponent.UnitData.TeamName == "Zombie") {
                    animator.SetBool("IsAttacking", true);
                }
            }
        }
        else if (movement == null)
        {
            currentTargets = GetNClosestTargets();
        }

        if (targetsInRange.Count > 0)
        {
            currentTargets = GetNClosestTargets();
            isFighting = true;
            if (nextAttackTime <= Time.time)
            {
                foreach (var target in currentTargets) {
                    if (target != null)
                    {
                        if (ProjectilePrefab != null) // for ranged fighters
                        {
                            Projectile projectile = Instantiate<Projectile>(ProjectilePrefab);
                            projectile.SetShooter(currentUnitHealthComponent.UnitData.TeamName, AttackDamage, target);
                            Vector3 firePoint = new Vector3((float)(gameObject.transform.position.x + 0.05), (float)(gameObject.transform.position.y + 0.244), 0);
                            projectile.transform.position = firePoint;
                            projectile.Target = target;
                            AttackSound.Play();
                            CombatDamageTexts(target);
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
                            target.transform.localScale = new Vector3(target.transform.localScale.x - 0.0015f * AttackDamage, target.transform.localScale.y - 0.0015f * AttackDamage, target.transform.localScale.z - 0.0015f * AttackDamage);
                            var targetAttackRange = target.transform.Find("AttackRange");
                            targetAttackRange.localScale = new Vector3(targetAttackRange.localScale.x + 0.0015f * AttackDamage, targetAttackRange.localScale.y + 0.0015f * AttackDamage, targetAttackRange.localScale.z + 0.0015f * AttackDamage);

                        }
                    }
                }               
                nextAttackTime = Time.time + 1/AttackSpeed; // the bigger the attackspeed the faster it hits
            }
        }
        else {
            isFighting = false;
            if (currentUnitHealthComponent.UnitData.TeamName != "Plant")
            {
                direction = GetDirection();
                if (direction.x > 0) {
                    transform.localScale = new Vector3(1, 1, 1);
                }else if (direction.x < 0) {
                    transform.localScale = new Vector3(-1, 1, 1);
                }
            }
        }

    }

    public void CombatDamageTexts(Health target)
    {
        GameObject combatText = Instantiate(CombatTextPrefab, new Vector3(target.transform.position.x + UnityEngine.Random.Range(-0.5f, 0.5f), target.transform.position.y + UnityEngine.Random.Range(-0.5f, 0.5f), target.transform.position.z), Quaternion.identity);
        combatText.transform.Find("combat_text").GetComponent<TextMeshPro>().text = "-" + AttackDamage;
        combatText.transform.Find("HealthNeg").gameObject.SetActive(true);
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
        if (currentUnitHealthComponent.UnitData.TeamName == "Zombie")
        {
            enemies = EntityController.Instance.PlantCharacters;
        }
        else if (currentUnitHealthComponent.UnitData.TeamName == "Plant")
        {
            enemies = EntityController.Instance.ZombieCharacters;
        }
        
        if (targetsInRange.Count > 0)
        {
            foreach (Health target in targetsInRange)
            {
                if (target != null)
                {
                    float dist = Vector2.Distance(target.transform.position, transform.position);
                    targetsWithDist.Add((target, dist));
                }
                else {
                    targetsInRange.Remove(target);
                }
            }
        }
        else {
            foreach (Health enemy in enemies)
            {
                if (enemy != null)
                {
                    float dist = Vector2.Distance(enemy.transform.position, transform.position);
                    targetsWithDist.Add((enemy, dist));
                }
            }
        }


        targetsWithDist.Sort((x, y) => x.Item2.CompareTo(y.Item2));

        for (int i = 0; i < Math.Min(targetsWithDist.Count, maxUnitsAttackingAtOnce); i++) {
            nClosestTargets.Add(targetsWithDist[i].Item1);
        }
        return nClosestTargets;
    }
}
