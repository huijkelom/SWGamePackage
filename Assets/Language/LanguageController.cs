using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class LanguageController : MonoBehaviour
{
    public static LanguageController Instance;

    public delegate void LanguageEventHandler();
    public static event LanguageEventHandler LanguageChangedEvent;

    private static LanguageContainer _CurrentLanguage;
    private List<string> _AvailibleLanguages;
    public List<string> AvailibleLanguages { get { return _AvailibleLanguages; } }

    private void Awake()
    {
        //singleton
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
            Debug.LogWarning("A second language controller was loaded, maybe you should make a initialisation scene?");
        }

        //Inventorise what languages are availible
        _AvailibleLanguages = new List<string>();
        foreach (string path in Directory.GetFiles(Application.dataPath + "\\StreamingAssets\\Languages", "??.xml"))
        {
            _AvailibleLanguages.Add(Path.GetFileNameWithoutExtension(path));
        }
    }

    private void Start()
    {
        if (IsLanguageAvailible("en")) //default to english
        {
            LoadLanguage("en");
        }
        else if(_AvailibleLanguages.Count > 0) //if there is no english then default to the first language found
        {
            LoadLanguage(_AvailibleLanguages[0]);
        }
        else
        {
            Debug.LogWarning("LanguageController | Start | There are no language files availible.");
        }
    }

    public bool IsLanguageAvailible(string languageCode)
    {
        if (_AvailibleLanguages != null)
        {
            return _AvailibleLanguages.Contains(languageCode);
        }
        Debug.LogWarning("LanguageController | IsLanguageAvailible | Availible languages have not been loaded yet.");
        return false;
    }

    /// <summary>
    /// Loads the laguage of the given language code if availible.
    /// Language codes adhere to the ISO 639-1 standard for language codes.
    /// </summary>
    /// <param name="languageCode">The code of the language in compliance with ISO 639-1</param>
    public void LoadLanguage(string languageCode)
    {
        if (_AvailibleLanguages.Contains(languageCode))
        {
            _CurrentLanguage = XML_to_Class.LoadClassFromXML<LanguageContainer>("\\StreamingAssets\\Languages\\" + languageCode + ".xml");
        }
        else
        {
            Debug.LogWarning("LanguageController | LoadLanguage | Attempted to load a language that is not available.");
        }
    }

    /// <summary>
    /// Used by L_Text to fetch the text entry with the given ID for the currently loaded language.
    /// </summary>
    /// <param name="id">ID of the entry in the list of text entries</param>
    /// <returns>The text entry</returns>
    public static string GetText(int id)
    {
        if(_CurrentLanguage != null)
        {
            if(id >= _CurrentLanguage.Texts.Count)
            {
                /* Return a string that the L_Text can safely break on.
                *  I do this so i can throw the error from the L_Text instance, 
                *  making it easier to see where in the scene the issue is. */
                return string.Empty;
            }
            return _CurrentLanguage.Texts[id];
        }
        if (Instance == null)
        {
            Debug.LogError("LanguageController | GetText | Missing an instance of LanguageController");
        }
        return "Missing language file.";
    }

    /// <summary>
    /// Used by L_Image to fetch the path to the image with the given ID for the currently loaded language.
    /// </summary>
    /// <param name="id">ID of the entry in the list of images</param>
    /// <returns>Path of the image, relative to the data folder</returns>
    public static string GetImage(int id)
    {
        if(_CurrentLanguage != null)
        {
            if(id >= _CurrentLanguage.Images.Count)
            {
                return string.Empty;
            }
            return _CurrentLanguage.Images[id];
        }
        if (Instance == null)
        {
            Debug.LogError("LanguageController | GetText | Missing an instance of LanguageController");
        }
        return "Missing language file.";
    }

    /// <summary>
    /// Used by L_Image to fetch the path to the audio file with the given ID for the currently loaded language.
    /// </summary>
    /// <param name="id">ID of the entry in the list of audio files</param>
    /// <returns>Path of the file, relative to the data folder</returns>
    public static string GetAudio(int id)
    {
        if (_CurrentLanguage != null)
        {
            if (id >= _CurrentLanguage.AudioFiles.Count)
            {
                return string.Empty;
            }
            return _CurrentLanguage.AudioFiles[id];
        }
        if (Instance == null)
        {
            Debug.LogError("LanguageController | GetText | Missing an instance of LanguageController");
        }
        return "Missing language file.";
    }
}
