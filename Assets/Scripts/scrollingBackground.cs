using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.015f;
    public Renderer bgRend;
    Color bgColor0 = Color.red;
    Color color1 = Color.yellow;
    public float colorCycleDuration = 60f;
    public bool colorCycleEnabled;
    void Start()
    {

    }
    void Update()
    {
        bgRend.material.mainTextureOffset += new Vector2(0, scrollSpeed * Time.deltaTime);
        if (colorCycleEnabled)
        {
            float t = Mathf.PingPong(Time.time, colorCycleDuration) / colorCycleDuration;
            bgRend.material.color = Color.Lerp(bgColor0, color1, t);
        }
    }
}