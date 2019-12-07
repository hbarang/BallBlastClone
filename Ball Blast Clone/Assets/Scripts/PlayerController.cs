using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 4.0f;
    private int bulletPerSecond = 1;
    private float bulletSpawnRate;
    public GameObject bulletPrefab;

    public delegate void OnPlayerHit();
    public event OnPlayerHit PlayerHitEvent;

    public static PlayerController Instance;

    private float originalResolutionScale = 0.6f;

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
        this.transform.position = new Vector3(transform.position.x, -Boundaries.Instance.ScreenBounds.y + 3.5f, transform.position.z);        
    }

    void Update()
    {

        if (Input.touchSupported)
        {

            if (Input.touchCount > 0)
            {
                Touch input = Input.touches[0];

                if (input.phase == TouchPhase.Began)
                {
                    ChangePlayerPosition(Camera.main.ScreenToWorldPoint(input.position).x);
                    InvokeRepeating("SpawnBullets", bulletSpawnRate, bulletSpawnRate);
                }

                if (input.phase == TouchPhase.Moved)
                {

                    ChangePlayerPosition(Camera.main.ScreenToWorldPoint(input.position).x);

                }

                if (input.phase == TouchPhase.Ended)
                {
                    CancelInvoke();
                }

            }

        }

        else
        {

            if (Input.GetMouseButtonUp(0))
            {
                CancelInvoke();

            }

            if (Input.GetMouseButtonDown(0))
            {
                InvokeRepeating("SpawnBullets", bulletSpawnRate, bulletSpawnRate);
            }

            if (Input.GetMouseButton(0))
            {

                ChangePlayerPosition(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);

            }
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

    private float playerObjectHalfSize = 0.3f;
    void ChangePlayerPosition(float xPosition)
    {

        float step = speed * Time.deltaTime;
        Vector3 target = transform.position;
        target.x = xPosition > Boundaries.Instance.ScreenBounds.x - playerObjectHalfSize ? Boundaries.Instance.ScreenBounds.x - playerObjectHalfSize : xPosition < -Boundaries.Instance.ScreenBounds.x + playerObjectHalfSize ? -Boundaries.Instance.ScreenBounds.x + playerObjectHalfSize : xPosition;
        transform.position = Vector3.MoveTowards(transform.position, target, step);

    }
}
