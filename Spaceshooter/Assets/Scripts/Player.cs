using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public static int Score = 0;
    public static int Lives = 3;
    public static Text playerStats;
    

    //
    public float playerSpeed;
    public float fireRate;

    //
    private float fireTimer;
    private float inverseFireRate;

    // Use this for initialization
    void Start()
    {
        inverseFireRate = 1f / fireRate;
        fireTimer = inverseFireRate;

        playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
        UpdateStats();
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
        float boundsX = width / 2f + 0.75f;
        float boundsY = height / 2f;

        // Move Player depending on Input
        float hor = Input.GetAxis("Horizontal");
        float amtToMoveX = hor * playerSpeed * Time.deltaTime;
        float amtToMoveY = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;

        transform.Translate(new Vector3(amtToMoveX, amtToMoveY), Space.World);

        transform.rotation = Quaternion.Slerp(Quaternion.identity, Quaternion.Euler(0, Mathf.Sign(hor) * -30, 0), Mathf.Abs(hor));

        // Screen wrap
        if (transform.position.x < -boundsX)
            transform.position = new Vector3(boundsX, transform.position.y, transform.position.z);

        if (transform.position.x > boundsX)
            transform.position = new Vector3(-boundsX, transform.position.y, transform.position.z);

        if (transform.position.y > boundsY + 0.25f)
            transform.position = new Vector3(transform.position.x, boundsY + 0.25f, transform.position.z);
        if (transform.position.y < -boundsY + 1.75f)
            transform.position = new Vector3(transform.position.x, -boundsY + 1.75f, transform.position.z);
    }

    public static void UpdateStats()
    {
        playerStats.text = "Score: " + Score.ToString() + "\nLives: " + Lives.ToString();
    }
}
