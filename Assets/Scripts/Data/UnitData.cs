using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/UnitData")]
public class UnitData : ScriptableObject 
{
    public string TeamName;
    public List<GameObject> DroppablePotions;
    public string UnitName;
    public int CurrencyAmountOnDeath;
    public float MaxHealth;
    public float MovementSpeed;
    public float AttackDamage;
    public float AttackSpeed;
    public float AttackRangeSize;
}
