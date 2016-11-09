using UnityEngine;
using System.Collections;

public abstract class Weapon : MonoBehaviour {

    public float fireRate;
    public bool fireOnSingleHit;

    private float fireTimer;

    void Start()
    {
        fireTimer = float.MaxValue;
    }

	// Update is called once per frame
	void Update () {
        // fire if fire axis is fired
        if (fireOnSingleHit && Input.GetKeyDown(KeyCode.Space))
        {
            fireTimer = 1f / fireRate;
            Fire();
        }

        // fire
        if (Input.GetKey(KeyCode.Space))
            Fire();

    }

    void Fire()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < 1f / fireRate)
            return;

        fireTimer = 0;

        // instantiate projectile
        CreateProjectiles();
    }

    public abstract void CreateProjectiles();
}
