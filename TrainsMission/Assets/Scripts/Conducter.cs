using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conducter : MonoBehaviour
{
    public int lineNum = 1;
    public int stationsPerLine = 4;
    WorldGrid gridScript;
    List<Vector3[]> trainLines; 
	// Use this for initialization
	void Start ()
    {
        gridScript = GameObject.Find("TrainGrid").GetComponent<WorldGrid>();
        trainLines = new List<Vector3[]>();
        for (int i = 0; i < lineNum; i++)
        {
            trainLines.Add(MakeLine());
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    Vector3[] MakeLine()
    {
        Vector3[] newLine = new Vector3[stationsPerLine];
        Debug.Log("hit");
        for (int i = 0; i < stationsPerLine; i++)
        {
            int randStationNum = Random.Range(0, gridScript.stationNum);
            Debug.Log(randStationNum);
            newLine[i] = gridScript.stationList[randStationNum].position;

        }
        return newLine;
    }

    void OnDrawGizmos()
    {
        if (trainLines != null)
        {
            for(int i = 0; i < lineNum; i++)
            {
                for (int j = 0; j < stationsPerLine-1; j++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(trainLines[i][j], trainLines[i][j + 1]);
                    Debug.Log(trainLines[i][j]);
                }
            }
        }
    }

}
