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
/// </summary>
public class BlobInputProcessing : MonoBehaviour
{
    public InputTypes InputType;
    public CameraTypes CameraType;
    public bool AverageAllInputsToOnePoint;
    /// <summary>
    /// Input will only trigger once withing the area of the input untill that area is clear of blobs.
    /// Use this to prevent multiple inputs with one ball throw. It is in screen width size percentages.
    /// 130% will cover the whole screen irregardless of the input's position.
    /// and .
    /// </summary>
    [Range(0,130)]
    public byte UninteractableAreaSize;
    [Space(20)]
    [Header("Settings for physical interaction type.")]
    public byte Size;
    public byte Force;
    [Space(5)]
    [Header("Do not change!")]
    public GameObject Ball;

    //Every frame we check for blob and mouse input
    private void FixedUpdate()
    {
        if (AverageAllInputsToOnePoint)
        {
            Vector2 total = new Vector2();
            foreach (Blob b in BlobDetectionGateway.Blobs)
            {
                total += b.Position;
            }
            InteractInput(total / BlobDetectionGateway.Blobs.Count);
        }
        else
        {
            foreach (Blob b in BlobDetectionGateway.Blobs)
            {
                InteractInput(b.Position);
            }
        }
        if (Input.GetMouseButton(0))
        {
            InteractInput(Input.mousePosition);
        }
    }

    /// <summary>
    /// Attempts to raycast of point overlap at the given location to "collide" with an object.
    /// On succes it will try to fetch a class implementing I_SmartwallInteractable on the gameobject 
    /// it hit and call the Hit(Vector3 hitPosition).
    /// </summary>
    private void InteractInput(Vector2 screenPosition)
    {
        switch (InputType)
        {
            case InputTypes.Raycast3D:
                RaycastHit hit = new RaycastHit();
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 0f));
                Debug.DrawRay(ray.origin, ray.direction);
                if (Physics.Raycast(ray.origin, ray.direction, out hit, 300, 1 << 8))
                {
                    Debug.Log(hit.transform.name);
                    hit.transform.gameObject.GetComponent<I_SmartwallInteractable>().Hit(hit.point);
                }
                break;
            case InputTypes.Raycast2D:
                Vector2 point = Camera.main.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, 0f));
                Collider2D hit2D = Physics2D.OverlapPoint(point);
                if(hit2D != null)
                {
                    hit2D.gameObject.GetComponent<I_SmartwallInteractable>().Hit(new Vector3(point.x,point.y,0f));
                }
                break;
            case InputTypes.Physical:
                Ray ray2 = Camera.main.ScreenPointToRay(new Vector3(screenPosition.x, screenPosition.y, 0f));
                GameObject ball = Instantiate(Ball, ray2.origin, Quaternion.identity);
                ball.transform.localScale = new Vector3(Size, Size, Size);
                ball.GetComponent<Rigidbody>().AddForce(ray2.direction * Force,ForceMode.Impulse);
                break;
        }

    }
}
