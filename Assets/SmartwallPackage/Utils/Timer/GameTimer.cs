using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource), typeof(Image))]
public class GameTimer : MonoBehaviour
{
    [Tooltip("Time limit can be overwritten by the setting file if it contains a setting from Time.")]
    public float TimeLimit;

    [Space]
    public Text LabelOfTimer;
    public Image Gage;

    [Space][Tooltip("The amount of seconds when the timer needs to execute certain behaviours.")]
    public int AlmostFinishedTime = 5;
    public Color AlmostFinishedColor;

    [Space]
    public Image _FinishedFade;
    public UnityEvent TimerRanOut = new UnityEvent();

    private AudioSource _AlmostFinishedAudio;
    private AudioSource _FinishedAudio;

    private float _StartTime;
    private Color _ColourStart;
    private bool Paused = false;

    public void SetState(bool state)
    {
        gameObject.SetActive(state);
    }

    /// <summary>
    /// Start running the set timer.
    /// </summary>
    public void StartTimer()
    {
        AudioSource[] _audioSources = GetComponents<AudioSource>();
        _AlmostFinishedAudio = _audioSources[0];
        _FinishedAudio = _audioSources[1];

        _StartTime = Time.time;
        LabelOfTimer.color = _ColourStart;
        StartCoroutine("RunTimer");
    }

    /// <summary>
    /// Pause or unpause the timer.
    /// </summary>
    public void PauseTimer(bool pause)
    {
        Paused = pause;
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

        //load time setting from settings file, if there is not Time setting in the file the inspector value is used.
        string[] temp = GlobalGameSettings.GetSetting("Playtime").Split(' ');
        if (temp.Length > 1)
        {
            TimeLimit = int.Parse(temp[0]);
        }
        int minutes = (int)(TimeLimit / 60);
        int seconds = (int)(TimeLimit % 60);
        LabelOfTimer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");
    }

    IEnumerator RunTimer()
    {
        float t = TimeLimit;
        float redFade = 0;
        bool finale = false;

        while (t > 0)
        {
            if (!Paused)
            {
                int minutes = (int)(t / 60);
                int seconds = (int)(t % 60);
                Gage.fillAmount = t / TimeLimit;
                LabelOfTimer.text = minutes.ToString("D2") + ":" + seconds.ToString("D2");

                if (t < AlmostFinishedTime)
                {
                    redFade += Time.deltaTime / AlmostFinishedTime;
                    LabelOfTimer.color = Color.Lerp(_ColourStart, AlmostFinishedColor, redFade);

                    if (!finale)
                    {
                        _AlmostFinishedAudio.Play();
                        finale = true;
                    }
                }

                t -= Time.deltaTime;
            }

            yield return null;          
        }

        t = 0;
        Color c = _FinishedFade.color;
        c.a = 0.5f;
        _FinishedFade.color = c;
        _FinishedAudio.Play();
        yield return new WaitForSeconds(0.5f);
        //make sure the player isn't able to hit stuff anymore
        BlobInputProcessing.SetState(false);
        yield return new WaitForSeconds(1.5f);
        TimerRanOut.Invoke();
    }
}