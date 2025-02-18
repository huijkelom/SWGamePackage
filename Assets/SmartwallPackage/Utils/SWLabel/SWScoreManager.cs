using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SWScoreManager : MonoBehaviour
{
    [SerializeField] private List<SWLabel> Labels;

    private List<int> Scores = new List<int>();

    private void Awake()
    {
        //Check how many players there are
        int playerCount = int.Parse(GlobalGameSettings.GetSetting("Players"));

        //Add labels based on what value you loaded
        Scores = new List<int>();
        for (int i = Labels.Count - 1; i >= playerCount; i--)
        {
            Labels[i].gameObject.SetActive(false);
        }
    }

    public void AddPoints(int player, int points)
    {
        Scores[player] += points;
        Labels[player].UpdateLabel(Scores[player].ToString());
    }

    public void FinishGame()
    {
        ScoreScreenController.MoveToScores(Scores);
    }
}