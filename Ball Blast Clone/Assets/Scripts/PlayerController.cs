using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    public float bulletSpawnRate;
    public GameObject bulletPrefab;

    public delegate void OnPlayerHit();
    public event OnPlayerHit PlayerHitEvent;

    public static PlayerController Instance;


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

    }


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
            target.x = targetXValue > Boundaries.Instance.ScreenBounds.x ? Boundaries.Instance.ScreenBounds.x : targetXValue < -Boundaries.Instance.ScreenBounds.x ? -Boundaries.Instance.ScreenBounds.x : targetXValue;
            transform.position = Vector3.MoveTowards(transform.position, target, step);
        }


    }

    private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == Tags.Ball)
        {
            if (PlayerHitEvent != null)
            {
                GameManager.Instance.GameOver = true;
                PlayerHitEvent();
            }
        }

    }


    void SpawnBullets()
    {
        Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
    }

}
