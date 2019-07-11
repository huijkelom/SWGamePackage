using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestructOnImpact : MonoBehaviour
{
    public float Delay;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject, Delay);
    }
}
