using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shield : MonoBehaviour
{
    private float shieldMax = 10f;
    private float shieldCurrent;
    private void OnEnable() {
        shieldCurrent = shieldMax;
    }
    void Update()
    {
        if (shieldCurrent <= 0) {
            gameObject.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "waypoint") {
            shieldCurrent -= Random.Range(0.5f,1.0f);
        }
        if (other.gameObject.tag == "enemyProjectile") {
            shieldCurrent -= Random.Range(0.2f,0.5f);
        }
    }
}