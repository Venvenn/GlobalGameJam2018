﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class Player : MonoBehaviour
{
    WorldGrid gridScript;
    List<GameObject> trainsList;
    GameObject[] trainArray;
    GameObject trainOn;
    Light playerLight;
    public GameObject winText;
    public GameObject winPanel;
    public GameObject timeText;
    bool onTrain;
    bool win = false;
    static bool record;

    static float bestTime = 0;
    static float currentTime = 0.00f;
    int scoreTime;
    //hide win text
    private void Awake()
    {
        winText.SetActive(false);
        winPanel.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {


    }
    public void Init()
    {

        win = false;
        //reset time
        currentTime = 0.01f;
        record = true;
        //init variables
        gridScript = GameObject.Find("TrainGrid").GetComponent<WorldGrid>();
        trainsList = new List<GameObject>();
        playerLight = transform.Find("PlayerSpotlight").GetComponent<Light>();

        //fill train list with trains in scene
        for (int i = 0; i < GameObject.Find("Trains").transform.childCount; i++)
        {
            trainsList.Add(GameObject.Find("Trains").transform.GetChild(i).gameObject);
        }
       
    }
        // Update is called once per frame
    void Update()
    {

        if (record)
        {
            currentTime += 1 * Time.deltaTime;

            string mili = currentTime.ToString();

            if (currentTime < 100)
            {
                if (currentTime >= 10)
                {
                    timeText.GetComponent<Text>().text = "00:" + mili[0] + mili[1] + ":" + mili[3] + mili[4];
                }
                else
                {
                    timeText.GetComponent<Text>().text = "00:" + "0" + mili[0] + ":" + mili[2] + mili[3];
                }
            }
            else
            {
                if (currentTime >= 1000)
                {

                    timeText.GetComponent<Text>().text = mili[0] + mili[1] + ":" + mili[3] + mili[4] + ":" + mili[6] + mili[7];


                }
                else
                {
                    timeText.GetComponent<Text>().text = "0" + mili[0] + ":" + mili[1] + mili[2] + ":" + mili[4] + mili[5];
                }
            }
        }


        


        if (!onTrain)
        {
            if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
            {

                Debug.Log(trainsList.Count);
                CheckTrain();
            }
        }
        else
        {
            if (Input.GetKeyDown("space") || Input.GetMouseButtonDown(0))
            {

                Debug.Log(trainsList.Count);
                CheckPlatform();
            }
        }

    }
    void CheckTrain()
    {
        foreach(GameObject t in trainsList)
        {
            if (Vector3.Distance(transform.position, t.transform.position) < 2)
            {

                playerLight.intensity = 2;
                playerLight.spotAngle = 80;
                transform.parent = t.transform;
                transform.position = t.transform.position + (Vector3.up/3);
                onTrain = true;
            }
            Debug.Log(Vector3.Distance(transform.position, t.transform.position));
        }
    }
    void CheckPlatform()
    {
        foreach (Vector3 s in gridScript.trainLines[transform.parent.GetComponent<TrainScript>().line])
        {
            if (Vector3.Distance(transform.position, s) < 2)
            {
                transform.parent = null;
                transform.position = s + Vector3.one/2;
                playerLight.intensity = 1;
                playerLight.spotAngle = 30;
                onTrain = false;
                if (s == gridScript.EndStation.position)
                {
                    
                    win = true;
                    winText.GetComponent<Text>().text = "You Got Clarence to their Stop in: " + Math.Round((double)currentTime,2) + " Seconds";
                    winPanel.SetActive(win);
                    winText.SetActive(win);
                    StopRecord(true);
                }
            }
        }
    }

    //pause timer or save high score
    static void StopRecord(bool checkBestTime)
    {
        record = false;
        if (checkBestTime && currentTime < bestTime)
        {
            bestTime = currentTime;
            PlayerPrefs.SetFloat("Best Time", bestTime);
        }
    }

}
