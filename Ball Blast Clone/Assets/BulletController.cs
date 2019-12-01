using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    private Vector2 screenBounds;

    private void Awake()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }


    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (transform.position.y > screenBounds.y)
        {
            Destroy(this.gameObject);
        }
    }



}
