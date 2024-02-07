using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[AddComponentMenu("UI/Button", 30)]
public class SWButton : Button, I_SmartwallInteractable
{
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
        while (currentSelectionState == SelectionState.Pressed)
        {
            yield return null;
        }

        Cooldown = false;
    }
#endif

#if !UNITY_EDITOR
    private IEnumerator _FakeClick()
    {
        Cooldown = true;
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        Cooldown = false;
    }
#endif
}