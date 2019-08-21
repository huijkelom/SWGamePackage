using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    public Text LabelOfTimer;
    public Image Gage;
    /// <summary>
    /// Time limit can be overwritten by the setting file if it contains a setting from Time.
    /// </summary>
    public float TimeLimit;
    public Color ColorWhenOutOfTime;
    public float PercentageOutOfTime = 15;
    private float _StartTime;
    private Color _ColourStart;
    private bool Started = false;
    public UnityEvent TimerRanOut = new UnityEvent();

    public void StartTimer()
    {
        Started = true;
        _StartTime = Time.time;
        LabelOfTimer.color = _ColourStart;
    }

    void Awake()
    {
        //Check if a Text class has been linked
        if (LabelOfTimer == null)
        {
            LabelOfTimer = gameObject.GetComponent<Text>(); //Try to find a Text class
            if (LabelOfTimer == null)
            {
                Debug.LogWarning("L_Text | Start | Text changer has no label to change and can't find one on its gameobject: " + gameObject.name);
                return;
            }
            else
            {
                Debug.LogWarning("L_Text | Start | Text changer has no label to change but it has found a Text class on its gameobject: " + gameObject.name);
            }
        }
        _ColourStart = LabelOfTimer.color;
    }

    private void Start()
    {
        //load time setting from settings file, if there is not Time setting in the file the inspector value is used.
        string[] temp = GlobalGameSettings.GetSetting("Time").Split(':');
        if (temp.Length > 0)
        {
            TimeLimit = int.Parse(temp[0]) * 60 + int.Parse(temp[1]);
        }
    }

    void Update()
    {
        float t = TimeLimit;
        if (Started)
        {
            t -= (Time.time - _StartTime);
        }
        if (t <= 0)
        {
            TimerRanOut.Invoke();
            t = 0;
        }
        int minutes = (int)(t / 60);
        int seconds = (int)(t % 60);
        Gage.fillAmount = t / TimeLimit;

        LabelOfTimer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        if (t < (TimeLimit / PercentageOutOfTime))
        {
            float factor = t / PercentageOutOfTime;
            LabelOfTimer.color = Color.Lerp(ColorWhenOutOfTime, _ColourStart, factor);
        }
    }
}
