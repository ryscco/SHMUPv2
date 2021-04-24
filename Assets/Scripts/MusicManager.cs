using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    Object[] bgPlaylist;
    int nextSong = 1;
    void Awake()
    {
        bgPlaylist = Resources.LoadAll("Music", typeof(AudioClip));
        gameObject.GetComponent<AudioSource>().clip = bgPlaylist[0] as AudioClip;
    }
    void Start()
    {
        gameObject.GetComponent<AudioSource>().Play();
    }
    void Update()
    {
        if (!gameObject.GetComponent<AudioSource>().isPlaying)
        {
            gameObject.GetComponent<AudioSource>().clip = bgPlaylist[nextSong] as AudioClip;
            gameObject.GetComponent<AudioSource>().Play();
            nextSong++;
        }
        if (nextSong == bgPlaylist.Length - 1) {
            nextSong = 0;
        }
    }
}
