using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Boundaries
{
    private static Vector2 _screenBounds;

    public static Vector2 ScreenBounds
    {
        get
        {
            _screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            return _screenBounds;
        }

    }

}

