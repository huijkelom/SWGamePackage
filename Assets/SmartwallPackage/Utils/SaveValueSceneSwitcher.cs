using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveValueSceneSwitcher : MonoBehaviour
{
    [SerializeField] private string SaveValue;

    // Start is called before the first frame update
    void Start()
    {
        string value = GlobalGameSettings.GetSetting(SaveValue);
        StartCoroutine(switchScene(int.Parse(value)));
    }

    /// <summary>
    /// Switches to a new scene after 1 frame.
    /// </summary>
    /// <param name="index">the index of the scene in build order</param>
    /// <returns></returns>
    private IEnumerator switchScene(int index)
    {
        yield return null;
        SceneManager.LoadScene(index);
    }
}