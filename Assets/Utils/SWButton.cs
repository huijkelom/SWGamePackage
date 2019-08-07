using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Put this class on a GUI button to have it Click event fire on smartwall input.
/// </summary>
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Collider2D))]
public class SWButton : MonoBehaviour, I_SmartwallInteractable
{
    Button _ButtonImOn;
    private void Awake()
    {
        _ButtonImOn = gameObject.GetComponent<Button>();
    }

    // Start is called before the first frame update
    public void Hit(Vector3 location)
    {
        _ButtonImOn.onClick.Invoke();
    }
}
