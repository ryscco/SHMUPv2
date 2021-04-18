using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBehavior : MonoBehaviour
{
    public Animator playerAnimator;
    public Rigidbody2D playerRB2D;
    public float playerSpeed = 3.5f;
    public float maxPlayerSpeed = 5f;
    public float accelerationFactor = 0.02f;
    public bool controlScheme = true;
    public float shootCooldownTime = 0.2f;
    private bool shootCooldownToggle = false;
    private float nextFireTime = 0;
    // private GameController theGameController = null;
    private float spriteSizeX = 0;
    private float spriteSizeY = 0;
    void Start()
    {
        playerAnimator = GetComponent<Animator>();
        playerRB2D = GetComponent<Rigidbody2D>();
        // theGameController = FindObjectOfType<GameController>();
        spriteSizeX = gameObject.GetComponent<SpriteRenderer>().bounds.size.x;
        spriteSizeY = gameObject.GetComponent<SpriteRenderer>().bounds.size.y;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            controlScheme = !controlScheme;
            playerSpeed = 3.5f;
        }

        Vector3 pos = transform.position;

        if (controlScheme) // Default scheme is player locked to mouse
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
            // Check player position for edge transitioning
            if (pos.x < -6.28)
            {
                pos.x = 6.28f;
            }
            if (pos.y > 5.4)
            {
                pos.y = -5.4f;
            }
            if (pos.x > 6.28)
            {
                pos.x = -6.28f;
            }
            if (pos.y < -5.4)
            {
                pos.y = 5.4f;
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
        }
        transform.position = pos;
    }
    private void FlipX()
    {
        Vector3 flip = transform.localScale;
        flip.x *= -1;
        transform.localScale = flip;
    }
}