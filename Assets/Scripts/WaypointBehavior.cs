using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointBehavior : MonoBehaviour
{
    private GameController theGameController;
    private Animator wpAnim;
    private Transform prevPos;
    private Color color;
    public string waypointName;
    public int wpHealth = 4;
    private float immuneTime;
    public bool isImmune;
    public CameraSupport camSupp;
    void Start()
    {
        camSupp = Camera.main.GetComponent<CameraSupport>();
        isImmune = true;
        immuneTime = Time.time + 1f;
        theGameController = FindObjectOfType<GameController>();
        wpAnim = gameObject.GetComponent<Animator>();
        prevPos = transform;
        waypointName = gameObject.name;
        color = new Color(1f, 1f, 1f, 1f);
    }
    void Update()
    {
        if (Time.time > immuneTime) isImmune = false;
        gameObject.GetComponent<SpriteRenderer>().material.color = color;
        if (!(camSupp.isInside(gameObject.GetComponent<SpriteRenderer>().bounds)))
        {
            relocateSelf();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (Time.time > (immuneTime + 1f) && GetComponent<SpriteRenderer>().enabled)
        {
            if (other.gameObject.name == "playerShip" || other.gameObject.tag == "missile" && GetComponent<SpriteRenderer>().enabled)
            {
                waypointDestroy();
            }
            if (other.gameObject.tag == "projectile")
            {
                wpHealth--;
                color.a = GetComponent<SpriteRenderer>().material.color.a - 0.25f;
                if (wpHealth < 1)
                {
                    color.a = 1f;
                    gameObject.GetComponent<SpriteRenderer>().material.color = color;
                    waypointDestroy();
                }
            }
        }
    }
    private void relocateSelf()
    {
        wpAnim.SetBool("killed", false);
        Vector3 pos = transform.localPosition;
        pos.x = (transform.localPosition.x + Random.Range(-0.75f, 0.75f));
        pos.y = (transform.localPosition.y + Random.Range(-0.75f, 0.75f));
        pos.z = 0;
        transform.localPosition = pos;
        immuneTime = Time.time + 1f;
        isImmune = true;
        wpHealth = 4;
        theGameController.wps = GameObject.FindGameObjectsWithTag("waypoint");
    }
    public void toggleVis()
    {
        GetComponent<SpriteRenderer>().enabled = !(GetComponent<SpriteRenderer>().enabled);
    }
    void waypointDestroy()
    {
        wpAnim.SetBool("killed", true);
        theGameController.sfxExplosion.Play();
        Invoke("relocateSelf", 0.75f);
    }
}