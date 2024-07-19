using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;
    public bool startMusic = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // This if-statement alters the pitch of the song ever so slightly, per loop.
        // startMusic is set to true, by PlayerStart.cs. 
        if (!audioSource.isPlaying && startMusic && PlayerLives.maxLives >= 2)
        {
            audioSource.pitch = Random.Range(0.9f, 1.0f);
            audioSource.Play();
        }
        else if(!audioSource.isPlaying && startMusic && PlayerLives.maxLives < 2)
        {
            audioSource.pitch = Random.Range(1.2f, 1.3f);
            audioSource.Play();
        }
    }
}
