using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;

public class Projectile : MonoBehaviour
{
    public float ProjectileSpeed = 20f;
    public Health Target;

    void Update()
    {
        if (Target != null)
        {
            if (Vector2.Distance(transform.position, Target.transform.position) < 0.1)
            {
                GameObject.Destroy(gameObject); // hävitab projectile
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, Target.transform.position, Time.deltaTime * ProjectileSpeed);
            }
        }
        else
        {
            GameObject.Destroy(gameObject);
        }
    }
}
