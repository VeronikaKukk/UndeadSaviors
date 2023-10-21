using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Events
{
    public static event Action<int> OnSetMoney;
    public static void SetMoney(int value) => OnSetMoney?.Invoke(value);

    public static event Func<int> OnGetMoney;
    public static int GetMoney() => OnGetMoney?.Invoke() ?? 0; 

    
    public static event Action<ShopData> OnZombieSelected;
    public static void SelectZombie(ShopData data) => OnZombieSelected?.Invoke(data);



    public static event Action<float> OnSetDamage;
    public static void SetDamage(float value) => OnSetDamage?.Invoke(value);

    public static event Func<float> OnGetDamage;
    public static float GetDamage() => OnGetDamage?.Invoke() ?? 0;


    public static event Action<float> OnSetAttackSpeed;
    public static void SetAttackSpeed(float value) => OnSetAttackSpeed?.Invoke(value);

    public static event Func<float> OnGetAttackSpeed;
    public static float GetAttackSpeed() => OnGetAttackSpeed?.Invoke() ?? 0;


    public static event Action<float> OnSetMovementSpeed;
    public static void SetMovementSpeed(float value) => OnSetMovementSpeed?.Invoke(value);

    public static event Func<float> OnGetMovementSpeed;
    public static float GetMovementSpeed() => OnGetMovementSpeed?.Invoke() ?? 0;


    public static event Action<float> OnSetMaxHealth;
    public static void SetMaxHealth(float value) => OnSetMaxHealth?.Invoke(value);

    public static event Func<float> OnGetMaxHealth;
    public static float GetMaxHealth() => OnGetMaxHealth?.Invoke() ?? 0;

}
