using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    private GameController theGameController;
    private Animator wpAnim;
    private Transform prevPos;
    private Color color;
    public string wpName;
    public int wpHealth = 4;
    private float immuneTime;
    public bool isImmune;
    void Start()
    {
        isImmune = true;
        immuneTime = Time.time + 1f;
        theGameController = FindObjectOfType<GameController>();
        wpAnim = gameObject.GetComponent<Animator>();
        prevPos = transform;
        wpName = gameObject.name;
        color = new Color(1f, 1f, 1f, 1f);
    }
    void Update()
    {
        if (Time.time > immuneTime) isImmune = false;
        gameObject.GetComponent<SpriteRenderer>().material.color = color;
        if (wpHealth < 1)
        {
            color.a = 1f;
            gameObject.GetComponent<SpriteRenderer>().material.color = color;
            relocateSelf();
            // wpAnim.SetBool("killed", true);
            // Destroy(gameObject, 0.75f);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time > (immuneTime + 1f))
        {
            if (other.gameObject.name == "playerShip" || other.gameObject.tag == "missile" && GetComponent<SpriteRenderer>().enabled)
            {
                relocateSelf();
                // wpAnim.SetBool("killed", true);
                // Destroy(gameObject, 0.75f);
            }
            if (other.gameObject.tag == "projectile")
            {
                --wpHealth;
                color.a = GetComponent<SpriteRenderer>().material.color.a - 0.25f;
            }
        }
    }
    private void relocateSelf()
    {
        Vector3 pos = transform.localPosition;
        pos.x = (transform.localPosition.x + Random.Range(-0.75f, 0.75f));
        pos.y = (transform.localPosition.y + Random.Range(-0.75f, 0.75f));
        pos.z = 0;
        transform.localPosition = pos;
        wpAnim.SetBool("killed", true);
        wpAnim.SetBool("killed", false);
        immuneTime = Time.time + 1f;
        isImmune = true;
        wpHealth = 4;
        theGameController.wps = GameObject.FindGameObjectsWithTag("waypoint");
    }
    public void toggleVis() {
        GetComponent<SpriteRenderer>().enabled = !(GetComponent<SpriteRenderer>().enabled);
    }
}