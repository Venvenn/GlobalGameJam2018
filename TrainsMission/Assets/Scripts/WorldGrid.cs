using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public int[,] trainGrid;
    public int mapSize;
    public int stationNum;
    public Mesh mesh;
    private Vector3[] vertices;
    public Material material;
    public List<Station> stationList;
    public int lineNum = 5;
    public int stationsPerLine = 4;
    List<TrainLine> trainLines;
    Vector3 vectorP1;
    Vector3 vectorP2;
    // Use this for initialization
    void Start ()
    {
        stationList = new List<Station>();
        trainLines = new List<TrainLine>();
        InitGrid();
        for(int i = 0; i < lineNum; i++)
        {
            trainLines.Add(MakeLine());
        }

        vectorP1 = trainLines[0].lineStatons[0].position;
        vectorP2 = trainLines[1].lineStatons[1].position;



    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    void InitGrid()
    {
        trainGrid = new int[mapSize, mapSize];
        PlaceStation();
    }


    void PlaceStation()
    {
        Station newStation = new Station();
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
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());
            GameObject stationObject = new GameObject();
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
            Debug.Log(stationList.Count);
            trainGrid[newStation.gridX, newStation.gridY] = 1;
        }

    }

    TrainLine MakeLine()
    {
        TrainLine newLine = new TrainLine();
        //Station prevStation = new Station();
        //Debug.Log(stationList.Count);
        //for (int i = 0; i < stationsPerLine; i++)
        //{
        //    Debug.Log(stationList.Count);
        //    int max = stationList.Count;
        //    int randStationNum = Random.Range(0, max - 1);
        //    newLine.lineStatons.Add(stationList[randStationNum]);
        //    newLine.lineStatons[i].nextStation = prevStation;
        //    prevStation = newLine.lineStatons[randStationNum];
        //}
        //newLine.lineStatons[0].nextStation = newLine.lineStatons[newLine.lineStatons.Count];

        Debug.Log(stationList[0].position);

        newLine.lineStatons.Add(stationList[0]);
        newLine.lineStatons.Add(stationList[1]);
        newLine.lineStatons[1].nextStation = newLine.lineStatons[0];
        newLine.lineStatons.Add(stationList[2]);
        newLine.lineStatons[2].nextStation = newLine.lineStatons[1];
        newLine.lineStatons[0].nextStation = newLine.lineStatons[1];
        GameObject lineObject = new GameObject();
        lineObject.AddComponent<MeshFilter>();
        lineObject.AddComponent<MeshRenderer>();
        lineObject.GetComponent<MeshFilter>().mesh = mesh;
        lineObject.transform.parent = gameObject.transform;
        lineObject.transform.position = newLine.lineStatons[0].position;
        GameObject lineObject2 = new GameObject();
        lineObject2.AddComponent<MeshFilter>();
        lineObject2.AddComponent<MeshRenderer>();
        lineObject2.GetComponent<MeshFilter>().mesh = mesh;
        lineObject2.transform.parent = gameObject.transform;
        lineObject.transform.position = newLine.lineStatons[1].position;

        return newLine;
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



    }
}
