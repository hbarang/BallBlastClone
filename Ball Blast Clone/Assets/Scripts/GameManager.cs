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

        Debug.Log(gameManagerParameters);


        spawnPosition = new Vector3(-Boundaries.ScreenBounds.x, Boundaries.ScreenBounds.y, 0);
        GameObject Ball = Instantiate(ballPrefab, spawnPosition, Quaternion.identity);
        Ball.SetActive(true);
    }


    public void LoadJson()
    {

        jsonString = File.ReadAllText(jsonPath);
        gameManagerParameters = GameManagerParameters.CreateFromJSON(jsonString);

    }


}
