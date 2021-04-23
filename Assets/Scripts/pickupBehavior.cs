using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pickupBehavior : MonoBehaviour
{
    public float movementSpeed;
    public GameController theGameController;
    void Start()
    {
        theGameController = FindObjectOfType<GameController>();
        movementSpeed = 1.5f + Random.Range(0.0f, 1.0f);
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, -5.4f, 0f), movementSpeed * Time.smoothDeltaTime);
        if (transform.position.y < -5.4f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            float pickupDetermine = Random.value;
            if (pickupDetermine <= 0.65f)
            {
                theGameController.pickupMissile();
            }
            else if (pickupDetermine > 0.65f && pickupDetermine <= 0.9f)
            {
                theGameController.pickupShield();
            }
            else
            {
                if (theGameController.playerLives < 3)
                {
                    theGameController.givePlayerLife();
                }
                else
                {
                    theGameController.instantiatePickup();
                }
            }
            Destroy(gameObject);
        }
    }
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}