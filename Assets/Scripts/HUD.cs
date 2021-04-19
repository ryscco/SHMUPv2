using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD : MonoBehaviour
{
    Color colorFull = new Color(1f, 1f, 1f, 1f);
    Color colorFaded = new Color(1f, 1f, 1f, 0.3f);
    GameObject playLive1, playLive2, playLive3;
    public GameController gameController;
    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        playLive1 = GameObject.Find("playerLives1");
        playLive2 = GameObject.Find("playerLives2");
        playLive3 = GameObject.Find("playerLives3");

    }
    void Update()
    {
        if (gameController.playerLives == 3)
        {
            playLive1.GetComponent<SpriteRenderer>().material.color = colorFull;
            playLive2.GetComponent<SpriteRenderer>().material.color = colorFull;
            playLive3.GetComponent<SpriteRenderer>().material.color = colorFull;
        }
        else if (gameController.playerLives == 2)
        {
            playLive1.GetComponent<SpriteRenderer>().material.color = colorFull;
            playLive2.GetComponent<SpriteRenderer>().material.color = colorFull;
            playLive3.GetComponent<SpriteRenderer>().material.color = colorFaded;
        }
        else if (gameController.playerLives == 1)
        {
            playLive1.GetComponent<SpriteRenderer>().material.color = colorFull;
            playLive2.GetComponent<SpriteRenderer>().material.color = colorFaded;
            playLive3.GetComponent<SpriteRenderer>().material.color = colorFaded;
        }
        else
        {
            playLive1.GetComponent<SpriteRenderer>().material.color = colorFaded;
            playLive2.GetComponent<SpriteRenderer>().material.color = colorFaded;
            playLive3.GetComponent<SpriteRenderer>().material.color = colorFaded;
        }
    }
    void addPlayLive1()
    {
        playLive1.GetComponent<SpriteRenderer>().material.color = colorFull;
    }
    void addPlayLive2()
    {
        playLive2.GetComponent<SpriteRenderer>().material.color = colorFull;
    }
    void addPlayLive3()
    {
        playLive3.GetComponent<SpriteRenderer>().material.color = colorFull;
    }
    void removePlayLive1()
    {
        playLive1.GetComponent<SpriteRenderer>().material.color = colorFaded;
    }
    void removePlayLive2()
    {
        playLive2.GetComponent<SpriteRenderer>().material.color = colorFaded;
    }
    void removePlayLive3()
    {
        playLive3.GetComponent<SpriteRenderer>().material.color = colorFaded;
    }
}
