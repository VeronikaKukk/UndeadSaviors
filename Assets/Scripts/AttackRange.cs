using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackRange : MonoBehaviour
{    
    public event Action<Health> OnEnemyEnteredAttackRange;
    public event Action<Health> OnEnemyExitedAttackRange;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit")) { // if the collider is the units' own collider not its' attackrange
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                OnEnemyEnteredAttackRange.Invoke(enemyHealth);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Unit"))// if the collider is the units' own collider not its' attackrange
        {
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                OnEnemyExitedAttackRange.Invoke(enemyHealth);
            }
        }
    }
}
