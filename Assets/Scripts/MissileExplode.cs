using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileExplode : MonoBehaviour
{
    private float blastRadiusSize = 10f;
    Vector3 blastIncrease = new Vector3(0.01f, 0.01f, 1f);
    void Update()
    {
        if (this.transform.localScale.x < blastRadiusSize)
        {
            this.transform.localScale += blastIncrease * 2;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other, 0.75f);
        // Destroy(gameObject, 5.0f);
    }
}