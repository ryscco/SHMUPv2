using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileBehavior : MonoBehaviour
{
    public float projectileSpeed = 8.5f;
    public GameObject explodePrefab;
    void Start()
    {
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
        }
        transform.position += transform.up * (projectileSpeed * Time.smoothDeltaTime);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Transform pos = other.gameObject.transform;
            if (gameObject.tag == "missile")
            {
                Instantiate(explodePrefab, pos);
            }
            Destroy(gameObject);
        }
    }
}