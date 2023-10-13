using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float MovementSpeed;

    public void Move(Vector2 value) 
    {
        transform.position += (Vector3)value * MovementSpeed * Time.deltaTime;
    }
}
