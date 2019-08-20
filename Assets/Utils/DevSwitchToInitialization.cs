﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevSwitchToInitialization : MonoBehaviour
{
    private void Start()
    {
        if(MainThreadDispatcher.Instance == null)
        {
            SceneManager.LoadScene(0);
            Debug.LogWarning("DevSwitchToInitialization | Start | No gamemaster detected, did you start from an init scene?");
        }
    }
}