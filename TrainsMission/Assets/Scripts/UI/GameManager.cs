using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public UIManager UI;
    public GameObject pannle;

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    public void TogglePauseMenu()
    {
        
        // not the optimal way but for the sake of readability
        if (pannle.activeSelf)
        {
            pannle.SetActive(false);
            Time.timeScale = 1.0f;
        }
        else
        {
            pannle.SetActive(true);
            Time.timeScale = 0f;
        }

        Debug.Log("GAMEMANAGER:: TimeScale: " + Time.timeScale);
    }
}
