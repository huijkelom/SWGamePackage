using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSoundsManager : MonoBehaviour
{
    private static ScoreSoundsManager _Instance;
    public AudioSource WinSound;

    private void Awake()
    {
        _Instance = this;
    }

    public static void PlayWinSound()
    {
        _Instance?._PlayWinSound();
    }

    private void _PlayWinSound()
    {
        if (!WinSound.isPlaying)
        {
            WinSound.Play();
        }
    }
}
