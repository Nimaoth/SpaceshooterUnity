using UnityEngine;
using System.Collections;
using System;

public class Laser : Weapon
{
    public GameObject projectile;

    public override void CreateProjectiles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform t = transform.GetChild(i);

            Instantiate(projectile, t.position, Quaternion.identity);
        }
    }
}
