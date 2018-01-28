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
    public Transform Train;
    public Transform[] Station;
    public Transform Track;
    List<GameObject> tracks;
    private Vector3[] vertices;
    public Material material;
    public List<Station> stationList;
    List<BezierSpline> splineScript;
    Player Player;
    public List<List<Vector3>> trainLines;
    List<int> stationsOnLine;
    Color[] LineColour;
    Station StartStation;
    Station EndStation;
    GameObject[] minimapSphere;







    // Use this for initialization
    void Start ()
    {
        trainLines = new List<List<Vector3>>();
        stationList = new List<Station>();
        stationsOnLine = new List<int>();
        maxStationLines = FindMax();
        StartStation = new Station();
        EndStation = new Station();
        splineScript = new List<BezierSpline>();
        tracks = new List<GameObject>();
        Player = GameObject.Find("Player").GetComponent<Player>();
        minimapSphere = new GameObject[stationNum];

        InitGrid();
        AssignStartEnd();
        LineColour = new Color[lineNum];
        SetColour();
        PlaceLineBlocks();
        Player.Init();


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
            AssignSplines();
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
                trainGrid[x, y] = 0;
            }
        }

        for (int i = 0; i < stationNum; i++)
        {
            Station newStation = new Station();
            GameObject stationObject = new GameObject("train");
            GameObject stationText = new GameObject("text");
            newStation.nameGenerator.Init();
            System.Random pseudoRandom = new System.Random(seed.GetHashCode());

            stationObject.AddComponent<MeshFilter>();
            stationObject.AddComponent<MeshRenderer>();
            stationObject.transform.parent = gameObject.transform;
            stationText.AddComponent<MeshRenderer>();
            stationText.AddComponent<TextMesh>(); ;
            stationText.transform.parent = stationObject.transform;


            newStation.gridX = Random.Range(0, mapSize);
            newStation.gridY = Random.Range(0, mapSize);
            newStation.Generate();
            Debug.Log(newStation.nameGenerator.stationNames.Count);

            newStation.position = GridToPosition(newStation.gridX, newStation.gridY);
            stationObject.transform.position = newStation.position;// + (Vector3.left*2) + Vector3.forward;
            Instantiate(Station[Random.Range(0,2)], stationObject.transform.position, Station[0].transform.rotation, stationObject.transform);
            stationObject.name = newStation.stationName;
            stationText.GetComponent<TextMesh>().text = stationObject.name;
            stationText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
            newStation.stationObject = stationObject;
            stationList.Add(newStation);
            trainGrid[newStation.gridX, newStation.gridY] = 1;

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
        track.GetComponent<SplineDecorater>().frequency = 2000;
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
        //splineScript.SetControlPoint(3, trainLines[4][1]);
        //splineScript.AddCurve();
        //splineScript.SetControlPoint(6, trainLines[4][2]);
        //splineScript.AddCurve();
        //splineScript.SetControlPoint(9, trainLines[4][3]);


        //splineLine.transform.position = trainLines[trainLines.Count-1][0];
        //splineScript.SetControlPoint(0, trainLines[trainLines.Count-1][0]);
        //for (int i = 1; i < trainLines[trainLines.Count-1].Count; i++)
        //{

        //    Debug.Log(splineScript.ControlPointCount);
        //    splineScript.SetControlPoint(splineScript.ControlPointCount - 3, trainLines[trainLines.Count-1][i]);
        //    splineScript.AddCurve();
        //}
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
        Vector3 currentPos = StartStation.stationObject.transform.position;
        Station sMax = new Station();
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
        Player.transform.position = currentPos;
        EndStation = sMax;
        Debug.Log(StartStation.position);
        Debug.Log(EndStation.position);
    }
        
    Vector3 GridToPosition(int x, int y)
    {
        return new Vector3(-mapSize / 2 + x + 0.5f, 0, -mapSize / 2 + y + 0.5f);
    }



    void OnDrawGizmos()
    {
        if (trainGrid != null)
        {
            //for (int y = 0; y < mapSize; y++)
            //{
            //    for (int x = 0; x < mapSize; x++)
            //    {
            //        if (trainGrid[x, y] == 1)
            //        {
            //            Gizmos.color = Color.red;
            //        }
            //        else
            //        {
            //            Gizmos.color = Color.black;
            //        }
            //        Gizmos.DrawCube(new Vector3(x, 3, y), Vector3.one);
            //    }
            //}
            //if (trainLines != null)
            //{
            //    Gizmos.color = Color.green;
            //    for (int l = 0; l < lineNum; l++)
            //    {
            //        Gizmos.color = LineColour[l];
            //        for (int i = 0; i < stationsPerLine - 1; i++)
            //        {

            //            //Gizmos.DrawCube(new Vector3(trainLines[l][i].x, 3, trainLines[l][i].z), Vector3.one);
            //            Gizmos.DrawLine(trainLines[l][i], trainLines[l][i + 1]);

            //        }
            //    }
            //}
        }
      

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
        for (int i = 0; i < trainLines.Count; i++)
        {

                int offset = 0;
                for (int k = 0; k < stationList.Count; k++)
                {
                    if (trainLines[i].Contains(stationList[k].position))
                    {
                    GameObject miniMapobject = new GameObject();
                    miniMapobject.AddComponent<MeshFilter>();
                    miniMapobject.AddComponent<MeshRenderer>();
                    miniMapobject.GetComponent<MeshFilter>().mesh = mesh;
                    miniMapobject.layer = 8;

                    Renderer render = miniMapobject.GetComponent<Renderer>();
                    render.material.color = LineColour[i];

                    miniMapobject.transform.position = new Vector3(stationList[k].position.x + offset - stationList[k].linesConnected, 0 , stationList[k].position.z);
                    miniMapobject.transform.localScale.Set(15, 15, 15);


                    GameObject lineBlock = new GameObject();
                    lineBlock.AddComponent<MeshFilter>();
                    lineBlock.AddComponent<MeshRenderer>();
                    lineBlock.GetComponent<MeshFilter>().mesh = mesh;
                    lineBlock.transform.localScale.Set(0.3f, 0.3f, 0.3f);
                    Renderer rend = lineBlock.GetComponent<Renderer>();
                    rend.material.color = LineColour[i];

                    lineBlock.transform.position = new Vector3(stationList[k].position.x + offset - stationList[k].linesConnected, 3, stationList[k].position.z);
                    offset++;




                }
            }
       
        }
        for (int i = 0; i < tracks.Count; i++)
        {

            GameObject tTransfrom = new GameObject();
            tTransfrom.AddComponent<MeshFilter>();
            tTransfrom.AddComponent<MeshRenderer>();
            tTransfrom.GetComponent<MeshFilter>().mesh = mesh;
            tTransfrom.transform.localScale = Track.localScale;
            tracks[i].layer = 9;
            tracks[i].GetComponent<SplineDecorater>().items[0] = tTransfrom.transform;
            tracks[i].GetComponent<SplineDecorater>().items[0].GetComponent<MeshRenderer>().material.color = LineColour[i];


        }
    }

    public void GenerateNames()
    {


    }

}
