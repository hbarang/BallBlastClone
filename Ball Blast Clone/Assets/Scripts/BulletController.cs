using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;
    private Vector2 screenBounds;

    public int bulletDamage;


    private void Start()
    {
        bulletDamage = GameManager.Instance.BulletDamage;
    }

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        if (transform.position.y > Boundaries.Instance.ScreenBounds.y)
        {
            Destroy(this.gameObject);
        }

    }






}
