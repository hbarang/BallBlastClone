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

    private int _currentLevel = 0;
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            _currentLevel = value;
            CurrentLevelHpDecrease = 0;
            if (LevelChangedEvent != null)
            {
                LevelChangedEvent();
            }
        }
    }

    public delegate void OnLevelChange();
    public event OnLevelChange LevelChangedEvent;

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


    public GameManagerParameters gameManagerParameters;

    Level generatedLevel;
    int lvl5Hp;

    int _bulletDamage = 1;

    public int BulletDamage
    {
        get
        {
            return _bulletDamage;
        }
    }

    int _currentLevelTotalHp;
    public int CurrentLevelTotalHp
    {
        get
        {
            return _currentLevelTotalHp;
        }
        set
        {
            _currentLevelTotalHp = value;

            if (CurrentLevelTotalHpChanged != null)
            {
                CurrentLevelTotalHpChanged(_currentLevelTotalHp);
            }
        }
    }

    public delegate void OnCurrentLevelTotalHpChange(int hp);
    public event OnCurrentLevelTotalHpChange CurrentLevelTotalHpChanged;


    int _currentLevelHpDecrease;
    public int CurrentLevelHpDecrease
    {
        get
        {
            return _currentLevelHpDecrease;
        }
        set
        {
            _currentLevelHpDecrease = value;

            if (CurrentLevelDamageChanged != null)
            {
                CurrentLevelDamageChanged(_currentLevelHpDecrease);
            }
        }
    }
    public delegate void OnCurrentLevelHpDecreaseChange(int damage);
    public event OnCurrentLevelHpDecreaseChange CurrentLevelDamageChanged;

    private bool _gameStarted = false;
    public bool GameStarted
    {
        get
        {
            return _gameStarted;
        }
        set
        {
            _gameStarted = value;
            if (value)
            {
                GameStartedEvent();
            }
        }
    }
    public delegate void OnGameStart();
    public event OnGameStart GameStartedEvent;
    

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
        setLevel5Hp();

        LevelChangedEvent += SpawnBall;
        LevelChangedEvent += ChangeBulletDamage;
        UIManager.Instance.TouchToPlayButton.onClick.AddListener(StartGame);

    }

    void StartGame()
    {
        CurrentLevel = 1;
        GameStarted = true;
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
            CurrentLevelTotalHp = SpawnBallFromLevel(gameManagerParameters.levels[_currentLevel - 1]);
            BallsSpawned = gameManagerParameters.levels[CurrentLevel - 1].balls.Length * 3;
        }

        else
        {
            GenerateLevel(4);
            CurrentLevelTotalHp = SpawnBallFromLevel(generatedLevel);
            BallsSpawned = generatedLevel.balls.Length * 3;
        }



    }


    public void GenerateLevel(int numberOfBalls)
    {
        int newLevelHp = ((Random.value > 0.5f) ? lvl5Hp * 80 : lvl5Hp * 120) / 100;
        int initialBallsTotalHp = newLevelHp / 2;
        int splitBallsTotalHp = initialBallsTotalHp;

        List<int> initialBallsHpList = new List<int>();
        List<int> splitBallsHpList = new List<int>();

        generatedLevel = new Level(numberOfBalls);

        for (int initialBallIndex = 0; initialBallIndex < numberOfBalls; initialBallIndex++)
        {

            if (initialBallIndex == numberOfBalls - 1)
            {
                initialBallsHpList.Add(initialBallsTotalHp);
                splitBallsHpList.Add(splitBallsTotalHp);
                break;
            }

            initialBallsHpList.Add(Random.Range(6, initialBallsTotalHp / 2));
            initialBallsTotalHp -= initialBallsHpList[initialBallIndex];

            splitBallsHpList.Add(Random.Range(initialBallsHpList[initialBallIndex] / 2, initialBallsHpList[initialBallIndex]));
            splitBallsTotalHp -= splitBallsHpList[initialBallIndex] * 2;


        }

        int indexCounter = 0;

        foreach (Ball item in generatedLevel.balls)
        {
            item.hp = initialBallsHpList[indexCounter];
            item.splits[0] = item.splits[1] = splitBallsHpList[indexCounter] / 2;
            item.delay = indexCounter * Random.Range(1, 4);
            indexCounter += 1;
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

        ballController.HpDecreaseEvent += ChangeDamageDecreased;

        StartCoroutine(WaitForDelay(ball, delay));

    }

    public int SpawnBallFromLevel(Level level)
    {
        int totalHp = 0;
        foreach (Ball item in level.balls)
        {
            bool spawnRight = (Random.value > 0.5f);
            float xPosition = (spawnRight ? 1 : -1) * Boundaries.Instance.ScreenBounds.x + (spawnRight ? -1f : 1f);
            spawnPosition = new Vector3(xPosition, Boundaries.Instance.ScreenBounds.y, 0);

            IndividualBallSpawn(spawnPosition, item.hp, item.splits, spawnRight ? Vector3.left : Vector3.right, true, item.delay);
            totalHp += item.hp + item.splits[0] + item.splits[1];
        }

        return totalHp;
    }



    IEnumerator WaitForDelay(GameObject ball, int delay)
    {

        yield return new WaitForSeconds(delay);
        ball.SetActive(true);

    }


    void ChangeBulletDamage()
    {
        _bulletDamage += gameManagerParameters.bullet_damage_increase;
    }

    public void ChangeDamageDecreased(int damage)
    {
        CurrentLevelHpDecrease += damage;
    }

}
