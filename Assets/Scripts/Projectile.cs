using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 20f;
    public Health Target;

    private GameObject shooter;
    private float attackDamage;
    private Health triggerTarget;

    public void SetShooter(GameObject shooter, float attackDamage, Health target)
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
            if ((shooter.name.Contains("Plant") && triggerTarget.UnitData.TeamName == "Zombie") ||
                (shooter.name.Contains("Zombie") && triggerTarget.UnitData.TeamName == "Plant"))
            {
                Debug.Log("shooting scucceed for " + shooter.name);
                triggerTarget.CurrentHealth -= attackDamage;
                Attacking.Instance.CombatDamageTexts(triggerTarget);
                GameObject.Destroy(gameObject);
            }
        }
    }

}
