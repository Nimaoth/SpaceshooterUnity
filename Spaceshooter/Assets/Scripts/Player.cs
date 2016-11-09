using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        normal,
        dash
    }

    public static int Score = 0;
    public static int Lives = 3;
    public static Text playerStats;
    

    //
    public float playerSpeed;
    public float rotationSpeed;
    public float fireRate;
    public float dashSpeed;
    public float dashFade;

    //
    private float fireTimer;
    private float inverseFireRate;

    private float leftDoubleTapTime = 0;
    private float rightDoubleTapTime = 0;

    private float currentDashSpeed = 0;

    private PlayerState state;

    // Use this for initialization
    void Start()
    {
        inverseFireRate = 1f / fireRate;
        fireTimer = inverseFireRate;

        playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
        UpdateStats();

        state = PlayerState.normal;
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
        float boundsX = width / 2f - 0.5f;// + 0.75f;
        float boundsY = height / 2f;


        switch (state)
        {
            case PlayerState.normal:
                UpdateStateNormal();
                break;
            case PlayerState.dash:
                UpdateStateDash();
                break;
        }
        

        // Screen wrap
        if (transform.position.x < -boundsX)
            transform.position = new Vector3(-boundsX, transform.position.y, transform.position.z);

        if (transform.position.x > boundsX)
            transform.position = new Vector3(boundsX, transform.position.y, transform.position.z);

        if (transform.position.y > boundsY + 0.25f)
            transform.position = new Vector3(transform.position.x, boundsY + 0.25f, transform.position.z);
        if (transform.position.y < -boundsY + 1.75f)
            transform.position = new Vector3(transform.position.x, -boundsY + 1.75f, transform.position.z);

    }

    private void UpdateStateNormal()
    {
        // Move Player depending on Input
        float hor = Input.GetAxis("Horizontal");
        float amtToMoveX = hor * playerSpeed * Time.deltaTime;
        float amtToMoveY = Input.GetAxis("Vertical") * playerSpeed * Time.deltaTime;

        transform.Translate(new Vector3(amtToMoveX, amtToMoveY), Space.World);

        Quaternion targetRotation = Quaternion.Euler(0, Input.GetAxisRaw("Horizontal") * -30, 0);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed);
        

        // dash
        bool doubleTapLeft = false;
        bool doubleTapRight = false;
        float doubleTimeTimeSpan = 0.2f;

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Time.time < leftDoubleTapTime + doubleTimeTimeSpan)
            {
                doubleTapLeft = true;
            }
            leftDoubleTapTime = Time.time;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Time.time < rightDoubleTapTime + doubleTimeTimeSpan)
            {
                doubleTapRight = true;
            }
            rightDoubleTapTime = Time.time;
        }

        if (doubleTapLeft)
        {
            state = PlayerState.dash;
            currentDashSpeed = -dashSpeed;
        }

        if (doubleTapRight)
        {
            state = PlayerState.dash;
            currentDashSpeed = dashSpeed;
        }
    }

    private float map(float f, float aMin, float aMax, float bMin, float bMax)
    {
        return ((f - aMin) / (aMax - aMin)) * (bMax - bMin) + bMin;
    }

    private void UpdateStateDash()
    {
        transform.Translate(new Vector3(currentDashSpeed * Time.deltaTime, 0), Space.World);


        transform.rotation = Quaternion.Euler(0, Mathf.Sign(currentDashSpeed) * map(Mathf.Abs(currentDashSpeed), 0, dashSpeed, -400, 0), 0);


        currentDashSpeed = Mathf.Lerp(currentDashSpeed, 0, dashFade);


        float av = Mathf.Abs(currentDashSpeed);

        if (av < playerSpeed)
        {
            UpdateStateNormal();
        }
        if (av < 1)
        {
            state = PlayerState.normal;
        }
    }

    public static void UpdateStats()
    {
        playerStats.text = "Score: " + Score.ToString() + "\nLives: " + Lives.ToString();
    }
}
