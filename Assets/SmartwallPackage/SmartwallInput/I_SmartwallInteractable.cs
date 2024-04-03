using UnityEngine;

/// <summary>
/// <para>Implement this interface in a class and it will have the Hit() method executed when its Gameobject is hit by the Smartwall.</para>
/// <para>The Hit() method has 2 variables:</para>
/// <para> - hitPosition, the Unity coordinates of where exactly the object was hit.</para>
/// <para> - inputType, what method was used to hit the object.</para>
/// <para>  There are 4 types of input:</para>
/// <para>        - Ball, hopefully obvious. </para>
/// <para>        - MouseDown, the first frame a mouse input got held down. </para>
/// <para>        - MouseHold, the mouse is being held down. </para>
/// <para>        - MouseUp, the first frame the mouse is no longer being held. </para>
/// <para>NOTE: The Gameobject needs a 2D or 3D collider in order to be hit!</para>
/// </summary>
public interface I_SmartwallInteractable
{
    void Hit(Vector3 hitPosition, InputType inputType);
}