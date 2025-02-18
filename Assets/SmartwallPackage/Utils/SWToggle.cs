using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Toggle))]
[RequireComponent(typeof(BoxCollider2D))]
[ExecuteInEditMode]
public class SWToggle : MonoBehaviour, I_SmartwallInteractable
{
#if UNITY_EDITOR   
    private BoxCollider2D BoxCollider;
    private RectTransform RectTransform;

    private Vector2 DeltaSize;
    private Vector2 DeltaOffset;
       
    private void Awake()
    {
        RectTransform = transform.GetChild(0).GetComponent<RectTransform>();
        BoxCollider = transform.GetComponent<BoxCollider2D>();

        DeltaSize = RectTransform.rect.size;
        BoxCollider.size = DeltaSize + Vector2.one * 50;

        DeltaOffset = RectTransform.localPosition;
        BoxCollider.offset = DeltaOffset;
    }

    private void Update()
    {
        Vector2 rectSize = RectTransform.rect.size;
        if (rectSize != DeltaSize)
        {
            DeltaSize = rectSize;
            BoxCollider.size = rectSize + Vector2.one * 50;
        }

        Vector2 RectOffset = RectTransform.localPosition;
        if (RectOffset != DeltaOffset)
        {
            DeltaOffset = RectOffset;
            BoxCollider.offset = RectOffset;
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