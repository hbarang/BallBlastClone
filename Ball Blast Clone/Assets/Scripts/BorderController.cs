using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderController : MonoBehaviour
{
    // Start is called before the first frame update


    void Start()
    {
        AdjustPosition();
        Boundaries.Instance.OnScreenBoundsChangeEvent += AdjustPosition;
    }

    void AdjustPosition()
    {

        if (tag == Tags.BorderRight)
        {
            transform.position = new Vector3(Boundaries.Instance.ScreenBounds.x, -Boundaries.Instance.ScreenBounds.y, transform.position.z);
        }
        if (tag == Tags.BorderLeft)
        {
            transform.position = new Vector3(-Boundaries.Instance.ScreenBounds.x, -Boundaries.Instance.ScreenBounds.y, transform.position.z);

        }
        if (tag == Tags.BorderUpper)
        {
            transform.position = new Vector3(0, Boundaries.Instance.ScreenBounds.y, transform.position.z);
        }
        if (tag == Tags.Ground)
        {
            transform.position = new Vector3(0, -Boundaries.Instance.ScreenBounds.y + 2.5f, transform.position.z);
        }
        
    }

    private void OnDestroy()
    {

        Boundaries.Instance.OnScreenBoundsChangeEvent -= AdjustPosition;

    }

}
