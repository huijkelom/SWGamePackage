using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTester : MonoBehaviour
{
    public InputField a;
    public InputField b;
    public InputField c;
    public InputField d;

    public void GoToScores()
    {
        List<int> scores = new List<int>();
        if (!a.text.Equals(string.Empty))
        {
            scores.Add(int.Parse(a.text));
        }
        if (!b.text.Equals(string.Empty))
        {
            scores.Add(int.Parse(b.text));
        }
        if (!c.text.Equals(string.Empty))
        {
            scores.Add(int.Parse(c.text));
        }
        if (!d.text.Equals(string.Empty))
        {
            scores.Add(int.Parse(d.text));
        }
        Debug.Log(a.text + " " + b.text + " " + c.text + " " + d.text);
        ScoreScreenController.MoveToScores(scores);
    }

}
