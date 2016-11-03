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

	// Use this for initialization
	void Start () {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        float amtToMove = projectileSpeed * Time.deltaTime;
        transform.Translate(Vector3.up * amtToMove);

        if (transform.position.y > 6.4f)
            Destroy(gameObject);
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            collider.enabled = false;
            renderer.enabled = false;

            other.GetComponent<Enemy>().TakeDamage(damage);

            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            Player.Score += 100;
            Player.UpdateStats();
        }
    }
}
