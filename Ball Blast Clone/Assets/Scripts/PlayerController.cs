using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;


    void Update()
    {

        if (Input.GetMouseButton(0))
        {
            float step = speed * Time.deltaTime;
            Vector3 target = transform.position;
            target.x = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }
    }

}
