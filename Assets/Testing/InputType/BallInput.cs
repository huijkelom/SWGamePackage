using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInput : MonoBehaviour, I_SmartwallInteractable
{
    public void Hit(Vector3 hitPosition, InputType inputType)
    {
        if (inputType == InputType.Ball)
        {
            DebugBox.Instance.SetMessage("You just hit the SmartWall with a ball!");
        }
    }
}
