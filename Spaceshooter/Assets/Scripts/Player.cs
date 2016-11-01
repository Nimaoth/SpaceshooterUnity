using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    //
    public float playerSpeed;
    public float maxHealth;
    public float fireRate;
    public GameObject projectilePrefab;

    //
    private float currentHealth;
    private float fireTimer;
    private float inverseFireRate;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        inverseFireRate = 1f / fireRate;
        fireTimer = inverseFireRate;
    }

    // Update is called once per frame
    void Update()
    {
        // calculation of camera bounds, found on http://answers.unity3d.com/questions/230190/how-to-get-the-width-and-height-of-a-orthographic.html
        // ------------------------------
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        // ------------------------------

        // the bounds of the screen, past it the player will be moved to the other side
        // half of the window size + half of the players size (which is 1.5)
        float bounds = width / 2f + 0.75f;

        // Move Player depending on Input
        float amtToMove = Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime;
        transform.Translate(Vector3.right * amtToMove);

        // Screen wrap
        if (transform.position.x < -bounds)
            transform.position = new Vector3(bounds, transform.position.y, transform.position.z);

        if (transform.position.x > bounds)
            transform.position = new Vector3(-bounds, transform.position.y, transform.position.z);

        // fire if fire axis is fired
        if (Input.GetKeyDown(KeyCode.Space))
        {
            fireTimer = inverseFireRate;
            Fire();
        }

        // fire
        if (Input.GetKey(KeyCode.Space))
            Fire();
    }

    void Fire()
    {
        fireTimer += Time.deltaTime;
        if (fireTimer < inverseFireRate)
            return;

        fireTimer = 0;

        // instantiate projectile
        var position = transform.position + Vector3.up * (transform.localScale.y * 0.6f);
        Instantiate(projectilePrefab, position, Quaternion.identity);
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // game over
            Debug.Log("Game Over!");
        }
    }
}
