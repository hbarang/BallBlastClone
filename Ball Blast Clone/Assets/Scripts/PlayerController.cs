using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    public float bulletSpawnRate;
    public GameObject bulletPrefab;



    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke();

        }

        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("SpawnBullets", 0.3f, bulletSpawnRate);
        }

        if (Input.GetMouseButton(0))
        {
            float step = speed * Time.deltaTime;
            Vector3 target = transform.position;
            float targetXValue = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            target.x = targetXValue > Boundaries.ScreenBounds.x ? Boundaries.ScreenBounds.x : targetXValue < -Boundaries.ScreenBounds.x ? -Boundaries.ScreenBounds.x : targetXValue;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }


    }


    void SpawnBullets()
    {
        Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
    }

}
