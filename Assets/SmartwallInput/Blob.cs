using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Container class for Blob data, location and sizes are normalized. Pivot of the blob is centered.
/// </summary>
public class Blob
{
    public int Id;
    public float X;
    public float Y;
    public float Width;
    public float Height;

    public Blob(int id, float x, float y, float width, float height)
    {
        Id = id;
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }
}
