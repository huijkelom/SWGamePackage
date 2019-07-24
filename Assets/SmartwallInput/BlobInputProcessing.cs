#pragma warning disable 0168
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputTypes { Raycast2D, Raycast3D, Physical}
public enum CameraTypes { Perspective, Orthographic}

/// <summary>
/// This class take the Blob data and translates it to interaction. 2D and 3D input modes will fetch 
/// the object at the input point of the ball and if it is in possesion of a collider and an I_SmartwallInteractable
/// script, it will call the Hit() function on said script. Detection is done on layer 8.
/// In Physical input mode an invisible ball is thrown at the impact point, only work with 3D physics. 
/// If you want the ball to only hit certain things consider useing https://docs.unity3d.com/Manual/LayerBasedCollision.html
/// Note that it is possible to have multiple BlobInputProcessing in a scene if you want to have a mix of detection types.
/// </summary>
public class BlobInputProcessing : MonoBehaviour
{
    public InputTypes InputType;
    public CameraTypes CameraType;
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
    [Header("Setting for physical interaction type.")]
    public byte Force;
    [Header("Do not change!")]
    public GameObject Ball;

    private Dictionary<Vector2,bool> _InteractedPoints = new Dictionary<Vector2, bool>();

    //Every frame we check for blob and mouse input
    private void FixedUpdate()
    {
        foreach(KeyValuePair<Vector2,bool> kvp in _InteractedPoints)
        {
            _InteractedPoints[kvp.Key] = false;
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

    }

    /// <summary>
    /// Attempts to raycast or point/circle overlap at the given location to "collide" with an object.
    /// On succes it will try to fetch a class implementing I_SmartwallInteractable on the gameobject 
    /// it hit and call the Hit(Vector3 hitPosition).
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
                        hit.transform.gameObject.GetComponent<I_SmartwallInteractable>()?.Hit(hit.point);
                    }
                }
                else
                {
                    RaycastHit hit = new RaycastHit();
                    if (Physics.Raycast(ray.origin, ray.direction, out hit, Mathf.Infinity))
                    {
                        hit.transform.gameObject.GetComponent<I_SmartwallInteractable>()?.Hit(hit.point);
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
                    hit2D.gameObject.GetComponent<I_SmartwallInteractable>()?.Hit(new Vector3(point.x, point.y, 0f));
                    //?. means only do if object is not null
                }
                break;
            case InputTypes.Physical:
                if (Ball == null)
                {
                    Debug.LogError("BlobInputProcessing | InteractInput | Missing throwable object. Please link the default 'ball' provided in the package.");
                    return;
                }
                Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 0f));
                GameObject ball = Instantiate(Ball, ray2.origin, Quaternion.identity);
                ball.transform.localScale = new Vector3(size * Screen.width, size * Screen.width, size * Screen.width);
                try
                {
                    ball.GetComponent<Rigidbody>().AddForce(ray2.direction * Force, ForceMode.Impulse);
                }
                catch (NullReferenceException nREx)
                {
                    Debug.LogError("BlobInputProcessing | InteractInput | You have changed the throw object but the new object is missing a Rigidbody!");
                }
                break;
        }

    }
}
