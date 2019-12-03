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
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            _currentLevel = value;

            if (LevelChangedEvent != null)
            {
                LevelChangedEvent();
            }
        }
    }

    public delegate void OnLevelChange();
    public static event OnLevelChange LevelChangedEvent;

    private int _ballsSpawned = 0;
    public int BallsSpawned
    {
        get
        {
            return _ballsSpawned;
        }

        set
        {
            _ballsSpawned = value;

            if (value == 0)
            {
                CurrentLevel += 1;
            }
        }
    }


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
        LevelChangedEvent += SpawnBall;
    }



    public void LoadJson()
    {
        jsonString = File.ReadAllText(jsonPath);
        gameManagerParameters = GameManagerParameters.CreateFromJSON(jsonString);
    }


    public void SpawnBall()
    {

        foreach (Ball item in gameManagerParameters.levels[_currentLevel - 1].balls)
        {
            bool spawnRight = (Random.value > 0.5f);
            float xPosition = (spawnRight ? 1 : -1) * Boundaries.ScreenBounds.x + (spawnRight ? -1f : 1f);
            spawnPosition = new Vector3(xPosition, Boundaries.ScreenBounds.y, 0);
            GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
            BallController ballController = ball.GetComponent<BallController>();

            ballController.Hp = item.hp;
            ballController.splits = item.splits;
            ballController.SpawnHorizontalDirection = spawnRight ? Vector3.left : Vector3.right;
            ballController.Splittable = true;

            StartCoroutine(WaitForDelay(ball, item.delay));
        }
        BallsSpawned = gameManagerParameters.levels[CurrentLevel - 1].balls.Length * 3;

    }


    IEnumerator WaitForDelay(GameObject ball, int delay)
    {

        yield return new WaitForSeconds(delay);
        ball.SetActive(true);

    }


}
