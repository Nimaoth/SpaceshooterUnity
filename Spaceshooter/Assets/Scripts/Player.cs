using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class Player : MonoBehaviour
{
    enum PlayerState
    {
        normal,
        dash,
        dead
    }

    private static int _score;
    public static int Score
    {
        get
        {
            return _score;
        }
        set
        {
            int oldScore = _score;
            _score = value;
            Instance.OnScoreChange(oldScore, value);
        }
    }
    public static int Lives = 1;
    public static int Missed = 0;
    public static int ShieldAmount = 0;
    public static Text playerStats;
    private static Player Instance;


    //
    public float playerSpeed;
    public float rotationSpeed;
    public float dashSpeed;
    public float dashFade;
    public float respawnTime;
    public GameObject explosionPrefab;
    public List<GameObject> weaponPrefabs;
    public int[] weaponScores;
    public StartGameButtonBehaviour uiManager;


    //
    private float leftDoubleTapTime = 0;
    private float rightDoubleTapTime = 0;
    private float currentDashSpeed = 0;

    private PlayerState state;

    private GameObject currentWeapon;
    private int currentWeaponIndex = 0;

    private GameObject shield;

    // Use this for initialization
    void Start()
    {
        Lives = 1;
        Missed = 0;
        _score = 0;
        ShieldAmount = 100;

        Instance = this;

        playerStats = GameObject.Find("PlayerStats").GetComponent<Text>();
        UpdateStats();

        state = PlayerState.normal;

        var weapon = transform.FindChild("Weapon");
        currentWeapon = (GameObject) Instantiate(weaponPrefabs[0], transform.position, Quaternion.identity, weapon);

        shield = transform.Find("Shield").gameObject;
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
        float boundsX = width / 2f + 0.75f;// - 0.5f;
        float boundsY = height / 2f;


        switch (state)
        {
            case PlayerState.normal:
                UpdateStateNormal();
                break;
            case PlayerState.dash:
                UpdateStateDash();
                break;
            case PlayerState.dead:
                break;
        }
        

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

        if (doubleTapLeft || Input.GetKeyDown(KeyCode.Q))
        {
            state = PlayerState.dash;
            currentDashSpeed = -dashSpeed;
        }

        if (doubleTapRight || Input.GetKeyDown(KeyCode.E))
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
        playerStats.text = "Score: " + Score.ToString() + "\nLives: " + Lives.ToString() + "\nMissed: " + Missed.ToString();
    }

    public void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Enemy")
        {
            if (ShieldAmount > 0)
            {
                ShieldAmount -= collider.GetComponentInParent<Enemy>().damage;
                if (ShieldAmount <= 0)
                {
                    ShieldAmount = 0;
                    shield.GetComponent<Shield>().UpdateStats();
                    shield.SetActive(false);
                }
            }
            else
            {
                Player.Lives--;
                Player.UpdateStats();
                StartCoroutine(DestroyShip());
            }

            collider.GetComponentInParent<Enemy>().Reset();
        }
    }

    private void Lose()
    {
        uiManager.LoadLevelByName("Lose");
    }

    private void OnScoreChange(int oldValue, int newValue)
    {
        int next = currentWeaponIndex + 1;
        if (next > 0 && next < weaponScores.Length && next < weaponPrefabs.Count)
        {
            if (oldValue < weaponScores[next] && newValue >= weaponScores[next])
            {
                Destroy(currentWeapon);
                currentWeapon = (GameObject)Instantiate(weaponPrefabs[next], transform.position, Quaternion.identity, transform.FindChild("Weapon"));

                currentWeaponIndex = next;
            }
        }

        if (_score >= 3000)
        {
            uiManager.LoadLevelByName("Win");
        }
    }

    IEnumerator DestroyShip()
    {
        Disable();
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        transform.position = new Vector3(0, 0, 0);

        yield return new WaitForSeconds(1.5f);
        
        if (Player.Lives == 0)
        {
            Lose();
            yield return null;
        }

        Enable();
        state = PlayerState.normal;

        currentWeaponIndex = 0;
        Destroy(currentWeapon);
        currentWeapon = (GameObject)Instantiate(weaponPrefabs[0], transform.position, Quaternion.identity, transform.Find("Weapon"));
    }

    private void Enable()
    {
        GetComponentInChildren<Renderer>().enabled = true;
        GetComponentInChildren<Collider>().enabled = true;

        transform.Find("Weapon").gameObject.SetActive(true);
    }
    private void Disable()
    {
        GetComponentInChildren<Renderer>().enabled = false;
        GetComponentInChildren<Collider>().enabled = false;

        transform.Find("Weapon").gameObject.SetActive(false);
    }
}
