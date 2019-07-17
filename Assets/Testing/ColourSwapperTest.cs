using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourSwapperTest : MonoBehaviour, I_SmartwallInteractable
{
    public AudioSource As;
    public void Hit(Vector3 hitPosition)
    {
        As.Play();
    }
}
