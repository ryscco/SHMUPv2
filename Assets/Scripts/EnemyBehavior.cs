using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private int hitsRemaining = 4;
    public Animator enemyAnimator = null;
    public GameController theGameController = null;
    Color color;
    public char patrolType; // A (waypoints in order) or B (random waypoints)
    public float enemySpeed = 2.5f;
    GameObject[] wps;
    public int wpIndex; // This enemy's next waypoint destination
    float wpRadius;
    int fire;
    void Start()
    {
        wpIndex = Random.Range(0, 5);
        wps = GameObject.FindGameObjectsWithTag("waypoint");
        wpRadius = wps[0].GetComponent<SpriteRenderer>().size.x / 2;
        // Set patrol type on creation randomly between A and B
        if (Random.value < 0.5f) patrolType = 'A';
        else patrolType = 'B';
        enemyAnimator = GetComponent<Animator>();
        color = GetComponent<SpriteRenderer>().material.color;
        theGameController = FindObjectOfType<GameController>();
    }
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = color;
        if (hitsRemaining < 1)
        {
            color.a = 1f;
            gameObject.GetComponent<SpriteRenderer>().material.color = color;
            enemyAnimator.SetBool("killed", true);
            Destroy(gameObject, 0.75f);
        }

        if (Vector3.Distance(wps[wpIndex].transform.position, transform.position) < wpRadius)
        {
            if (patrolType == 'A')
            {
                wpIndex++;
                if (wpIndex >= wps.Length)
                {
                    wpIndex = 0;
                }
            }
            else
            {
                int rand = Random.Range(0, 5);
                if (rand != wpIndex)
                {
                    wpIndex = rand;
                }
                else
                {
                    wpIndex++;
                }
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, wps[wpIndex].transform.position, Time.deltaTime * enemySpeed);

        fire = Random.Range(0, 1000);
        if (fire == 0)
        {
            GameObject projectile = Instantiate(Resources.Load("Prefabs/enemyProjectile") as GameObject);
            projectile.transform.localPosition = transform.localPosition;
            projectile.transform.rotation = transform.rotation;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        string n = other.gameObject.tag;
        if (n == "Player" || n == "missile" || n == "shield")
        {
            enemySpeed = 0;
            enemyAnimator.SetBool("killed", true);
            theGameController.GetComponent<GameController>().sfxExplosion.Play();
            Destroy(gameObject, 0.75f);
            if (n != "missile") theGameController.touchEnemy();
        }
        else if (n == "projectile")
        {
            --hitsRemaining;
            color.a = GetComponent<SpriteRenderer>().material.color.a * 0.8f;
        }
    }
    private void OnDestroy()
    {
        theGameController.killEnemy();
    }
}