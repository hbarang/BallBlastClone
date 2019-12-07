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

    string jsonPath = "level";

    List<GameObject> InstantiatedBalls;

    private int _currentLevel = 6;
    public int CurrentLevel
    {
        get
        {
            return _currentLevel;
        }
        set
        {
            if (!GameOver)
            {
                _currentLevel = value;

                if (ActivateGameWonEvent != null && value != 1)
                {
                    Time.timeScale = 0f;
                    ActivateGameWonEvent();
                }

                CurrentLevelHpDecrease = 0;

                if (LevelChangedEvent != null)
                {
                    LevelChangedEvent();
                }
            }

        }

    }

    public delegate void OnLevelChange();
    public event OnLevelChange LevelChangedEvent;

    public event OnLevelChange ActivateGameWonEvent;

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

            if (value == 0 && !GameOver)
            {
                CurrentLevel += 1;
            }
        }
    }


    public GameManagerParameters gameManagerParameters;

    Level generatedLevel;
    int handPickedLevelCapHp;

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

    public bool GameOver = false;

    public int HandPickedLevelCap = 5;
    public int MaximumBallCap = 4;
    public int BulletPerSecondCap;
    public int BulletDamageCap;

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
        SetLevelCapHp();

        LevelChangedEvent += SpawnBall;
        LevelChangedEvent += ChangeBulletDamage;

        PlayerController.Instance.PlayerHitEvent += ReplayLevel;

        //UIManager.Instance.TouchToPlayButton.onClick.AddListener(StartGame);

        InstantiatedBalls = new List<GameObject>();
        Time.timeScale = 0f;
        
        BulletPerSecondCap = 1 + HandPickedLevelCap * gameManagerParameters.bullet_count_increase;
        BulletDamageCap = 1 + HandPickedLevelCap * gameManagerParameters.bullet_damage_increase;

    }


    public void StartGame()
    {

        CurrentLevel = 1;
        GameStarted = true;
        Time.timeScale = 1f;

    }


    void SetLevelCapHp()
    {
        foreach (Ball item in gameManagerParameters.levels[HandPickedLevelCap - 1].balls)
        {
            handPickedLevelCapHp += item.hp + item.splits[0] + item.splits[1];
        }
    }


    public void LoadJson()
    {

        TextAsset levelFile = Resources.Load(jsonPath) as TextAsset;
        gameManagerParameters = GameManagerParameters.CreateFromJSON(levelFile);
        
    }


    public void SpawnBall()
    {

        if (_currentLevel <= HandPickedLevelCap)
        {
            CurrentLevelTotalHp = SpawnBallFromLevel(gameManagerParameters.levels[_currentLevel - 1]);
            BallsSpawned = gameManagerParameters.levels[CurrentLevel - 1].balls.Length * 3;
        }

        else
        {
            GenerateLevel(MaximumBallCap);
            CurrentLevelTotalHp = SpawnBallFromLevel(generatedLevel);
            BallsSpawned = generatedLevel.balls.Length * 3;
        }



    }

    private int GeneratedLevelHpLowerPercantage = 80;
    private int GeneratedLevelHpUpperPercantage = 80;
    public void GenerateLevel(int numberOfBalls)
    {
        int newLevelHp = ((Random.value > 0.5f) ? handPickedLevelCapHp * GeneratedLevelHpLowerPercantage : handPickedLevelCapHp * GeneratedLevelHpUpperPercantage) / 100;
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
        InstantiatedBalls.Add(ball);
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
            spawnPosition = new Vector3(xPosition, Boundaries.Instance.ScreenBounds.y - 1, 0);

            IndividualBallSpawn(spawnPosition, item.hp, item.splits, spawnRight ? Vector3.left : Vector3.right, true, item.delay);
            totalHp += item.hp + item.splits[0] + item.splits[1];
        }

        return totalHp;
    }



    IEnumerator WaitForDelay(GameObject ball, int delay)
    {

        yield return new WaitForSeconds(delay);

        if (ball != null)
        {
            ball.SetActive(true);
        }


    }


    void ChangeBulletDamage()
    {
        if (_currentLevel != 1 && _bulletDamage < BulletDamageCap)
        {
            _bulletDamage += gameManagerParameters.bullet_damage_increase;
        }
    }

    public void ChangeDamageDecreased(int damage)
    {
        CurrentLevelHpDecrease += damage;
    }


    void ReplayLevel()
    {
        Time.timeScale = 0f;
        DestroyUnspawnedBalls();

        SpawnBall();
    }

    void DestroyUnspawnedBalls()
    {

        foreach (GameObject item in InstantiatedBalls)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }

        InstantiatedBalls.Clear();

    }

}
