using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooting : MonoBehaviour
{
    public static ProjectileShooting Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void Shoot(Projectile ProjectilePrefab, Health target, GameObject currentUnit)
    {
        Projectile projectile1 = Instantiate<Projectile>(ProjectilePrefab);
        Vector3 firePoint = new Vector3((float)(currentUnit.transform.position.x + 0.05), (float)(currentUnit.transform.position.y + 0.244), 0);
        projectile1.transform.position = firePoint;
        projectile1.Target = target;
    }

}
