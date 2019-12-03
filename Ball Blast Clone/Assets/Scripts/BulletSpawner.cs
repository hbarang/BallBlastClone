using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    private float spawnRate;
    public GameObject bulletPrefab;


    void Start()
    {
        InvokeRepeating("SpawnBullets", 0.3f, 0.3f);
    }



    void SpawnBullets()
    {
        Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
    }
}
