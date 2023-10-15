using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/PotionData")]
public class PotionData : ScriptableObject
{
    public string PotionName;// health speed or damage
    public float BuffAmount;// number that shows how much buff you get
    public float AliveTime; // how long the potion stays on the field

}

