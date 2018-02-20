using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine;

public class LevelData : MonoBehaviour
{
    public Level CurrentLevel;
    public List<Level> LevelList = new List<Level>();
    public static List<Game> savedGames = new List<Game>();
    void Start()
    {
        DontDestroyOnLoad(this);
        Debug.Log(CurrentLevel.name);
    }

    public void LevelSelector(int selectorNum)
    {
        CurrentLevel = LevelList[selectorNum-1];
        SceneManager.LoadScene(2);
    }

    public void Save()
    {
        Game.current.currentLevel = CurrentLevel;
        savedGames.Add(Game.current);
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/savedGames.gd");
        bf.Serialize(file, LevelData.savedGames);
        file.Close();
    }
    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedGames.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/savedGames.gd", FileMode.Open);
            LevelData.savedGames = (List<Game>)bf.Deserialize(file);
            file.Close();
        }
    }

}

[System.Serializable]
public class Game
{
    public static Game current;
    public Level currentLevel;
}

//level data structure
[System.Serializable]
public class Level
{
    public GameMode mode;
    public string name;
    public int number;
    public int mapSize;
    public int stationNum;
    public int lineNum;
    public int stationsPerLine;
    public Transform[] stationPrefabs;
}

//map types
[System.Serializable]
public enum GameMode
{
    Campaign,
    Tutorial
}