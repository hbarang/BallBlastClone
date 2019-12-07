using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectFactory : MonoBehaviour
{
    public static GameObjectFactory Instance;
    public Queue<GameObject> BulletQueue;
    public Queue<GameObject> HitEffectQueue;
    public GameObject BulletPrefab;
    public GameObject HitEffectPrefab;

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
        HitEffectQueue = new Queue<GameObject>();

    }

    private void Update()
    {

    }
    public GameObject GetBullet(Vector3 position)
    {
        GameObject ballToReturn;

        if (BulletQueue.Count == 0)
        {
            ballToReturn = Instantiate(BulletPrefab, position, Quaternion.identity);
            return ballToReturn;
        }
        else
        {
            ballToReturn = BulletQueue.Dequeue();
            ballToReturn.transform.position = position;
            ballToReturn.SetActive(true);
            return ballToReturn;
        }
    }

    public void ReleaseBullet(GameObject ball)
    {
        ball.SetActive(false);
        BulletQueue.Enqueue(ball);
    }

    public GameObject CreateHitEffect(Vector3 position)
    {
        GameObject hitEffectToReturn;

        if (HitEffectQueue.Count != 0)
        {
            hitEffectToReturn = HitEffectQueue.Dequeue();
            hitEffectToReturn.transform.position = position;
            hitEffectToReturn.SetActive(true);
        }
        else
        {
            hitEffectToReturn = Instantiate(HitEffectPrefab, position, Quaternion.identity);
        }

        hitEffectToReturn.GetComponent<VFXController>().CallCoroutine();
        return hitEffectToReturn;
    }
    public void ReleaseHitEffect(GameObject hitEffect)
    {
        HitEffectQueue.Enqueue(hitEffect);
    }




}
