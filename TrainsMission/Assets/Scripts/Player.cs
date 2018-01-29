using System.Collections;
using System.Collections.Generic;
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
    bool onTrain;
    bool win = false;

    private void Awake()
    {
        winText.SetActive(false);
    }

    // Use this for initialization
    void Start()
    {
        gridScript = GameObject.Find("TrainGrid").GetComponent<WorldGrid>();
        trainsList = new List<GameObject>();
        playerLight = transform.Find("PlayerSpotlight").GetComponent<Light>();

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

                playerLight.intensity = 2;
                playerLight.spotAngle = 80;
                transform.parent = t.transform;
                transform.position = t.transform.position + (Vector3.up/3);
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
                playerLight.intensity = 1;
                playerLight.spotAngle = 30;
                Debug.Log("hit station");
                onTrain = false;
                if (s == gridScript.EndStation.position)
                {
                    win = true;
                    winText.SetActive(win);                 
                    Debug.Log("win");
                }
            }
        }
    }

}
