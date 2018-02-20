using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartUpOnClick : MonoBehaviour {

    public void Begin(int Index)
    {
        //LevelData levelScript = GameObject.Find("LevelManager").GetComponent<LevelData>();
        //levelScript.CurrentLevel = levelScript.LevelList[0];
        SceneManager.LoadScene(Index);
    }
}
