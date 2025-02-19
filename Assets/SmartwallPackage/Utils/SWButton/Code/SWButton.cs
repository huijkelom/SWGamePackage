using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(BoxCollider2D))]
[ExecuteInEditMode]
public class SWButton : MonoBehaviour, I_SmartwallInteractable
{
#if UNITY_EDITOR
    private Vector2 DeltaSize;
    private Vector2 DeltaPadding;
    private BoxCollider2D BoxCollider;

    private RectTransform RectTransform;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        BoxCollider = transform.GetComponent<BoxCollider2D>();

        DeltaSize = RectTransform.rect.size;
        DeltaPadding = new Vector2(50, 50);
        BoxCollider.size = DeltaSize + DeltaPadding;
    }

    private void Update()
    {
        Vector2 rectSize = RectTransform.rect.size;
        if (rectSize != DeltaSize)
        {
            DeltaSize = rectSize;
            BoxCollider.size = rectSize + DeltaPadding;         
        }

        if (BoxCollider.size != DeltaSize + DeltaPadding)
        {
            DeltaPadding = BoxCollider.size - rectSize;
        }
    }

    public void Hit(Vector3 hitPosition, InputType inputType) { }
#endif

#if !UNITY_EDITOR
    private bool Cooldown = false;

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