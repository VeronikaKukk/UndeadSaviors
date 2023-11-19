using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInfo : MonoBehaviour
{
    public List<UnitData> units;
    public UnitInfoCard UnitInfoCardPrefab;
    public GameObject InfoPanel;

    void Start()
    {
        foreach (var unit in units) {
            UnitInfoCard card = Instantiate<UnitInfoCard>(UnitInfoCardPrefab, InfoPanel.transform);
            card.SetData(unit);
        }
    }
}
