using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class HitAreaSizeScaler : MonoBehaviour
{
    private Vector2 DeltaSize;
    private Vector2 DeltaPadding;
    [SerializeField] private BoxCollider2D BoxCollider;

    private RectTransform RectTransform;

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();

        DeltaSize = RectTransform.rect.size;
        DeltaPadding = new Vector2(-100, -100);
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
            //DeltaPadding = BoxCollider.size - rectSize;
        }
    }
}