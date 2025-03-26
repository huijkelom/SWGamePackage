using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    [SerializeField] private int StartScene = 1;
    [Space]
    [SerializeField] private float TransitionTime = 1f;
    [SerializeField] private Animator Animator;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadInstantly(1);
    }

    /// <summary>
    /// Loads the level that corresponds to the index in the build queue
    /// </summary>
    public void LoadLevel(int index, Transition transition)
    {
        if (transition == Transition.None)
        {
            LoadInstantly(index);
            return;
        }

        StartCoroutine(_LoadLevel(index, transition));
    }

    public void LoadLevel(string name, Transition transition)
    {
        if (transition == Transition.None)
        {
            SceneManager.LoadScene(name);
            return;
        }

        StartCoroutine(_LoadLevel(name, transition));
    }

    /// <summary>
    /// Loads the level after the currently loaded sceen in the build queue
    /// </summary>
    public void LoadNextLevel(Transition transition)
    {
        int index = SceneManager.GetActiveScene().buildIndex + 1;
        if (transition == Transition.None)
        {
            LoadInstantly(index);
            return;
        }

        StartCoroutine(_LoadLevel(index, transition));
    }

    private void LoadInstantly(int index)
    {
        SceneManager.LoadScene(index);
    }

    private IEnumerator _LoadLevel(int index, Transition transition)
    {
        Animator.Play(transition.ToString() + "_Out");
        AudioManager.Instance.Play("TransitionOut");

        yield return new WaitForSeconds(1f);
        AsyncOperation loading = SceneManager.LoadSceneAsync(index);
        while (!loading.isDone)
        {
            yield return null;
        }

        Animator.Play(transition.ToString() + "_In");
        AudioManager.Instance.Play("TransitionIn");
    }

    private IEnumerator _LoadLevel(string name, Transition transition)
    {
        Animator.Play(transition.ToString() + "_Out");
        AudioManager.Instance.Play("TransitionOut");

        yield return new WaitForSeconds(1f);
        AsyncOperation loading = SceneManager.LoadSceneAsync(name);
        while (!loading.isDone)
        {
            yield return null;
        }

        Animator.Play(transition.ToString() + "_In");
        AudioManager.Instance.Play("TransitionIn");
    }
}

public enum Transition
{
    None, 
    Crossfade,
    Circle,
    ZigZag,
    Heart,
    PixelCircle
}