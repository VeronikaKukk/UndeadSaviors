using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public bool IsPotionPickedUp;

    void Awake()
    {
        Instance = this;
        IsPotionPickedUp = false;
    }

    public void SetPotionPickedUp(bool value)
    {
        IsPotionPickedUp = value;
    }
}
