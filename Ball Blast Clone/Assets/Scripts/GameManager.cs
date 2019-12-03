using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject ballPrefab;
    Vector3 spawnPosition;

    string jsonPath = "Assets/level.json";
    string jsonString;

    private int _currentLevel = 1;
    private int _ballsSpawned = 0;
    private float _levelTimer = 0;

    GameManagerParameters gameManagerParameters;

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

    private void Start()
    {
        LoadJson();
        Physics.gravity = new Vector3(0, gameManagerParameters.gravity, 0);
        SpawnBall();
        
    }


    public void LoadJson()
    {
        jsonString = File.ReadAllText(jsonPath);
        gameManagerParameters = GameManagerParameters.CreateFromJSON(jsonString);
    }


    public void SpawnBall(){
        spawnPosition = new Vector3(-Boundaries.ScreenBounds.x, Boundaries.ScreenBounds.y, 0);
        foreach (Ball item in gameManagerParameters.levels[_currentLevel-1].balls)
        {
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            BallController ballController = ball.GetComponent<BallController>();
            
            ballController.Hp = item.hp;
            ballController.splits = item.splits;

            StartCoroutine(WaitForDelay(ball, item.delay));
        }
    }

    IEnumerator WaitForDelay(GameObject ball, int delay)
    {

        yield return new WaitForSeconds(delay);
        ball.SetActive(true);

    }


}
