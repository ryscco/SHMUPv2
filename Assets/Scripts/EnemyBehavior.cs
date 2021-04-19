using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private int hitsRemaining = 4;
    public Animator enemyAnimator;
    public GameController gameController;
    Color color;
    void Start()
    {
        enemyAnimator = GetComponent<Animator>();
        color = GetComponent<SpriteRenderer>().material.color;
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
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "playerShip")
        {
            enemyAnimator.SetBool("killed", true);
            Destroy(gameObject, 0.75f);
            gameController.numberOfEnemiesTouched += 1;
        }
        if (other.gameObject.tag == "projectile")
        {
            --hitsRemaining;
            color.a = GetComponent<SpriteRenderer>().material.color.a * 0.8f;
        }
        if (other.gameObject.tag == "missile")
        {
            enemyAnimator.SetBool("killed", true);
            Destroy(gameObject, 0.75f);
        }
    }
    private void OnDestroy()
    {
        // gameController.numberOfEnemiesKilled += 1;
    }
}