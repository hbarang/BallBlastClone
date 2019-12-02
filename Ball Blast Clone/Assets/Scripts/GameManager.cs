using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private static List<Level> Levels;
    private static float _gravity;
    private static int _bulletCountIncrease;
    private static int _bulletDamageIncrease;

    public GameObject ballPrefab;

    Vector3 spawnPosition;
    void Awake()
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

    private void Start() {
        spawnPosition = new Vector3(-Boundaries.ScreenBounds.x, Boundaries.ScreenBounds.y, 0);
        GameObject Ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        Ball.SetActive(true);
    }




}
