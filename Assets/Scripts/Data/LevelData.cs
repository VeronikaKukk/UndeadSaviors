using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/LevelData")]

public class LevelData : ScriptableObject
{
    public string LevelName;
    public int LevelNumber;
    public string SceneName;
    public float Gametime;
    public int StartingMoney;

    public List<UnitData> Plants;
    public List<ShopData> Zombies;
}
