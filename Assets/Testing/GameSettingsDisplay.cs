using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsDisplay : MonoBehaviour
{
    public Text Diff;
    public Text Play;

    // Start is called before the first frame update
    void Start()
    {
        Diff.text = GlobalGameSettings.Difficulty.ToString();
        Play.text = GlobalGameSettings.Players.ToString();
    }
}
