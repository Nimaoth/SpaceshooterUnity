using UnityEngine;
using System.Collections;

public class AimingRocket : MonoBehaviour {

    //
    public float speed = 3;
    public float rotationSpeed = 0.1f;
    public float lifeTime = 2;
    public GameObject explosionPrefab;


    //
    private GameObject enemies;
    private Transform target;

    private float spawnTime;

	// Use this for initialization
	void Start () {
        enemies = GameObject.Find("Enemies");

        target = FindClosestEnemy();

        spawnTime = Time.time;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (target != null)
        {
            Vector3 dir = target.position - transform.position;
            dir.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed);
        }



        transform.position += transform.up * speed * Time.deltaTime;

        if (Time.time >= spawnTime + lifeTime)
        {
            Explode();
        }
    }

    private Transform FindClosestEnemy()
    {
        Transform closest = null;
        float distanceSq = float.MaxValue;

        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Transform t = enemies.transform.GetChild(i);
            float dSq = (t.position - transform.position).sqrMagnitude;

            if (closest == null || dSq < distanceSq)
            {
                closest = t;
                distanceSq = dSq;
            }
        }

        return closest;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Explode();


            Player.Score += 100;
            Player.UpdateStats();
        }
    }

    private void Explode()
    {
        Destroy(gameObject);
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
