using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Attach this class to an item with a Image class from UnityEngine.UI to connect it to the langauge management system.
/// Input the ID of the Image in the list of Images, if you don't link a Image to change via the inspector it will 
/// search for a Image class on its gameobject, if non is found it will post a waring in console.
/// </summary>
public class L_Image : MonoBehaviour
{
    public int IdOfImage;
    public Image ImageToChange;
    
    void Start()
    {
        //check if a Image has been linked
        if (ImageToChange == null)
        {
            ImageToChange = gameObject.GetComponent<Image>();
            if (ImageToChange == null) //Try to find an Image class
            {
                Debug.LogWarning("L_Image | Start | Image changer has no image to change and can't find one on its gameobject: " + gameObject.name);
                return;
            }
            else
            {
                Debug.LogWarning("L_Image | Start | Image changer has no image to change but it has found a Image class on its gameobject: " + gameObject.name);
            }
        }
        //register for language change
        LanguageController.LanguageChangedEvent += GetImagePathFromLanguageControllerAndChangeImage;
        GetImagePathFromLanguageControllerAndChangeImage();
    }

    public void GetImagePathFromLanguageControllerAndChangeImage()
    {
        string newImgPath = LanguageController.GetImage(IdOfImage);
        if (newImgPath.Equals(string.Empty))
        {
            Debug.LogWarning("L_Image | GetSpriteFromLanguageControllerAndPlaceItInImage | Atempted to load a Image entry that is not availible in the language file, is the file up to date?");
        }
        else if (Uri.IsWellFormedUriString(newImgPath,UriKind.Relative))
        {
            ImageToChange.sprite.texture.LoadImage(File.ReadAllBytes(Application.dataPath + Path.DirectorySeparatorChar + newImgPath));
        }
    }
}
