using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
    
    //
    public float minSpeed;
    public float maxSpeed;
    public float maxHealh;

    public float damage;

    //
    private float currentSpeed;
    private float currentHealth;

    private new Collider collider;
    private new Renderer renderer;

    // Use this for initialization
    void Start () {
        collider = GetComponent<Collider>();
        renderer = GetComponent<Renderer>();

        Reset();
	}
	
	// Update is called once per frame
	void Update () {
        float amtToMove = currentSpeed * Time.deltaTime;
        transform.Translate(0, -amtToMove, 0);

        if (transform.position.y <= -5)
        {
            Reset();
        }
	}

    void Reset()
    {
        currentSpeed = Random.Range(minSpeed, maxSpeed);
        transform.position = new Vector3(Random.Range(-6f, 6f), 7, 0);

        currentHealth = maxHealh;

        collider.enabled = true;
        renderer.enabled = true;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            collider.enabled = false;
            renderer.enabled = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}
