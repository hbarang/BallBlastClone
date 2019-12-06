using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3.0f;
    private int bulletPerSecond = 1;
    private float bulletSpawnRate;
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

    private void Start()
    {
        GameManager.Instance.LevelChangedEvent += ChangeBulletSpawnRate;
        bulletSpawnRate = 1f / bulletPerSecond;
        this.transform.position = new Vector3(transform.position.x, -Boundaries.Instance.ScreenBounds.y+3.5f, transform.position.z);
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            CancelInvoke();

        }

        if (Input.GetMouseButtonDown(0))
        {
            InvokeRepeating("SpawnBullets",  bulletSpawnRate,  bulletSpawnRate);
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
        BulletFactory.Instance.GetBullet(this.transform.position);
    }


    void ChangeBulletSpawnRate()
    {
        if (bulletPerSecond < GameManager.Instance.BulletPerSecondCap)
        {
            bulletPerSecond += GameManager.Instance.gameManagerParameters.bullet_count_increase;
            bulletSpawnRate = 1f / bulletPerSecond;
        }
    }
}
