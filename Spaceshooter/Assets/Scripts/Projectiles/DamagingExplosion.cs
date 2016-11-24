using UnityEngine;
using System.Collections;

public class DamagingExplosion : MonoBehaviour {

    public float radius;
    public float damage;

    private ParticleSystem ps;

	void Start () {
        ps = GetComponent<ParticleSystem>();

        GameObject enemies = GameObject.Find("Enemies");

        float radiusSq = radius * radius;
        Debug.Log("Radius: " + radius);

        for (int i = 0; i < enemies.transform.childCount; i++)
        {
            Transform t = enemies.transform.GetChild(i);
            if ((t.position - transform.position).sqrMagnitude <= radiusSq)
            {
                Enemy e = t.GetComponent<Enemy>();
                e.TakeDamage(damage);

                Player.Score += 100;
                Player.UpdateStats();
            }
        }
	}
	
	void Update () {
        Debug.Log(ps.IsAlive());
        if (!ps.IsAlive())
            Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
