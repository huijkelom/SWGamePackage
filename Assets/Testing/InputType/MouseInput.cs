using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseInput : MonoBehaviour, I_SmartwallInteractable
{
    private int HoldTimer = 0;

    public void Hit(Vector3 hitPosition, InputType inputType)
    {
        switch (inputType)
        {
            case InputType.MouseDown:
                DebugBox.Instance.SetMessage("You just clicked the SmartWall with a mouse!");
                break;
            case InputType.MouseHold:
                HoldTimer++;
                DebugBox.Instance.SetMessage("You've been holding down the mouse button for " + HoldTimer + " frames now!");               
                break;
            case InputType.MouseUp:
                DebugBox.Instance.SetMessage("You let go of the mouse!");
                HoldTimer = 0;
                break;
        }
    }
}
