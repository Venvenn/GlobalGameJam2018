using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WorldGrid : MonoBehaviour {

    public int[,] trainGrid;
    public int mapSize = 50;
    public int stationNum = 10;
    public int lineNum = 5;
    public int stationsPerLine = 4;
    public int maxStationLines;
    public Mesh mesh;
    public Transform Train;
    public GameObject destinationText;
    public Transform[] stationPrefabs;
    public Transform trackObject;
    List<GameObject> tracks;
    private Vector3[] vertices;
    public Material material;
    public List<Station> stationList;
    List<BezierSpline> splineScript;
    Player Player;
    public List<List<Vector3>> trainLines;
    Color[] LineColour;
    public Station StartStation;
    public Station EndStation;
    GameObject[] minimapSphere;
    public Camera MainCamera;

    // Use this for initialization
    void Start ()
    {
        trainLines = new List<List<Vector3>>();
        stationList = new List<Station>();
        splineScript = new List<BezierSpline>();
        tracks = new List<GameObject>();
        Player = GameObject.Find("Player").GetComponent<Player>();
        minimapSphere = new GameObject[stationNum];
        maxStationLines = FindMax();

        InitGrid();
        AssignStartEnd();
        LineColour = new Color[lineNum];
        SetColour();
        PlaceLineBlocks();
        Player.Init();
    }

    //find maximum lines a station can have
	int FindMax()
    {
        int max = 0;
        max = Mathf.RoundToInt((lineNum * stationsPerLine) / stationNum);
        return max;
    }

    //initliase and generate the world
    void InitGrid()
    {
        InitTrainGrid();
        PlaceStations();
        for (int i = 0; i < lineNum; i++)
        {
            trainLines.Add(MakeLine());
            AssignSplines();
        }

        CheckUnConnected();
    }

    void InitTrainGrid()
    {
        trainGrid = new int[mapSize, mapSize];

        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                trainGrid[x, y] = 0;
            }
        }
    }

    void PlaceStations()
    {
        for (int i = 0; i < stationNum; i++)
        {
            Station newStation = new Station();
            stationList.Add(newStation);
            trainGrid[newStation.gridX, newStation.gridY] = 1;
            Instantiate(stationPrefabs[Random.Range(0, 3)], newStation.stationObject.transform.position, stationPrefabs[0].transform.rotation, newStation.stationObject.transform);
        }   
    }

    List<Vector3> MakeLine()
    {     
        List<Vector3> newLine = new List<Vector3>();
        for (int i = 0; i < stationsPerLine-1; i++)
        {
            int randStationNum = Random.Range(0, stationNum);
            if (maxStationLines > stationList[randStationNum].linesConnected)
            {
                newLine.Add(stationList[randStationNum].position);
                stationList[randStationNum].linesConnected++;
                //Debug.Log(stationList[randStationNum].position);
            }
            else
            {
                while (stationList[randStationNum].linesConnected >= maxStationLines)
                {
                    randStationNum = Random.Range(0, stationNum);
                }
                newLine.Add(stationList[randStationNum].position);
              


                stationList[randStationNum].linesConnected++;
            }


        }

        //splineLine.GetComponent<BezierSpline>().Loop = true;
        newLine.Add(newLine[0]);
        return newLine;
    }

    void AssignSplines()
    {
        GameObject splineLine = new GameObject();
        splineLine.AddComponent<BezierSpline>();
        splineLine.name = trainLines.Count.ToString();
        splineLine.transform.parent = GameObject.Find("SplineLines").transform;
        splineScript.Add(splineLine.GetComponent<BezierSpline>());
        splineScript[trainLines.Count-1].Reset();

        GameObject train = new GameObject();
        train.name = "train";
        train.transform.parent = GameObject.Find("Trains").transform;
        train.AddComponent<MeshFilter>();
        train.AddComponent<MeshRenderer>();
        train.AddComponent<TrainScript>();
        train.GetComponent<TrainScript>().line = trainLines.Count - 1;
        Instantiate(Train, new Vector3 (transform.position.x, 0.35f, transform.position.x), transform.rotation, train.transform);

        train.AddComponent<SplineWalker>();
        train.GetComponent<SplineWalker>().spline = splineScript[trainLines.Count - 1];
        train.GetComponent<SplineWalker>().duration = 30;
        train.GetComponent<SplineWalker>().lookForward = true;
        train.GetComponent<SplineWalker>().mode = SplineWalker.SplineWalkerMode.Loop;

        train.tag = "train";

        GameObject track = new GameObject();
        track.name = "track";
        track.AddComponent<SplineDecorater>();
        track.GetComponent<SplineDecorater>().frequency = 1000;
        track.GetComponent<SplineDecorater>().spline = splineScript[trainLines.Count - 1];
        track.GetComponent<SplineDecorater>().items = new Transform[1];
        track.layer = 9;


        

        tracks.Add(track);
        int pointNum = 0;

        splineScript[trainLines.Count - 1].SetControlPoint(pointNum, trainLines[trainLines.Count - 1][0]);
        for (int i = 1; i < trainLines[trainLines.Count - 1].Count; i++)
        {
            pointNum += 3;
            splineScript[trainLines.Count - 1].SetControlPoint(pointNum, trainLines[trainLines.Count - 1][i]);
            splineScript[trainLines.Count - 1].AddCurve();
        }

        splineScript[trainLines.Count - 1].Loop = true;

    }
    void CheckUnConnected()
    {
        //Vector3[] newLine = new Vector3[stationsPerLine];
        for (int i = 0; i < stationList.Count; i++)
        {
            if(stationList[i].linesConnected == 0)
            {
                trainLines.Add(GetClosestStation(stationList[i]));
                lineNum++;
            }

        }
    }

    List<Vector3> GetClosestStation(Station currentStation)
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
        //Debug.Log(newLine[0]);
        for (int i = 1; i < stationsPerLine; i++)
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
            //Debug.Log(newLine[i]);
        }
        newLine[newLine.Count-1] = newLine[0];


        return newLine;
    }

    void AssignStartEnd()
    {
        StartStation = stationList[Random.Range(0, stationList.Count)];
        Vector3 currentPos = StartStation.position;
        Station sMax = null;
        float maxDist = 0;


        foreach (Station s in stationList)
        {
            foreach (List<Vector3> l in trainLines)
            {
                if (!(l.Contains(s.stationObject.transform.position)&& l.Contains(currentPos)))
                {
                    float dist = Vector3.Distance(s.stationObject.transform.position, currentPos);
                    if (dist > maxDist)
                    {
                        sMax = s;
                        maxDist = dist;
                    }
                }
            }
        }
        Player.transform.position = currentPos + (Vector3.one / 2);
        EndStation = sMax;
        destinationText.GetComponent<Text>().text = EndStation.stationName;

    }

    void SetColour()
    {
        for(int i = 0; i < lineNum; i++)
        {
            LineColour[i] = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 1);
        }
    }

    void PlaceLineBlocks()
    {
      
        for (int i = 0; i < tracks.Count; i++)
        {
            GameObject tTransfrom = new GameObject();
            tTransfrom.AddComponent<MeshFilter>();
            tTransfrom.AddComponent<MeshRenderer>();
            tTransfrom.GetComponent<MeshFilter>().mesh = mesh;
            tTransfrom.transform.localScale = new Vector3(tTransfrom.transform.localScale.x / 10, tTransfrom.transform.localScale.y / 10, tTransfrom.transform.localScale.z / 10);
            tracks[i].layer = 9;
            tracks[i].GetComponent<SplineDecorater>().items[0] = tTransfrom.transform;
            tracks[i].GetComponent<SplineDecorater>().items[0].GetComponent<MeshRenderer>().material.color = LineColour[i];

            float emission = Mathf.PingPong(Time.time, 1.0f);
            Color baseColor = Color.yellow; //Replace this with whatever you want for your base color at emission level '1'

            Color finalColor =LineColour[i] * Mathf.LinearToGammaSpace(emission);

            tracks[i].GetComponent<SplineDecorater>().items[0].GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
        }
    }

}
