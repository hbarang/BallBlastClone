using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

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

    Level generatedLevel;
    int lvl5Hp;

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
        setLevel5Hp();
        GenerateLevel();
    }

    void setLevel5Hp()
    {
        foreach (Ball item in gameManagerParameters.levels[4].balls)
        {
            lvl5Hp += item.hp + item.splits[0] + item.splits[1];
        }
    }

    public void LoadJson()
    {
        jsonString = File.ReadAllText(jsonPath);
        gameManagerParameters = GameManagerParameters.CreateFromJSON(jsonString);
    }


    public void SpawnBall()
    {

        if (_currentLevel <= 5)
        {
            foreach (Ball item in gameManagerParameters.levels[_currentLevel - 1].balls)
            {
                bool spawnRight = (Random.value > 0.5f);
                float xPosition = (spawnRight ? 1 : -1) * Boundaries.ScreenBounds.x + (spawnRight ? -1f : 1f);
                spawnPosition = new Vector3(xPosition, Boundaries.ScreenBounds.y, 0);

                IndividualBallSpawn(spawnPosition, item.hp, item.splits, spawnRight ? Vector3.left : Vector3.right, true, item.delay);

            }
        }
        else
        {

        }
        BallsSpawned = gameManagerParameters.levels[CurrentLevel - 1].balls.Length * 3;

    }

    public void GenerateLevel()
    {
        int newLevelHp = ((Random.value > 0.5f) ? lvl5Hp * 80 : lvl5Hp * 120) / 100;
        int initialBallsTotalHp = newLevelHp / 2;
        int splitBallsTotalHp = initialBallsTotalHp;

        List<int> initialBallsHpList = new List<int>();
        List<int> splitBallsHpList = new List<int>();


        for (int initialBallIndex = 0; initialBallIndex < 4; initialBallIndex++)
        {

            if (initialBallIndex == 3)
            {
                initialBallsHpList.Add(initialBallsTotalHp);
                splitBallsHpList.Add(splitBallsTotalHp);
                break;
            }

            initialBallsHpList.Add(Random.Range(6, initialBallsTotalHp/2));
            initialBallsTotalHp -= initialBallsHpList[initialBallIndex];

            splitBallsHpList.Add(Random.Range(initialBallsHpList[initialBallIndex]/2, initialBallsHpList[initialBallIndex]));    
            splitBallsTotalHp -= splitBallsHpList[initialBallIndex] * 2;


        }

    }


    public void IndividualBallSpawn(Vector3 spawnPosition, int hp, int[] splits, Vector3 SpawnDirection, bool splittable, int delay)
    {
        GameObject ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        BallController ballController = ball.GetComponent<BallController>();

        ballController.Hp = hp;
        ballController.splits = splits;
        ballController.SpawnHorizontalDirection = SpawnDirection;
        ballController.Splittable = splittable;

        StartCoroutine(WaitForDelay(ball, delay));

    }


    IEnumerator WaitForDelay(GameObject ball, int delay)
    {

        yield return new WaitForSeconds(delay);
        ball.SetActive(true);

    }


}
