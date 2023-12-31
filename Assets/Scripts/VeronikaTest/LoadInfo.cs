using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInfo : MonoBehaviour
{
    public List<UnitData> PlantUnits;
    public List<UnitData> ZombieUnits;

    public UnitInfoCard UnitInfoCardPrefab;

    void Start()
    {
        foreach (var unit in PlantUnits) {
            UnitInfoCard card = Instantiate<UnitInfoCard>(UnitInfoCardPrefab, gameObject.transform);
            card.SetData(unit);
        }

        foreach (var unit in ZombieUnits)
        {
            UnitInfoCard card = Instantiate<UnitInfoCard>(UnitInfoCardPrefab, gameObject.transform);
            card.SetData(unit);
        }
    }
}
