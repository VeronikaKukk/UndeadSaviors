using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieFactory : MonoBehaviour
{

    public static ZombieFactory Instance;
    public GameObject[] UnitTypes;
    private Dictionary<string, GameObject> unitNamesAndPrefabs;
    private Dictionary<GameObject, List<PotionData>> appliedPotionsOnUnit;

    public void Awake()
    {
        unitNamesAndPrefabs = new Dictionary<string, GameObject>();
        appliedPotionsOnUnit = new Dictionary<GameObject, List<PotionData>>();

        foreach (var unit in UnitTypes)
        {
            unitNamesAndPrefabs.Add(unit.name, unit);
            appliedPotionsOnUnit.Add(unit, new List<PotionData>());
        }

        Instance = this;

        Events.OnApplyPotion += ApplyPotion;
    }
    void ApplyPotion(string unitName, PotionData potionData)
    {
        if (unitNamesAndPrefabs.ContainsKey(unitName))
        {
            GameObject prefab = unitNamesAndPrefabs[unitName];
            appliedPotionsOnUnit[prefab].Add(potionData);
        }
    }
    public GameObject ApplyPotionsOnUnit(GameObject zombie, string unitName)
    {
        if (unitNamesAndPrefabs.ContainsKey(unitName))
        {
            GameObject unitPrefab = unitNamesAndPrefabs[unitName];
            List<PotionData> appliedPotions = appliedPotionsOnUnit[unitPrefab];
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
                }
                else if (potionData.PotionName == "Speed")
                {
                    //Debug.Log("new zombie " + unitName + "got speed potion");

                    var movespeed = zombie.GetComponent<Movement>();
                    var attackspeed = zombie.GetComponent<Attacking>();

                    movespeed.MovementSpeed += potionData.BuffAmount;
                    attackspeed.AttackSpeed += potionData.BuffAmount;
                }
            }

            return unitPrefab;
        }
        return null;

    }

    public void OnDestroy()
    {
        Events.OnApplyPotion -= ApplyPotion;

    }
}