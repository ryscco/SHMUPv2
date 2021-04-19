using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplode : MonoBehaviour
{
    private float blastRadiusSize = 5f;
    Vector3 blastIncrease = new Vector3(0.1f, 0.1f, 0f);
    void Update()
    {
        if (transform.localScale.x < blastRadiusSize)
        {
            transform.localScale += blastIncrease * 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other, 0.75f);
        // Destroy(gameObject, 0.75f);
    }
}