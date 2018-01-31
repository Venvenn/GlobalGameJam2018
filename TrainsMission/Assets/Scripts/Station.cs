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
    WorldGrid gridScript;
    public ProceduralNameGenerator nameGenerator = new ProceduralNameGenerator();

    //Constructor
    public Station()
    {
        //initlaise objects
        gridScript = GameObject.Find("TrainGrid").GetComponent<WorldGrid>();
        stationObject = new GameObject();
        GameObject stationText = new GameObject("text");
        string seed = Time.time.ToString();
        System.Random pseudoRandom = new System.Random(seed.GetHashCode());

        //initalise name generator
        nameGenerator.Init();

        // create station object and text
        stationObject.AddComponent<MeshFilter>();
        stationObject.AddComponent<MeshRenderer>();
        stationObject.transform.parent = gridScript.gameObject.transform;
        stationText.AddComponent<MeshRenderer>();
        stationText.AddComponent<TextMesh>();
        stationText.transform.parent = stationObject.transform;

        //assign position 
        gridX = Random.Range(0, gridScript.mapSize);
        gridY = Random.Range(0, gridScript.mapSize);
        position = GridToPosition(gridX, gridY);

        //generate name
        Generate();


        //check if station has the same name or too close and if so recreate station 
        for (int j = 0; j < gridScript.stationList.Count; j++)
        {
            if (Vector3.Distance(position, gridScript.stationList[j].position) < 5)
            {
                Debug.Log("1: " + Vector3.Distance(position, gridScript.stationList[j].position));
                gridX = Random.Range(0, gridScript.mapSize);
                gridY = Random.Range(0, gridScript.mapSize);
                position = GridToPosition(gridX, gridY);
                j = 0;
                Debug.Log("2: " + Vector3.Distance(position, gridScript.stationList[j].position));
            }
            if (stationName == gridScript.stationList[j].stationName)
            {
                Generate();
                j = 0;
            }
        }

        //set station object varibles 
        stationObject.transform.position = position;
        stationObject.name = stationName;

        //set text object varibales 
        stationText.GetComponent<TextMesh>().text = stationObject.name;
        stationText.GetComponent<TextMesh>().alignment = TextAlignment.Center;
        stationText.GetComponent<TextMesh>().fontSize = 40;
        stationText.transform.localScale = new Vector3(stationText.transform.localScale.x / 10, stationText.transform.localScale.y / 10, stationText.transform.localScale.z / 10);
        stationText.transform.localPosition = new Vector3(stationText.transform.localPosition.x - 1.44f, stationText.transform.localPosition.y + 1.9f, stationText.transform.localPosition.z - 0.41f);
        stationText.transform.Rotate(0, -50, 0);
    }


    //generate a station name
    public void Generate()
    {
        int stationNameCount = nameGenerator.stationNames.Count;
        stationName = nameGenerator.stationNames[Random.Range(0, stationNameCount)];
    }

    //convert grid position to real world position
    public Vector3 GridToPosition(int x, int y)
    {
        return new Vector3(-gridScript.mapSize / 2 + x + 0.5f, 0, -gridScript.mapSize / 2 + y + 0.5f);
    }

}
