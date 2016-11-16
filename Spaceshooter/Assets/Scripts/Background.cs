using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    //
    private float speed = 1.25;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        float move = speed * Time.deltaTime;
        transform.Translate(Vector3.down * move, Space.World);

        if (transform.position.y < -10.75f)
        {
            transform.position = new Vector3(transform.position.x, 14f, transform.position.z);
        }
	}
}
