using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public static EntityController Instance;
    // holds all zombie and plant units inside
    public List<Health> ZombieCharacters;
    public List<Health> PlantCharacters;
    public List<Collectable> Potions;
    public List<GameObject> Other;
    private void Awake()
    {
        Instance = this;
        ZombieCharacters = new List<Health>();
        PlantCharacters = new List<Health>();
        Potions = new List<Collectable>();
        Other = new List<GameObject>();

    }
    public void Reset()
    {
        ZombieCharacters = new List<Health>();
        PlantCharacters = new List<Health>();
        Potions = new List<Collectable>();
        Other = new List<GameObject>();
    }
}
