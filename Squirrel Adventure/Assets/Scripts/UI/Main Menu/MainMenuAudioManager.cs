using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuAudioManager : MonoBehaviour
{
    public static MainMenuAudioManager instance;

    public AudioSource day, night;

    void Awake()
    {
        instance = this;
    }

    public void PlayNightMusic(bool playNightMusic)
    {
        if (playNightMusic)
        {
            day.Stop();

            night.Play();
        }
        else
        {
            night.Stop();

            day.Play();
        }
    }
}
