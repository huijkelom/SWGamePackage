#pragma warning disable 0168
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum InputTypes { Raycast2D, Raycast3D}

/// <summary>
/// This class take the Blob data and translates it to interaction. The class will use a cylinder cast / overlap circle
/// the size of wich is determined by the size of the ball as detected by the system. If AccountForBallSize is false
/// a raycast from the center of the ball will be used instead. All gameobjects with colliders in the casted path will
/// have the Hit() methos called on all thier scripts implemneting the I_SmartwallInteractible interface.
/// Note that it is possible to have multiple BlobInputProcessing in a scene if you want to use 2D and 3D detection.
/// </summary>
public class BlobInputProcessing : MonoBehaviour
{
    public InputTypes InputType;
    /// <summary>
    /// rather then interacting on a single point should everthing in the balls radius be hit?
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

    //Every frame we check for blob and mouse input
    private void FixedUpdate()
    {
        List<Vector2> temp = _InteractedPoints.Keys.ToList();
        for(int i = 0; i<temp.Count;i++)
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
                InteractInput(total / BlobDetectionGateway.Blobs.Count, totalSize / BlobDetectionGateway.Blobs.Count);
            }
        }
        else
        {
            foreach (Blob b in BlobDetectionGateway.Blobs)
            {
                InteractInput(b.Position, b.Width);
            }
        }
        if (Input.GetMouseButton(0))
        {
            InteractInput(Input.mousePosition, 0.1f);
        }

        List<Vector2> toRemove = _InteractedPoints.Where(kvp => (kvp.Value == false)).Select(kvp => kvp.Key).ToList();
        foreach (Vector2 theKey in toRemove)
        {
            _InteractedPoints.Remove(theKey);
        }
    }

    /// <summary>
    /// Attempts to raycast or point/circle overlap at the given location to "collide" with an object.
    /// On succes it will try to fetch a class implementing I_SmartwallInteractable on the gameobject 
    /// it hit and call the Hit(Vector3 hitPosition) method.
    /// </summary>
    private void InteractInput(Vector2 screenPosition, float size)
    {
        if(UninteractableAreaSize > 0)
        {
            foreach(KeyValuePair<Vector2,bool> kvp in _InteractedPoints)
            {
                if(Vector2.Distance(screenPosition,kvp.Key) < (float)UninteractableAreaSize / 100f)
                {
                    _InteractedPoints[kvp.Key] = true;
                    return;
                }
            }
            _InteractedPoints.Add(screenPosition, true);
        }
        switch (InputType)
        {
            case InputTypes.Raycast3D:
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x * Screen.width, screenPosition.y * Screen.height, 0f));
                Debug.DrawRay(ray.origin, ray.direction);
                if (AccountForBallSize)
                {
                    RaycastHit[] hits = Physics.CapsuleCastAll(ray.origin, ray.origin + ray.direction, size * Screen.width, ray.direction);
                    foreach(RaycastHit hit in hits)
                    {
                        foreach (I_SmartwallInteractable script in hit.transform.gameObject.GetComponents<I_SmartwallInteractable>())
                        {
                            script.Hit(hit.point);
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
                            script.Hit(hit.point);
                        }
                    }
                }
                break;
            case InputTypes.Raycast2D:
                Vector2 point = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x * Screen.width, screenPosition.y * Screen.height, Camera.main.nearClipPlane));
                Debug.DrawRay(new Vector3(point.x, point.y, 0f), Vector3.forward, Color.red);
                List<Collider2D> hits2D = new List<Collider2D>();
                if (AccountForBallSize)
                {
                    hits2D.AddRange(Physics2D.OverlapCircleAll(point, size * Screen.width));
                }
                else
                {
                    hits2D.AddRange(Physics2D.OverlapPointAll(point));
                }
                foreach (Collider2D hit2D in hits2D)
                {
                    foreach (I_SmartwallInteractable script in hit2D.transform.gameObject.GetComponents<I_SmartwallInteractable>())
                    {
                        script.Hit(new Vector3(point.x, point.y, 0f));
                    }
                }
                break;
            //case InputTypes.Physical:
            //    if (Ball == null)
            //    {
            //        Debug.LogError("BlobInputProcessing | InteractInput | Missing throwable object. Please link the default 'ball' provided in the package.");
            //        return;
            //    }
            //    Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 0f));
            //    GameObject ball = Instantiate(Ball, ray2.origin, Quaternion.identity);
            //    ball.transform.localScale = new Vector3(size * Screen.width, size * Screen.width, size * Screen.width);
            //    try
            //    {
            //        ball.GetComponent<Rigidbody>().AddForce(ray2.direction * Force, ForceMode.Impulse);
            //    }
            //    catch (NullReferenceException nREx)
            //    {
            //        Debug.LogError("BlobInputProcessing | InteractInput | You have changed the throw object but the new object is missing a Rigidbody!");
            //    }
            //    break;
        }

    }
}