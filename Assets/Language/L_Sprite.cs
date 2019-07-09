using System;
using System.IO;
using UnityEngine;

public class L_Sprite : MonoBehaviour
{
    public int IdOfSprite;
    public SpriteRenderer SpriteToChange;
    
    void Start()
    {
        //check if a SpriteRenderer has been linked
        if (SpriteToChange == null)
        {
            SpriteToChange = gameObject.GetComponent<SpriteRenderer>();
            if(SpriteToChange == null) //Try to find an SpriteRenderer class
            {
                Debug.LogWarning("L_Sprite | Start | Sprite changer has no sprite to change and can't find one on its gameobject: " + gameObject.name);
                return;
            }
            else
            {
                Debug.LogWarning("L_Sprite | Start | Sprite changer has no sprite to change but it has found a SpriteRenderer class on its gameobject: " + gameObject.name);
            }
        }
        //register for language change
        LanguageController.LanguageChangedEvent += GetImagePathFromLanguageControllerAndChangeSprite;
        GetImagePathFromLanguageControllerAndChangeSprite();
    }

    void GetImagePathFromLanguageControllerAndChangeSprite()
    {
        string newImgPath = LanguageController.GetImage(IdOfSprite);
        if (newImgPath.Equals(string.Empty))
        {
            Debug.LogWarning("L_Image | GetSpriteFromLanguageControllerAndPlaceItInImage | Atempted to load a Image entry that is not availible in the language file, is the file up to date?");
        }
        else if (Uri.IsWellFormedUriString(newImgPath, UriKind.Relative))
        {
            SpriteToChange.sprite.texture.LoadImage(File.ReadAllBytes(Application.dataPath + Path.DirectorySeparatorChar + newImgPath));
        }
    }
}
