using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/ShopData")]
public class ShopData : ScriptableObject
{
    public string Name;
    public int Price;
    public Sprite Icon;
    public GameObject ZombiePrefab;
}
