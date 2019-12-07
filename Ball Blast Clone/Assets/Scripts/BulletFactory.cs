using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFactory : MonoBehaviour
{
    public static BulletFactory Instance;
    public Queue<GameObject> BulletQueue;
    public GameObject BulletPrefab;

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

        BulletQueue = new Queue<GameObject>();

    }

    public GameObject GetBullet(Vector3 position)
    {
        GameObject ballToReturn;

        if (BulletQueue.Count != 0)
        {
            ballToReturn = BulletQueue.Dequeue();
            ballToReturn.transform.position = position;
            ballToReturn.SetActive(true);
            return ballToReturn;
        }
        else
        {
            ballToReturn = Instantiate(BulletPrefab, position, Quaternion.identity);
            return ballToReturn;
        }
    }

    public void ReleaseBullet(GameObject ball)
    {
        ball.SetActive(false);
        BulletQueue.Enqueue(ball);
    }

    

}
