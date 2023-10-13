using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    public static EntityController Instance;
    // holds all zombie and plant units inside
    public List<Health> ZombieCharacters;
    public List<Health> PlantCharacters;

    private void Awake()
    {
        Instance = this;
    }
}
