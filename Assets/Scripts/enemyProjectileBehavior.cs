using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyProjectileBehavior : MonoBehaviour
{
    public float projectileSpeed = 8f;
    void Update()
    {
        transform.position -= transform.up * (projectileSpeed * Time.smoothDeltaTime);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}