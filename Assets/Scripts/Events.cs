using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Events
{
    public static event Action<int> OnSetMoney;
    public static void SetMoney(int value) => OnSetMoney?.Invoke(value);

    public static event Func<int> OnGetMoney;
    public static int GetMoney() => OnGetMoney?.Invoke() ?? 0; 

    
    public static event Action<ShopData> OnZombieSelected;
    public static void SelectZombie(ShopData data) => OnZombieSelected?.Invoke(data);


    public static event Action<string, PotionData> OnApplyPotion;
    public static void ApplyPotion(string unitName, PotionData data) => OnApplyPotion?.Invoke(unitName, data);


    public static event Action<string ,float> OnAddDamageValue;
    public static void AddDamageValue(string unitName, float value) => OnAddDamageValue?.Invoke(unitName, value);




    public static event Action<string,float> OnAddAttackSpeedValue;
    public static void AddAttackSpeedValue(string unitName, float value) => OnAddAttackSpeedValue?.Invoke(unitName, value);



    public static event Action<string, float> OnAddMovementSpeedValue;
    public static void AddMovementSpeedValue(string unitName, float value) => OnAddMovementSpeedValue?.Invoke(unitName, value);



    public static event Action<string, float> OnAddMaxHealthValue;
    public static void AddMaxHealthValue(string unitName, float value) => OnAddMaxHealthValue?.Invoke(unitName, value);



    public static event Action<bool> OnEndLevel;
    public static void EndLevel(bool isWin) => OnEndLevel?.Invoke(isWin);
}
