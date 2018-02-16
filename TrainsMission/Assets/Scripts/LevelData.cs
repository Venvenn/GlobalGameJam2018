using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public Level CurrentLevel;
    public List<Level> LevelList = new List<Level>();
    void Start()
    {
        DontDestroyOnLoad(this);
    }


}

//level data structure
[System.Serializable]
public class Level
{
    public GameMode mode;
    public string name;
    public int mapSize;
    public int stationNum;
    public int lineNum;
    public int stationsPerLine;
}

//map types
[System.Serializable]
public enum GameMode
{
    Campaign,
    Tutorial
}