using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody2D playerRB2D;
    public Vector3 bulletSpawnPoint;
    public Vector3 missileSpawnPoint;
    public GameObject shield;
    // public AudioSource sfxPickup;
    public float playerHealthCurrent;
    public float playerHealthMax = 10.0f;
    public float playerSpeed = 3.5f;
    public float maxPlayerSpeed = 8f;
    public float accelerationFactor = 0.02f;
    public bool controlScheme = true;
    public float shootCooldownTime = 0.2f;
    private bool shootCooldownToggle = false;
    private float nextFireTime = 0;
    public float shootMissleCooldownTime = 10f;
    private bool shootCooldownToggleMissile = false;
    private float nextMissileFireTime = 0;
    public bool playerHasMissile = false;
    private GameController theGameController = null;
    private float spriteSizeX = 0;
    private float spriteSizeY = 0;
    public bool turboEnabled;
    public float turboTime;
    public float shootCooldown;
    void Start()
    {
        shield = GameObject.Find("shield");
        shield.SetActive(false);
        playerAnimator = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
        bulletSpawnPoint.y = 0.4f;
        theGameController = FindObjectOfType<GameController>();
        spriteSizeX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        spriteSizeY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
        playerHealthCurrent = playerHealthMax;
        turboEnabled = false;
    }
    void Update()
    {
        if (nextFireTime > Time.time)
        {
            shootCooldown = nextFireTime - Time.time;
        }
        else shootCooldown = 0;
        // Randomize which side missiles spawn from
        if (Random.value < 0.5f)
        {
            missileSpawnPoint.x = -0.2f;
        }
        else
        {
            missileSpawnPoint.x = 0.2f;
        }

        // Test block for explode animation
        if (Input.GetKeyDown(KeyCode.K))
        {
            // playerAnimator.SetBool("killed", true);
        }

        // Toggle control scheme with M key
        if (Input.GetKeyDown(KeyCode.M))
        {
            controlScheme = !controlScheme;
            playerSpeed = 3.5f;
        }

        Vector3 pos = transform.position;

        if (controlScheme && theGameController.GetComponent<GameController>().isGameRunning()) // Default scheme is player locked to mouse
        {
            pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;
        }
        else
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            {
                if (playerSpeed < maxPlayerSpeed)
                {
                    playerSpeed += accelerationFactor;
                }
                else if (playerSpeed == maxPlayerSpeed)
                {
                    playerSpeed = maxPlayerSpeed;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    pos += ((playerSpeed * Time.smoothDeltaTime) * transform.up);
                }

                if (Input.GetKey(KeyCode.S))
                {
                    pos -= ((playerSpeed * Time.smoothDeltaTime) * transform.up);
                }
            }

            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            {
                playerAnimator.SetBool("strafing", true);
                if (Input.GetKey(KeyCode.A))
                {
                    if (transform.localScale.x != 1)
                    {
                        FlipX();
                    }
                    pos.x -= (playerSpeed * Time.smoothDeltaTime);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    if (transform.localScale.x != -1)
                    {
                        FlipX();
                    }
                    pos.x += (playerSpeed * Time.smoothDeltaTime);
                }
            }

            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
            {
                playerAnimator.SetBool("strafing", false);
            }
            // Decelerate player when movement keys aren't held
            if ((playerSpeed > 3.5) && !(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S)))
            {
                playerSpeed -= accelerationFactor;
            }
            pos = CheckEdges(pos);
        }

        // TURBO
        if (turboEnabled) shootCooldownTime = shootCooldownTime / 2;
        else shootCooldownTime = 0.2f;

        // Constrain fire rate
        if (Time.time > nextFireTime)
        {
            if (shootCooldownToggle)
            {
                shootCooldownToggle = false;
            }
            if (Input.GetKey(KeyCode.Space) && !(playerHasMissile) || turboEnabled)
            {
                GameObject projectileA = Instantiate(Resources.Load("Prefabs/playerProjectileA") as GameObject);
                projectileA.transform.localPosition = transform.localPosition + bulletSpawnPoint;
                projectileA.transform.rotation = transform.rotation;
                nextFireTime = Time.time + shootCooldownTime;
                theGameController.sfxLaser.Play();
                if (Time.time > turboTime && turboEnabled)
                {
                    turboEnabled = false;
                    theGameController.turboReady = false;
                    GameObject.Find("tButton").SetActive(false);
                }
            }
        }
        // Constrain missile fire rate & check existence
        if (playerHasMissile && Time.time > nextMissileFireTime)
        {
            if (shootCooldownToggleMissile)
            {
                shootCooldownToggleMissile = false;
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameObject missile = Instantiate(Resources.Load("Prefabs/playerMissile") as GameObject);
                missile.transform.localPosition = transform.localPosition + missileSpawnPoint;
                missile.transform.rotation = transform.rotation;
                nextMissileFireTime = Time.time + shootMissleCooldownTime;
                theGameController.shootMissile();
            }
        }
        // Test block for missile
        if (Input.GetKeyDown(KeyCode.G))
        {
            theGameController.pickupMissile();
        }
        if (playerHealthCurrent <= 0f)
        {
            theGameController.takePlayerLife();
            playerHealthCurrent = playerHealthMax;
        }
        // Final update of position
        transform.position = pos;
    }
    private void FlipX()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
    }
    private Vector3 CheckEdges(Vector3 pos)
    {
        if (pos.x < -6.93f)
        {
            pos.x = 6.3f;
        }
        if (pos.y > 5.4f)
        {
            pos.y = -5.4f;
        }
        if (pos.x > 6.93f)
        {
            pos.x = -6.3f;
        }
        if (pos.y < -5.4f)
        {
            pos.y = 5.4f;
        }
        return pos;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (shield.activeSelf == false)
        {
            if (other.gameObject.tag == "Enemy")
            {
                playerHealthCurrent -= Random.Range(1.0f, 2.5f);
            }
            if (other.gameObject.tag == "waypoint" && !(other.gameObject.GetComponent<WaypointBehavior>().isImmune))
            {
                playerHealthCurrent -= Random.Range(0.5f, 1.5f);
            }
            if (other.gameObject.tag == "pickup")
            {
                theGameController.GetComponent<GameController>().sfxPickup.Play();
            }
            if (other.gameObject.tag == "enemyProjectile")
            {
                playerHealthCurrent -= Random.Range(0.2f, 0.5f);
            }
        }
    }
    public void playerExplode()
    {
        playerAnimator.SetBool("killed", true);
        Invoke("makeInactiveDelay", 0.75f);
    }
    public void playerHealthTopOff()
    {
        playerHealthCurrent = playerHealthMax;
    }
    public void activateShield()
    {
        shield.SetActive(true);
    }
    void makeInactiveDelay()
    {
        gameObject.SetActive(false);
    }
}