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
    void Start()
    {
        theGameController = FindObjectOfType<GameController>();
        wpAnim = gameObject.GetComponent<Animator>();
        prevPos = transform;
        wpName = gameObject.name;
        color = new Color(1f,1f,1f,1f);
    }
    void Update()
    {
        gameObject.GetComponent<SpriteRenderer>().material.color = color;
        if (wpHealth < 1)
        {
            color.a = 1f;
            gameObject.GetComponent<SpriteRenderer>().material.color = color;
            wpAnim.SetBool("killed", true);
            Destroy(gameObject, 0.75f);
        }
    }
    private void OnDestroy()
    {
        theGameController.relocateWaypoint(prevPos, wpName);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "playerShip" || other.gameObject.tag == "missile")
        {
            wpAnim.SetBool("killed", true);
            Destroy(gameObject, 0.75f);
        }
        if (other.gameObject.tag == "projectile")
        {
            --wpHealth;
            color.a = GetComponent<SpriteRenderer>().material.color.a - 0.25f;
        }
    }
}