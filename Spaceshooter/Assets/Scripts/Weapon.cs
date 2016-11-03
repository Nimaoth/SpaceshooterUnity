using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour {

    public float fireRate;
    public bool fireOnSingleHit;
    public GameObject projectilePrefab;

    private float fireTimer;

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

        Debug.Log("ok");
    }

    void Fire()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < 1f / fireRate)
            return;

        fireTimer = 0;

        // instantiate projectile
        var position = transform.position + Vector3.up * (transform.localScale.y * 0.6f);
        Instantiate(projectilePrefab, position, Quaternion.identity);
    }
}
