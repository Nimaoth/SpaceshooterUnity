using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Projectile : MonoBehaviour {

    public float projectileSpeed;
    public float damage;

    public GameObject explosionPrefab;

    // 
    private new Collider collider;
    private new Renderer renderer;
    
    private float lifeTime;

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
        renderer = GetComponentInChildren<Renderer>();

        lifeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
        float amtToMove = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);

        lifeTime += Time.deltaTime;
        if (lifeTime > 0.5 && !renderer.isVisible)
        {
            Destroy(gameObject);
            return;
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            collider.enabled = false;
            renderer.enabled = false;

            other.GetComponent<Enemy>().TakeDamage(damage);

            if (explosionPrefab != null)
            {
                Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            }


            Player.Score += 100;
            Player.UpdateStats();
        }
    }
}
