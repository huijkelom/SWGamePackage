using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            int index = LanguageController.Instance.AvailibleLanguages.IndexOf(LanguageController.LanguageLoaded);
            index++;
            if(index >= LanguageController.Instance.AvailibleLanguages.Count)
            {
                index = 0;
            }
            LanguageController.Instance.LoadLanguage(LanguageController.Instance.AvailibleLanguages[index]);
        }
    }
}
