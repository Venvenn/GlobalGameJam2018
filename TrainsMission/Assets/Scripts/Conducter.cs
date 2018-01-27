using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conducter : MonoBehaviour
{
    public int lineNum = 1;
    public int stationsPerLine = 2;
    WorldGrid gridScript;
    List<TrainLine> trainLines; 
	// Use this for initialization
	void Start ()
    {
        gridScript = GameObject.Find("TrainGrid").GetComponent<WorldGrid>();
        trainLines = new List<TrainLine>();
        trainLines.Add(MakeLine());

    }

    // Update is called once per frame
    void Update()
    {

    }

    TrainLine MakeLine()
    {
        TrainLine newLine = new TrainLine();
        Station prevStation = new Station();
        Debug.Log("hit");
        for (int i = 0; i < stationsPerLine; i++)
        {
            int randStationNum = Random.Range(0, gridScript.stationList.Count);
            newLine.lineStatons.Add(gridScript.stationList[randStationNum]);
            newLine.lineStatons[i].nextStation = prevStation;
            prevStation = newLine.lineStatons[randStationNum];
        }
        newLine.lineStatons[0].nextStation = newLine.lineStatons[newLine.lineStatons.Count];
        return newLine;
    }

    void OnDrawGizmos()
    {
        if (trainLines != null)
        {
            for(int i = 0; i < lineNum; i++)
            {
                for (int j = 0; j < stationsPerLine; j++)
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawLine(trainLines[i].lineStatons[j].position, trainLines[i].lineStatons[j].nextStation.position);
                }
            }
        }
    }

}
