using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArea : MonoBehaviour
{
    [SerializeField] private PlayArea Area;

    /// <summary>
    /// Get a random position somewhere in the play area.
    /// </summary>
    public Vector2 GetSpawnPosition()
    {
        Vector2 position = new Vector2(
            Random.Range(Area.TopLeft.position.x, Area.BottomRight.position.x),
            Random.Range(Area.TopLeft.position.y, Area.BottomRight.position.y));

        return position;
    }

    /// <summary>
    /// Get the position on the play area relative to the to the percentual value of x and y, where 0,0 is the top-left corner of the area.
    /// </summary>
    /// <param name="x">The percentual value of the x-axis (between 0 and 1).</param>
    /// <param name="y">The percentual value of the y-axis (between 0 and 1).</param>
    /// <returns></returns>
    public Vector2 GetSpawnPosition(float x, float y)
    {
        x = Mathf.Clamp01(x);
        y = Mathf.Clamp01(y);

        Vector2 position = new Vector2(
            Mathf.Lerp(Area.TopLeft.position.x, Area.BottomRight.position.x, x),
            Mathf.Lerp(Area.TopLeft.position.y, Area.BottomRight.position.y, y)
            );

        return position;
    }

    [System.Serializable]
    private class PlayArea
    {
        public Transform TopLeft;
        public Transform BottomRight;
    }
}