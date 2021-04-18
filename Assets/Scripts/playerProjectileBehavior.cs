using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProjectileBehavior : MonoBehaviour
{
    public float projectileSpeed = 8.5f;
    void Start() {
        if (gameObject.CompareTag("missile")) {
            projectileSpeed = 4f;
        }
    }
    void Update()
    {
        if (gameObject.CompareTag("missile")) {
            projectileSpeed += 0.1f;
        }
        transform.position += transform.up * (projectileSpeed * Time.smoothDeltaTime);
    }
    private void OnBecameInvisible() {
        Destroy(gameObject);
    }
}