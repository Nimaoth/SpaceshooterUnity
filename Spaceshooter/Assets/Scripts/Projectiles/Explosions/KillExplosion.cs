using UnityEngine;
using System.Collections;

public class KillExplosion : MonoBehaviour {

    private new ParticleSystem particleSystem;

	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
	}
	
	void Update () {
	    if (!particleSystem.IsAlive())
        {
            Destroy(gameObject);
        }
	}
}
