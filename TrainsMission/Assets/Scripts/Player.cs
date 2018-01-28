using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    WorldGrid gridScript;
    List<GameObject> trainsList;
    GameObject[] trainArray;
    GameObject trainOn;
    public GameObject winText;
    bool onTrain;
    bool win = false;

    // Use this for initialization
    void Start()
    {
        gridScript = GameObject.Find("TrainGrid").GetComponent<WorldGrid>();
        trainsList = new List<GameObject>();

    }
    public void Init()
    {
        for (int i = 0; i < GameObject.Find("Trains").transform.childCount; i++)
        {
            Debug.Log("hit");
            trainsList.Add(GameObject.Find("Trains").transform.GetChild(i).gameObject);
        }
    }
        // Update is called once per frame
    void Update()
    {

        if (!onTrain)
        {
            if (Input.GetKeyDown("space"))
            {

                Debug.Log(trainsList.Count);
                CheckTrain();
            }
        }
        else
        {
            if (Input.GetKeyDown("space"))
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

                transform.parent = t.transform;
                transform.position = t.transform.position;
                Debug.Log("hit train");
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
                Debug.Log("hit station");
                onTrain = false;
                if (s == gridScript.EndStation.position)
                {
                    win = true;
                    winText.GetComponent<GUIText>().color = new Color(winText.GetComponent<GUIText>().color.r, winText.GetComponent<GUIText>().color.b, winText.GetComponent<GUIText>().color.g, 255);
                    Debug.Log("win");
                }
            }
        }
    }

}
