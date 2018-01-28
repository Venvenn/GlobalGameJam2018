using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station
{
    public GameObject stationObject;
    public Vector3 position;
    public Station nextStation;
    public int gridX;
    public int gridY;
    public int linesConnected;
    public string stationName;
    public ProceduralNameGenerator nameGenerator = new ProceduralNameGenerator();

    // Use this for initialization
    void Start ()
    {
        nameGenerator.Init();
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public void Generate()
    {
        int stationNameCount = nameGenerator.stationNames.Count;
        stationName = nameGenerator.stationNames[Random.Range(0, stationNameCount)];
    }
}
