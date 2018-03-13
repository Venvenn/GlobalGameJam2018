using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathCreator : MonoBehaviour
{
    [HideInInspector]
    public Path path;

    public Color anchorColour = Color.red;
    public Color controlColour = Color.white;
    public Color segmentColour = Color.green;
    public Color selectedColour = Color.yellow;
    public float anchorDiamertre = 0.1f;
    public float controlDiametre = 0.075f;
    public bool displayControlPoints = true;

    public void CreatePath()
    {
        path = new Path(transform.position);
    }

    void Reset()
    {
        CreatePath(); 
    }

}
