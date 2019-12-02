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


        if (collidedObjectTag == Tags.Ground)
        {
            newVelocity.x *= -1;
            ballRigidBody.velocity = newVelocity;
        }

        if (collidedObjectTag == Tags.BorderLeft || collidedObjectTag == Tags.BorderRight)
        {
            newVelocity.y = ballRigidBody.velocity.y;
            ballRigidBody.velocity = newVelocity;
        }

    }

    private void ChangeBallText(int currentHp){
        hpText.text = currentHp.ToString();
    }




}
