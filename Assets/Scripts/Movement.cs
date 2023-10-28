using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MovementSpeed;

    private void Awake()
    {
        Events.OnAddMovementSpeedValue += AddMovementSpeed;
        // this was in start before
        Health health = GetComponent<Health>();
        MovementSpeed = health.UnitData.MovementSpeed;
    }
    private void Start()
    {
        
    }
    private void OnDestroy()
    {
        Events.OnAddMovementSpeedValue -= AddMovementSpeed;
    }

    void AddMovementSpeed(string unitName, float speed)
    {
        if(gameObject.name.StartsWith(unitName)) { 
            MovementSpeed += speed;
        }
    }

    public void Move(Vector2 value) 
    {
        transform.position += (Vector3)value * MovementSpeed * Time.deltaTime;
    }
}
