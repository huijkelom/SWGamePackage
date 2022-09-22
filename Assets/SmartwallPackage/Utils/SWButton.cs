using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Collider2D))]
[AddComponentMenu("UI/Button", 30)]
public class SWButton : Button, I_SmartwallInteractable
{
    public List<RectTransform> Graphics;
    public int Pixels;

    private bool Cooldown = false;

#if !UNITY_EDITOR
    public void Hit(Vector3 hitpos) { }
#endif

#if UNITY_EDITOR
    public void Hit(Vector3 location)
    {
        if (!Cooldown)
        {
            StartCoroutine(_FakeClick());
        }
    }

    private IEnumerator _FakeClick()
    {
        Cooldown = true;

        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerDownHandler);
        MoveGraphics(-Pixels);

        yield return new WaitForSeconds(0.1f);
        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerClickHandler);
        yield return new WaitForSeconds(0.1f);

        ExecuteEvents.Execute(gameObject, new PointerEventData(EventSystem.current), ExecuteEvents.pointerUpHandler);
        MoveGraphics(Pixels);

        Cooldown = false;
    }

    private void MoveGraphics(int direction)
    {
        foreach (RectTransform graphic in Graphics)
        {
            graphic.rect.Set(graphic.rect.x, graphic.rect.y + direction, graphic.rect.width, graphic.rect.height);
        }
    }
#endif
}
    