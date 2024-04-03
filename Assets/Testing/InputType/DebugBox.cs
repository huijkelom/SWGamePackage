using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugBox : MonoBehaviour
{
    public static DebugBox Instance { get; private set; }

    [SerializeField] private Text Label;

    private void Start()
    {
        Instance = this;
    }

    public void SetMessage(string message)
    {
        Label.text = message;
    }
}