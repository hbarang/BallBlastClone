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
            if (value == 0)
            {
                HpZeroEvent();
            }

            else
            {
                _hp = value;
                if (HpChangedEvent != null)
                {
                    HpChangedEvent(_hp);
                }
            }
        }
    }


    public delegate void OnHpChange(int currentHp);
    public event OnHpChange HpChangedEvent;

    public delegate void OnHpZero();
    public event OnHpZero HpZeroEvent;



    private Text hpText;

    private Rigidbody ballRigidBody;

    private bool _splittable = true;
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

    public List<int> splits;

    public GameObject ballPrefab;


    private float firstHitVelocity = 0;
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
        ballRigidBody.AddForce(Vector3.right * 20);

        HpChangedEvent += ChangeBallText;

    }


    private void OnCollisionEnter(Collision other)
    {
        Vector3 newVelocity = other.relativeVelocity;
        string collidedObjectTag = other.gameObject.tag;

        newVelocity.y = firstHitVelocity == 0 ? other.relativeVelocity.y : firstHitVelocity;

        if (collidedObjectTag == Tags.Ground)
        {
            if(isFirstHit){
                firstHitVelocity = newVelocity.y;
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


    private void ChangeBallText(int currentHp)
    {
        hpText.text = currentHp.ToString();
    }


    private void Split()
    {

        if (Splittable)
        {
            GameObject split1, split2;
            BallController ballController1, ballController2;
            split1 = Instantiate(ballPrefab, transform.position, Quaternion.identity);
            split2 = Instantiate(ballPrefab, transform.position, Quaternion.identity);

            ballController1 = split1.GetComponent<BallController>();
            ballController2 = split2.GetComponent<BallController>();

            ballController1.SpawnHorizontalDirection = Vector3.right;
            ballController2.SpawnHorizontalDirection = Vector3.left;

            ballController1.Hp = splits[0];
            ballController2.Hp = splits[1];

            ballController1.Splittable = false;
            ballController2.Splittable = false;

            split1.SetActive(true);
            split2.SetActive(true);

        }
        else
        {
            Destroy(this.gameObject);//This might be a problem, maybe wait for a few seconds before destroying -- delegate issues
        }
    }


    private void OnDestroy()
    {
        HpChangedEvent -= ChangeBallText;
    }




}
