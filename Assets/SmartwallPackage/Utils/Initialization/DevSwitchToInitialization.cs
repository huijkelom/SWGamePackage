#pragma warning disable 0168
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevSwitchToInitialization : MonoBehaviour
{
    private void Awake()
    {
        if (AudioManager.Instance == null)
        {
            SceneManager.LoadScene(0);
            Debug.LogWarning("DevSwitchToInitialization | Start | No gamemaster detected, did you start from an init scene?");
        }
    }
}
