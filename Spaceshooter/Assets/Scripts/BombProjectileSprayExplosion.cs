using UnityEngine;
using System.Collections;

public class BombProjectileSprayExplosion : MonoBehaviour {

    public int laserCount = 5;
    public GameObject sprayProjectiles;

	// Use this for initialization
	void Start () {
        float angle = 0;
        float angleStep = 360f / laserCount;
	    for (int i = 0; i < laserCount; i++, angle += angleStep)
        {
            Instantiate(sprayProjectiles, transform.position, Quaternion.Euler(0, 0, angle));
        }
	}
}
