using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrap : MonoBehaviour
{
    private AttackRange attackRange;
    void Awake()
    {
        attackRange = GetComponent<AttackRange>();
        attackRange.OnEnemyEnteredAttackRange += KillUnit;
        attackRange.OnEnemyExitedAttackRange += DoNothing;
    }

    void KillUnit(Health enemy)
    {
        enemy.CurrentHealth -= 1000;
    }
    private void OnDestroy()
    {
        attackRange.OnEnemyEnteredAttackRange -= KillUnit;
        attackRange.OnEnemyExitedAttackRange -= DoNothing;

    }
    void DoNothing(Health enemy) { }
}
