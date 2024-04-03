using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[AddComponentMenu("UI/Button", 30)]
public class SWButton : Button, I_SmartwallInteractable
{
    private bool Cooldown = false;

#if UNITY_EDITOR
    public void Hit(Vector3 hitPosition, InputType inputType) { }
#endif

#if !UNITY_EDITOR
    public void Hit(Vector3 hitPosition, InputType inputType)
    {
        if (inputType == InputType.Ball && !Cooldown)
        {
            StartCoroutine(_FakeClick());
        }
    }

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