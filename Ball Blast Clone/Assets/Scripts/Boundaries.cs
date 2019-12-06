using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{
    private Vector2 _screenBounds;
    public Vector2 ScreenBounds
    {
        get
        {
            return _screenBounds;
        }
        set
        {
            _screenBounds = value;

            if (OnScreenBoundsChangeEvent != null)
            {
                OnScreenBoundsChangeEvent();
            }
        }
    }

    public delegate void OnScreenBoundsChange();
    public event OnScreenBoundsChange OnScreenBoundsChangeEvent;

    Resolution res;


    public static Boundaries Instance;
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(Instance);
        }

        res = Screen.currentResolution;

        ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

    }


    private void Update()
    {

        if (res.height != Screen.height || res.width != Screen.width)
        {
            ScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        }

    }


}

