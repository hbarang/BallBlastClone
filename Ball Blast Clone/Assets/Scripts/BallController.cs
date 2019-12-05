using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    private int _hp = 15;

    public int Hp
    {
        get
        {
            return _hp;
        }
        set
        {
            if (value <= 0)
            {
                HpDecreaseEvent(_hp);
                HpZeroEvent();
            }

            else
            {
                int decrease = _hp - value;
                _hp = value;
                if (HpChangedEvent != null)
                {
                    HpChangedEvent(_hp);
                }

                if (HpDecreaseEvent != null)
                {
                    HpDecreaseEvent(decrease);
                }
            }
        }
    }


    public delegate void OnHpChange(int currentHp);
    public event OnHpChange HpChangedEvent;
    public event OnHpChange HpDecreaseEvent;
    public delegate void OnHpZero();
    public event OnHpZero HpZeroEvent;



    private Text hpText;

    private Rigidbody ballRigidBody;

    private bool _splittable = false;
    public bool Splittable
    {
        get
        {
            return _splittable;
        }
        set
        {
            _splittable = value;
        }
    }

    public int[] splits;

    public GameObject ballPrefab;


    private float firstHitVelocity = 0;
    private float minimumVelocity = 8;
    private bool isFirstHit = true;


    private Vector3 _spawnHorizontalDirection;
    public Vector3 SpawnHorizontalDirection
    {
        get
        {
            return _spawnHorizontalDirection;
        }
        set
        {
            _spawnHorizontalDirection = value;
        }
    }

    private void Awake()
    {

        hpText = GetComponentInChildren<Text>();
        ballRigidBody = GetComponent<Rigidbody>();

    }

    private void Start()
    {
        ChangeBallText(_hp);
        ballRigidBody.AddForce(SpawnHorizontalDirection * Random.Range(15f, 30f));

        HpChangedEvent += ChangeBallText;
        HpZeroEvent += Split;
        PlayerController.Instance.PlayerHitEvent += DestroyOnGameOver;

    }


    private void OnCollisionEnter(Collision other)
    {
        Vector3 newVelocity = other.relativeVelocity;
        string collidedObjectTag = other.gameObject.tag;

        newVelocity.y = firstHitVelocity == 0 ? other.relativeVelocity.y : firstHitVelocity;

        if (collidedObjectTag == Tags.Ground)
        {
            if (isFirstHit)
            {
                firstHitVelocity = newVelocity.y < minimumVelocity ? minimumVelocity : newVelocity.y;
            }
            newVelocity.x *= -1;
            ballRigidBody.velocity = newVelocity;
        }

        if (collidedObjectTag == Tags.BorderLeft || collidedObjectTag == Tags.BorderRight)
        {
            newVelocity.y = ballRigidBody.velocity.y;
            ballRigidBody.velocity = newVelocity;
        }

    }

    private void OnTriggerEnter(Collider other)
    {

        Hp -= other.gameObject.GetComponent<BulletController>().bulletDamage;
        Destroy(other.gameObject);
    }


    private void ChangeBallText(int currentHp)
    {
        hpText.text = currentHp.ToString();
    }


    private void Split()
    {

        if (Splittable)
        {

            CreateBall(splits[0], Vector3.right);
            CreateBall(splits[1], Vector3.left);

            Destroy(this.gameObject);

        }
        else
        {
            Destroy(this.gameObject);//This might be a problem, maybe wait for a few seconds before destroying -- delegate issues
        }
    }


    void CreateBall(int hp, Vector3 spawnDirection)
    {
        GameObject split;
        BallController ballController;
        split = Instantiate(ballPrefab, transform.position, Quaternion.identity);

        ballController = split.GetComponent<BallController>();

        ballController.SpawnHorizontalDirection = spawnDirection;
        ballController.Hp = hp;
        ballController.HpDecreaseEvent += GameManager.Instance.ChangeDamageDecreased;

        split.SetActive(true);
    }


    private void OnDestroy()
    {
        HpChangedEvent -= ChangeBallText;
        HpDecreaseEvent -= GameManager.Instance.ChangeDamageDecreased;
        PlayerController.Instance.PlayerHitEvent -= DestroyOnGameOver;

        if (!GameManager.Instance.GameOver)
        {
            GameManager.Instance.BallsSpawned -= 1;
        }

    }

    void DestroyOnGameOver()
    {
        Destroy(this.gameObject);
    }


}
