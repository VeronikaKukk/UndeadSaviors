using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 20f;
    public Health Target;

    private string shooter;
    private float attackDamage;
    private Health triggerTarget;

    public bool isElectricity;
    private bool hasExploded = false;
    public void SetShooter(string shooter, float attackDamage, Health target)
    {
        this.shooter = shooter;
        this.attackDamage = attackDamage;
        this.triggerTarget = target;
    }
    
    void Update()
    {
        if (Target != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * ProjectileSpeed);
            if (Target.transform.position.x - transform.position.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else if (Target.transform.position.x - transform.position.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        triggerTarget = collision.GetComponent<Health>();
        if (shooter != null && triggerTarget != null)
        {
            if ((shooter.Contains("Plant") && triggerTarget.UnitData.TeamName == "Zombie") ||
                (shooter.Contains("Zombie") && triggerTarget.UnitData.TeamName == "Plant"))
            {
                Debug.Log("shooting succeed for " + shooter+triggerTarget.UnitData.UnitName);
                triggerTarget.CurrentHealth -= attackDamage;
                collision.gameObject.GetComponent<Attacking>().CombatDamageTexts(triggerTarget, attackDamage);
                if (isElectricity && !hasExploded)
                {
                    TweenCallback tweenCallback = () => { Destroy(gameObject); };
                    gameObject.transform.DOScale(new Vector3(5.0f, 5.0f, 5.0f), 1).OnComplete(tweenCallback);
                    hasExploded = true;
                }
                else if (!isElectricity) {
                    GameObject.Destroy(gameObject);
                }
            }
        }
    }

}
