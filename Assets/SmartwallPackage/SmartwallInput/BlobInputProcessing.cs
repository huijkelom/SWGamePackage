﻿#pragma warning disable 0168
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum DetectionType { Raycast2D, Raycast3D, Both }
public enum InputType { Ball, MouseDown, MouseHold, MouseUp }

/// <summary>
/// This class takes the Blob data and translates it to interactions. The class uses a cylinder cast / overlap circle
/// the size of which is determined by the size of the ball as detected by the system. If AccountForBallSize is set to false
/// a raycast from the center of the ball will be used instead. All gameobjects with colliders in the casted path will
/// have the Hit() method called on all their scripts implementing the I_SmartwallInteractible interface.
/// Note that it is possible to have multiple BlobInputProcessing in a scene if you want to use 2D and 3D detection.
/// </summary>
public class BlobInputProcessing : MonoBehaviour
{
    public DetectionType DetectionType;
    /// <summary>
    /// rather then interacting on a single point should everthing in the balls radius be hit?
    /// This only works reliably when using orthographic cameras!
    /// </summary>
    public bool AccountForBallSize;
    public bool AverageAllInputsToOnePoint;
    /// <summary>
    /// Input will only trigger once withing the area of the input untill that area is clear of blobs.
    /// Use this to prevent multiple inputs with one ball throw. It is in screen width size percentages.
    /// 130% will cover the whole screen irregardless of the input's position.
    /// and .
    /// </summary>
    [Range(0,130)]
    public byte UninteractableAreaSize;

    private Dictionary<Vector2,bool> _InteractedPoints = new Dictionary<Vector2, bool>();

    /// <summary>
    /// Wether the game is allowed to process inputs.
    /// </summary>
    private static bool Active = true;

    /// <summary>
    /// Checks for blob and mouse input every physics update
    /// </summary>
    private void FixedUpdate()
    {
        if (Active)
        {
            List<Vector2> temp = _InteractedPoints.Keys.ToList();
            for (int i = 0; i < temp.Count; i++)
            {
                _InteractedPoints[temp[i]] = false;
            }

            if (AverageAllInputsToOnePoint)
            {
                if (BlobDetectionGateway.Blobs.Count > 0)
                {
                    Vector2 total = new Vector2();
                    float totalSize = 0;
                    foreach (Blob b in BlobDetectionGateway.Blobs)
                    {
                        total += b.Position;
                        totalSize += b.Width;
                    }

                    InteractInput(total / BlobDetectionGateway.Blobs.Count, totalSize / BlobDetectionGateway.Blobs.Count, InputType.Ball);
                }
            }
            else
            {
                foreach (Blob b in BlobDetectionGateway.Blobs)
                {
                    InteractInput(b.Position, b.Width, InputType.Ball);
                }
            }

            List<Vector2> toRemove = _InteractedPoints.Where(kvp => (kvp.Value == false)).Select(kvp => kvp.Key).ToList();
            foreach (Vector2 theKey in toRemove)
            {
                _InteractedPoints.Remove(theKey);
            }
        }
    }

    /// <summary>
    /// Handles mouse input.
    /// </summary>
    private void Update()
    {
        if (Active)
        {
            //Mouse clicks
            if (Input.GetMouseButtonDown(0))
            {
                InteractInput(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height), 0.05f, InputType.MouseDown);
            }
            //Mouse holds
            else if (Input.GetMouseButton(0))
            {
                InteractInput(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height), 0.05f, InputType.MouseHold);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                InteractInput(new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height), 0.05f, InputType.MouseUp);
            }
        }
    }

    /// <summary>
    /// Attempts to raycast or point/circle overlap at the given location to "collide" with an object.
    /// On succes it will try to fetch a class implementing I_SmartwallInteractable on the gameobject 
    /// it hit and call the Hit(Vector3 hitPosition) method.
    /// </summary>
    private void InteractInput(Vector2 screenPosition, float size, InputType inputType)
    {
        if (UninteractableAreaSize > 0)
        {
            foreach (KeyValuePair<Vector2,bool> kvp in _InteractedPoints)
            {
                if (Vector2.Distance(screenPosition,kvp.Key) < (float)UninteractableAreaSize / 100f)
                {
                    _InteractedPoints[kvp.Key] = true;
                    return;
                }
            }

            _InteractedPoints.Add(screenPosition, true);
        }

        switch (DetectionType)
        {
            case DetectionType.Raycast3D:
                Detect3D(screenPosition, size, inputType);
                break;
            case DetectionType.Raycast2D:
                Detect2D(screenPosition, size, inputType);
                break;
            case DetectionType.Both:
                Detect3D(screenPosition, size, inputType);
                Detect2D(screenPosition, size, inputType);
                break;
        }
    }

    public void Detect3D(Vector2 screenPosition, float size, InputType inputType)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x * Screen.width, screenPosition.y * Screen.height, 0f));
        Debug.DrawRay(ray.origin, ray.direction);

        if (AccountForBallSize)
        {
            RaycastHit[] hits = Physics.CapsuleCastAll(ray.origin, ray.origin + ray.direction, (size * Screen.width) * (Camera.main.orthographicSize / (float)Screen.height), ray.direction);
            foreach (RaycastHit hit in hits)
            {
                foreach (I_SmartwallInteractable script in hit.transform.gameObject.GetComponents<I_SmartwallInteractable>())
                {
                    script.Hit(hit.point, inputType);
                }
            }
        }
        else
        {
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
            {
                foreach (I_SmartwallInteractable script in hit.transform.gameObject.GetComponents<I_SmartwallInteractable>())
                {
                    script.Hit(hit.point, inputType);
                }
            }
        }
    }

    public void Detect2D(Vector2 screenPosition, float size, InputType inputType)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x * Screen.width, screenPosition.y * Screen.height, Camera.main.nearClipPlane));
        Debug.DrawRay(new Vector3(point.x, point.y, 0f), Vector3.forward, Color.red);
        List<Collider2D> hits2D = new List<Collider2D>();
        if (AccountForBallSize)
        {
            hits2D.AddRange(Physics2D.OverlapCircleAll(point, (Camera.main.orthographicSize * 2f) * size));
        }
        else
        {
            hits2D.AddRange(Physics2D.OverlapPointAll(point));
        }
        foreach (Collider2D hit2D in hits2D)
        {
            foreach (I_SmartwallInteractable script in hit2D.transform.gameObject.GetComponents<I_SmartwallInteractable>())
            {
                script.Hit(new Vector3(point.x, point.y, 0f), inputType);
            }
        }
    }

    public static void SetState(bool state)
    {
        Active = state;
    }
}