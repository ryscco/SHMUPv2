using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    private Animator wpAnim;
    void Start()
    {
        wpAnim = gameObject.GetComponent<Animator>();
    }
    void Update()
    {
        
    }
    private void OnDestroy() {
        
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.name == "playerShip" || other.gameObject.tag == "missile") {
            wpAnim.SetBool("killed", true);
            Destroy(gameObject, 0.75f);
        }
    }
}