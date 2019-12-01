using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector2 screenBounds;

    private void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void Start()
    {
        if (tag == Tags.BorderRight)
        {
            transform.position = new Vector3(screenBounds.x, -screenBounds.y, transform.position.z);
        }
        if (tag == Tags.BorderLeft)
        {
            transform.position = new Vector3(-screenBounds.x, -screenBounds.y, transform.position.z);

        }
    }

}
