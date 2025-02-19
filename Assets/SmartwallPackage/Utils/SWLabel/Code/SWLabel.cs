using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SWLabel : MonoBehaviour
{
    [SerializeField] private Text Label;
    [SerializeField] private Image Background;

    public void UpdateLabel(string value)
    {
        Label.text = value;
    }

    public void UpdateBackground(Color value)
    {
        Background.color = value;
    }
}