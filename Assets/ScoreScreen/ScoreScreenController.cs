using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HighscoreContainer
{
    public int Highscore;
    public HighscoreContainer(int score)
    {
        Highscore = score;
    }
}

/// <summary>
/// This Class manages the score display scene. It is Important that the scene is still called "Scores".
/// You may wish to switch the replay button arrow, it is under C_UIRoot > P_Replay > BT_Replay
/// </summary>
public class ScoreScreenController : MonoBehaviour
{
    private static List<int> Scores = new List<int>();
    /// <summary>
    /// Current highscore, publicly availible incase you want to use it for something. DOES NOT PERSIST!
    /// </summary>
    public static int Highscore { get { return _Highscore; } }
    private static int _Highscore = 0;

    public int IndexOfSceneToMoveTo = 1;
    [HideInInspector]
    public float BarRiseAnimationTime = 0.7f;
    public GameObject P_Scoring;
    public GameObject ReplayButton;
    public GameObject ScoreBarBase;

    /// <summary>
    /// Moves to the scores scene to display the final scores and declare a winner and/or new highscore.
    /// Please set in the scene if you wish to use the continue or replay arrow on the button and set what
    /// scen should be loaded on pressing the button. There is an overload method to specify a scene on a 
    /// per invoke basis.
    /// </summary>
    /// <param name="scores"></param>
    public static void MoveToScores(List<int> scores)
    {
        Scores = scores;
        SceneManager.LoadScene("Scores");
    }
    /// <summary>
    /// Moves to the scores scene to display the final scores and declare a winner and/or new highscore.
    /// Please set in the scene if you wish to use the continue or replay arrow on the button. The sceneIndex
    /// parameter is for determining what scen to move to after the scores have been shown.
    /// </summary>
    public static void MoveToScores(List<int> scores, int sceneIndex)
    {
        Scores = scores;
        SceneManager.LoadScene("Scores");
    }

    void Start()
    {
        //load highscore from file
        if(GlobalGameSettings.GetSetting("Reset Highscore").Equals("No"))
        {
            LoadHighscore();
        }

        //check if we have all requirements linked
        if(ScoreBarBase == null) { Debug.LogError("ScoreScreenController | Start | Missing base object for score bars."); }
        if(P_Scoring == null) { Debug.LogError("ScoreScreenController | Start | Missing Link to perant panel."); }
        if(ReplayButton == null) { Debug.LogError("ScoreScreenController | Start | Missing Link to replay button."); }

        if (Scores == null)
        {
            Debug.LogError("ScoreScreenController | Start | No scores have been stored in the static Scores list!");
        }
        else
        {
            int numberOf0Scores = 0;
            int highestScore = 0;
            foreach(int score in Scores)
            {
                if(score == 0) { numberOf0Scores++; }
                if(score > highestScore) { highestScore = score; }
            }
            if(Scores.Count == 0)
            {
                Debug.LogError("ScoreScreenController | Start | No scores have been stored in the static Scores list!");
                return;
            }
            else if(Scores.Count - numberOf0Scores == 1)
            {
                SetupSinglePlayer();
            }
            else if (Scores.Count - numberOf0Scores > 1)
            {
                SetupMultiPlayer(highestScore);
            }
            if(highestScore > Highscore)
            {
                _Highscore = highestScore;
                SaveHighscore();
            }
        }
        Invoke("EnableReplay", BarRiseAnimationTime + 1f);
    }

    private void SetupSinglePlayer()
    {
        int highestScore;
        if(Scores[0] > Highscore)
        {
            highestScore = Scores[0];
        }
        else
        {
            highestScore = Highscore;
        }
        ScoreBar temp = Instantiate(ScoreBarBase, P_Scoring.transform).GetComponent<ScoreBar>();
        temp.SetNewBarColour(PlayerColourContainer.GetPlayerColour(1));
        temp.Begin(Scores[0], (float)Scores[0] / (float)highestScore, BarRiseAnimationTime, Scores[0] > Highscore, Scores[0] > Highscore, 0.1f);

        temp = Instantiate(ScoreBarBase, P_Scoring.transform).GetComponent<ScoreBar>();
        temp.SetNewBarColour(Color.black);
        temp.Begin(Highscore, (float)Highscore / (float)highestScore, BarRiseAnimationTime, false, false, 0.1f);
    }

    private void SetupMultiPlayer(int highestScore)
    {
        for(int i = 0; i < Scores.Count; i++)
        {
            if (Scores[i] > 0)
            {
                ScoreBar temp = Instantiate(ScoreBarBase, P_Scoring.transform).GetComponent<ScoreBar>();
                temp.SetNewBarColour(PlayerColourContainer.GetPlayerColour(i+1));
                temp.Begin(Scores[i], (float)Scores[i] / (float)highestScore, BarRiseAnimationTime, Scores[i] > Highscore && Scores[i] == highestScore, Scores[i] == highestScore, 0.1f);
            }
        }
    }

    private void SaveHighscore()
    {
        XML_to_Class.SaveClassToXML(new HighscoreContainer(Highscore), "StreamingAssets"+ Path.DirectorySeparatorChar + "HighScore");
    }

    private void LoadHighscore()
    {
        _Highscore = XML_to_Class.LoadClassFromXML<HighscoreContainer>("StreamingAssets"+ Path.DirectorySeparatorChar +"HighScore").Highscore;
    }

    private void EnableReplay()
    {
        ReplayButton.SetActive(true);
    }

    public void BT_Replay_Clicked()
    {
        SceneManager.LoadScene(IndexOfSceneToMoveTo);
    }
}
