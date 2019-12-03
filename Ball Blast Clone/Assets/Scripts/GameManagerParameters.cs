using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameManagerParameters
{
    public Level[] levels;


    public float gravity;
    public int bullet_count_increase;
    public int bullet_damage_increase;

    public static GameManagerParameters CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<GameManagerParameters>(jsonString);
    }
}