using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTestObject : MonoBehaviour
{
    public AudioSource As;
    private void OnMouseDown()
    {
        As.Play();
    }
}
