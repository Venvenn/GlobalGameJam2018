using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public int[,] trainGrid;
    public int mapSize = 50;
    public int stationNum = 10;
    public int lineNum = 5;
    public int stationsPerLine = 4;
    public int maxStationLines;
    public Mesh mesh;
    private Vector3[] vertices;
    public Material material;
    public List<Station> stationList;
    List<Vector3[]> trainLines;
    List<int> stationsOnLine;

    Station StartStation;
    Station EndStation;


    // Use this for initialization
    void Start ()
    { 
        trainLines = new List<Vector3[]>();
        stationList = new List<Station>();
        stationsOnLine = new List<int>();
        maxStationLines = FindMax();
        StartStation = new Station();
        EndStation = new Station();

        InitGrid();
        AssignStartEnd();

    }
	int FindMax()
    {
        int max = 0;
        max = Mathf.RoundToInt((lineNum * stationsPerLine) / stationNum);
        return max;
    }
	// Update is called once per frame
	void Update ()
    {

    }

    void InitGrid()
    {
        trainGrid = new int[mapSize, mapSize];
        PlaceStation();
        for (int i = 0; i < lineNum; i++)
        {
            trainLines.Add(MakeLine());
        }
        CheckUnConnected();
  

    }


    void PlaceStation()
    {
        string seed = Time.time.ToString();


        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                //GameObject darknessObject = new GameObject();
                //darknessObject.AddComponent<MeshFilter>();
                //darknessObject.AddComponent<MeshRenderer>();
                //darknessObject.GetComponent<MeshFilter>().mesh = mesh;
                //darknessObject.GetComponent<MeshRenderer>().material = material;
                //darknessObject.transform.parent = gameObject.transform;
                //darknessObject.name = x + " " + y;
                //darknessObject.transform.position = GridToPosition(x, y);
                trainGrid[x, y] = 0;
            }
        }

        for (int i = 0; i < stationNum; i++)
        {
            Station newStation = new Station();
            GameObject stationObject = new GameObject();
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            stationObject.AddComponent<MeshFilter>();
            stationObject.AddComponent<MeshRenderer>();
            stationObject.GetComponent<MeshFilter>().mesh = mesh;
            stationObject.transform.parent = gameObject.transform;

            newStation.gridX = Random.Range(0, mapSize);
            newStation.gridY = Random.Range(0, mapSize);

            newStation.position = GridToPosition(newStation.gridX, newStation.gridY);
            stationObject.transform.position = newStation.position;
            newStation.stationObject = stationObject;
            stationList.Add(newStation);
            trainGrid[newStation.gridX, newStation.gridY] = 1;

        }


    }
    Vector3[] MakeLine()
    {
        Vector3[] newLine = new Vector3[stationsPerLine];
        for (int i = 0; i < stationsPerLine-1; i++)
        {
            int randStationNum = Random.Range(0, stationNum);
            if (maxStationLines > stationList[randStationNum].linesConnected)
            {
                newLine[i] = stationList[randStationNum].position;
                stationList[randStationNum].linesConnected++;
                //Debug.Log(stationList[randStationNum].position);
            }
            else
            {
                while (stationList[randStationNum].linesConnected >= maxStationLines)
                {
                    randStationNum = Random.Range(0, stationNum);
                }
                newLine[i] = stationList[randStationNum].position;
                stationList[randStationNum].linesConnected++;
            }


        }
        newLine[stationsPerLine-1] = newLine[0];
        return newLine;
    }

    void CheckUnConnected()
    {
        Vector3[] newLine = new Vector3[stationsPerLine];
        for (int i = 0; i < stationList.Count; i++)
        {
            if(stationList[i].linesConnected == 0)
            {
                trainLines.Add(GetClosestStation(stationList[i]));
                lineNum++;
            }

        }
    }

    Vector3[] GetClosestStation(Station currentStation)
    {
        List<Vector3> newLine = new List<Vector3>();
        Station[] sMin = new Station[stationsPerLine];
        float[] minDist = new float[stationsPerLine];

        for (int i = 0; i < stationsPerLine; i++)
        {
            minDist[i] = Mathf.Infinity;
        }
        Vector3 currentPos = currentStation.stationObject.transform.position;
        newLine.Add(currentPos);
        Debug.Log(newLine[0]);
        for (int i = 1; i < stationsPerLine-1; i++)
        {
            foreach (Station s in stationList)
            {
                float dist = Vector3.Distance(s.stationObject.transform.position, currentPos);
                if (dist < minDist[i] && (!newLine.Contains(s.stationObject.transform.position)))
                {
                    sMin[i] = s;
                    minDist[i] = dist;
                }
            }
            newLine.Add(sMin[i].stationObject.transform.position);
            Debug.Log(newLine[i]);
        }



        return newLine.ToArray();
    }

    void AssignStartEnd()
    {
        StartStation = stationList[Random.Range(0, stationList.Count)];
        
    }
        
    Vector3 GridToPosition(int x, int y)
    {
        return new Vector3(-mapSize / 2 + x - (mapSize + 1) + 0.5f, 0, -mapSize / 2 + y + 0.5f);
    }



    void OnDrawGizmos()
    {
        if (trainGrid != null)
        {
            for (int y = 0; y < mapSize; y++)
            {
                for (int x = 0; x < mapSize; x++)
                {
                    if (trainGrid[x, y] == 1)
                    {
                        Gizmos.color = Color.red;
                    }
                    else
                    {
                        Gizmos.color = Color.black;
                    }
                    Gizmos.DrawCube(new Vector3(x, 3, y), Vector3.one);
                }
            }
        }
        Gizmos.color = Color.green;
        for (int l = 0; l < lineNum; l++)
        {
            for (int i = 0; i < stationsPerLine-1; i++)
            {
                Gizmos.DrawCube(new Vector3(trainLines[l][i].x, 3, trainLines[l][i].z), Vector3.one);
                Gizmos.DrawLine(trainLines[l][i], trainLines[l][i + 1]);

            }
        }


    }
}
