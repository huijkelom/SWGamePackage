using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[AddComponentMenu("UI/Button", 30)]
public class SWButton : Button, I_SmartwallInteractable
{
    [Header("Moving down objects on button press")]
    public List<RectTransform> Graphics;
    public float PressDistance;

    private bool Cooldown = false;

    public void Hit(Vector3 hitpos)
    {
        if (!Cooldown)
        {
            StartCoroutine(_FakeClick());
        }
    }

#if UNITY_EDITOR
    private IEnumerator _FakeClick()
    {
        Cooldown = true;

        MoveGraphics(-PressDistance);
        while (currentSelectionState == SelectionState.Pressed)
        {
            yield return null;
        }

        MoveGraphics(PressDistance);
        Cooldown = false;
    }
#endif

#if !UNITY_EDITOR
    private IEnumerator _FakeClick()
    {
        Cooldown = true;

        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        MoveGraphics(-PressDistance);

        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        yield return new WaitForSeconds(0.1f);

        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        MoveGraphics(PressDistance);

        Cooldown = false;
    }
#endif

    private void MoveGraphics(float distance)
    {
        foreach (RectTransform graphic in Graphics)
        {
            graphic.transform.Translate(Vector2.up * distance);
        }
    }
}