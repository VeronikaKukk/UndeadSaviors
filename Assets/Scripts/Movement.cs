using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MovementSpeed;

    private void Awake()
    {
        Events.OnSetMovementSpeed += SetMovementSpeed;
        Events.OnGetMovementSpeed += GetMovementSpeed;
    }

    private void OnDestroy()
    {
        Events.OnSetMovementSpeed -= SetMovementSpeed;
        Events.OnGetMovementSpeed -= GetMovementSpeed;
    }

    void SetMovementSpeed(float speed)
    {
        this.MovementSpeed = speed;
    }

    float GetMovementSpeed() => MovementSpeed;

    public void Move(Vector2 value) 
    {
        transform.position += (Vector3)value * MovementSpeed * Time.deltaTime;
    }
}
