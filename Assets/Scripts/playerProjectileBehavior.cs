using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileBehavior : MonoBehaviour
{
    public float projectileSpeed = 8.5f;
    public GameObject explodePrefab;
    GameObject[] enemies;
    int missileTarget;
    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        missileTarget = Random.Range(0, enemies.Length);
        if (gameObject.CompareTag("missile"))
        {
            projectileSpeed = 4f;
        }
    }
    void Update()
    {
        if (gameObject.CompareTag("missile"))
        {
            projectileSpeed += 0.1f;
            transform.position = Vector3.MoveTowards(transform.position, enemies[missileTarget].transform.position, Time.deltaTime * projectileSpeed);
            if (transform.position == enemies[missileTarget].transform.position)
            {
                Destroy(gameObject);
            }
        }
        else transform.position += transform.up * (projectileSpeed * Time.smoothDeltaTime);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "waypoint")
        {
            Destroy(gameObject);
        }
    }
}