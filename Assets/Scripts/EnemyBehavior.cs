using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    private float rotateSpeed = 135;
    private int hitsRemaining = 4;
    Color color;
    void Start()
    {
        color = GetComponent<SpriteRenderer>().material.color;
    }
    void Update()
    {
        transform.Rotate(transform.forward, rotateSpeed * Time.smoothDeltaTime);
        gameObject.GetComponent<SpriteRenderer>().material.color = color;
        if (hitsRemaining < 1)
        {
            Destroy(gameObject);
            GameObject.FindObjectOfType<GameController>().numberOfEnemiesKilled += 1;
        }
    }
    public void setRotateSpeed(float num)
    {
        rotateSpeed *= num;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            Destroy(gameObject);
            GameObject.FindObjectOfType<GameController>().numberOfEnemiesKilled += 1;
            GameObject.FindObjectOfType<GameController>().numberOfEnemiesTouched += 1;

        }
        if (other.gameObject.tag == "Projectile")
        {
            Destroy(other.gameObject);
            --hitsRemaining;
            color.a = GetComponent<SpriteRenderer>().material.color.a * 0.8f;
        }
    }
}