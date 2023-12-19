using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFactory : MonoBehaviour
{

    public static ZombieFactory Instance;
    public GameObject[] UnitTypes;
    private Dictionary<string, List<PotionData>> appliedPotionsOnUnit;

    public void Awake()
    {
        appliedPotionsOnUnit = new Dictionary<string, List<PotionData>>();

        foreach (var unit in UnitTypes)
        {
            appliedPotionsOnUnit.Add(unit.name, new List<PotionData>());
        }

        Instance = this;

        Events.OnApplyPotion += ApplyPotion;
    }

    void ApplyPotion(string unitName, PotionData potionData)
    {
        if (appliedPotionsOnUnit.ContainsKey(unitName))
        {
            appliedPotionsOnUnit[unitName].Add(potionData);
        }
    }
    public void ApplyPotionsOnUnit(GameObject zombie, string unitName)
    {
        if (appliedPotionsOnUnit.ContainsKey(unitName))
        {
            List<PotionData> appliedPotions = appliedPotionsOnUnit[unitName];
            foreach (PotionData potionData in appliedPotions)
            {
                if (potionData.PotionName == "Damage")
                {
                    //Debug.Log("new zombie " + unitName + "got damage potion");
                    var attack = zombie.GetComponent<Attacking>();
                    attack.AttackDamage += potionData.BuffAmount;
                }
                else if (potionData.PotionName == "Health")
                {
                    //Debug.Log("new zombie " + unitName + "got health potion");

                    var health = zombie.GetComponent<Health>();
                    health.MaxHealth += potionData.BuffAmount;
                    health.CurrentHealth += potionData.BuffAmount;
                }
                else if (potionData.PotionName == "MovementSpeed")
                {
                    //Debug.Log("new zombie " + unitName + "got movement speed potion");
                    var movespeed = zombie.GetComponent<Movement>();
                    movespeed.MovementSpeed += potionData.BuffAmount;
                }else if (potionData.PotionName == "AttackSpeed")
                {
                    //Debug.Log("new zombie " + unitName + "got attack speed potion");
                    var attackspeed = zombie.GetComponent<Attacking>();
                    attackspeed.AttackSpeed += potionData.BuffAmount;
                }
                else if (potionData.PotionName == "AttackRange")
                {
                    //Debug.Log("new zombie " + unitName + "got attack speed potion");
                    var attackrange = zombie.GetComponent<Attacking>();
                    attackrange.AttackRangeSize += potionData.BuffAmount;
                    attackrange.ChangeAttackRange();
                }
            }
        }
    }
    public void OnDestroy()
    {
        Events.OnApplyPotion -= ApplyPotion;

    }
}