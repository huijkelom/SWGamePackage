using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField] private int Level;
    [SerializeField] private Transition Transition;

    public void SwitchLevel()
    {
        LevelManager.Instance.LoadLevel(Level, Transition);
    }
}
