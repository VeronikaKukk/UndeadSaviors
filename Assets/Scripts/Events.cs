using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static event Action<int> OnSetMoney;
    public static void SetMoney(int value) => OnSetMoney?.Invoke(value);

    public static event Func<int> OnGetMoney;
    public static int GetMoney() => OnGetMoney?.Invoke() ?? 0; // kui on null - siis 0

    
    public static event Action<ShopData> OnZombieSelected;
    public static void SelectZombie(ShopData data) => OnZombieSelected?.Invoke(data);

}
