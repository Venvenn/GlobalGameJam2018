﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathCreator))]
public class PathEditor : Editor
{
    PathCreator creator;
    Path Path
    {
        get
        {
            return creator.path;
        }
    }

    const float segmeantSelectDistanceThreshold = 0.1f;
    int selectedSegmentIndex = -1;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUI.BeginChangeCheck();
        if (GUILayout.Button("Create New"))
        {
            Undo.RecordObject(creator, "Create New");
            creator.CreatePath();
        }


        bool isClosed = GUILayout.Toggle(Path.IsClosed, "Closed");
        if (isClosed != Path.IsClosed)
        {
            Undo.RecordObject(creator, "Toggle Closed");
            Path.IsClosed = isClosed;
        }

        bool autoSetControlPoints = GUILayout.Toggle(Path.AutoSetControlPoints, "Auto Set Control Points");
        if(autoSetControlPoints)
        {
            Undo.RecordObject(creator, "Toggle Auto Set Controls");
            Path.AutoSetControlPoints = autoSetControlPoints;
        }
        if(EditorGUI.EndChangeCheck())
        {
            SceneView.RepaintAll();
        }
    }

    private void OnSceneGUI()
    {
        Input();
        Draw();
    }

    void Input()
    {
        UnityEngine.Event guiEvent = UnityEngine.Event.current;
        Vector2 mousePos = HandleUtility.GUIPointToWorldRay(guiEvent.mousePosition).origin;
        if (guiEvent.type == EventType.MouseDown && guiEvent.button == 0 && guiEvent.shift)
        {
            if(selectedSegmentIndex != -1)
            {
                Undo.RecordObject(creator, "Split Segment");
                Path.SplitSegment(mousePos, selectedSegmentIndex);
            }
            else if(!Path.IsClosed)
            {
                Undo.RecordObject(creator, "Add Segment");
                Path.AddSegment(mousePos);
            }

        }

        if(guiEvent.type == EventType.MouseDown && guiEvent.button == 1)
        {
            float minDstToAnchor = creator.anchorDiamertre*0.5f;
            int closestAnchorIndex = -1;
            for(int i = 0; i < Path.NumPoints; i+=3)
            {
                float dst = Vector2.Distance(mousePos, Path[i]);
                if(dst < minDstToAnchor)
                {
                    minDstToAnchor = dst;
                    closestAnchorIndex = i;
                }

            }
            if(closestAnchorIndex != -1)
            {
                Undo.RecordObject(creator, "Delete Segment");
                Path.DeleteSegment(closestAnchorIndex);

            }
        }

        if(guiEvent.type == EventType.mouseMove)
        {
            float minDstToSegment = segmeantSelectDistanceThreshold;
            int newSelectedSegmentIndex = -1;
            for (int i = 0; i < Path.NumSegments; i++)
            {
                Vector2[] points = Path.GetPointsInSegment(i);
                float dst = HandleUtility.DistancePointBezier(mousePos, points[0], points[3], points[1], points[2]);
                if (dst < minDstToSegment)
                {
                    minDstToSegment = dst;
                    newSelectedSegmentIndex = i;
                }

            }
            if(newSelectedSegmentIndex != selectedSegmentIndex)
            {
                selectedSegmentIndex = newSelectedSegmentIndex;
                SceneView.RepaintAll();
            }
        }
      

    }

    void Draw()
    {
        for (int i = 0; i < Path.NumSegments; i++)
        {
 
            Vector2[] points = Path.GetPointsInSegment(i);
            if(creator.displayControlPoints)
            {
                Handles.color = Color.black;
                Handles.DrawLine(points[1], points[0]);
                Handles.DrawLine(points[2], points[3]);
            }
            Color segmentColour = (i == selectedSegmentIndex && UnityEngine.Event.current.shift) ? creator.selectedColour : creator.segmentColour;
            Handles.DrawBezier(points[0], points[3], points[1], points[2], segmentColour, null, 2);

        }


        for(int i = 0; i < Path.NumPoints; i++)
        {
            if (i % 3 == 0 || creator.displayControlPoints)
            {
                Handles.color = (i % 3 == 0) ? creator.anchorColour : creator.controlColour;
                float handleSize = (i % 3 == 0) ? creator.anchorDiamertre : creator.controlDiametre;
                Vector2 newPos = Handles.FreeMoveHandle(Path[i], Quaternion.identity, handleSize, Vector2.zero, Handles.CylinderHandleCap);
                if (Path[i] != newPos)
                {
                    Undo.RecordObject(creator, "Move point");
                    Path.MovePoint(i, newPos);
                }
            }    
        }
    }

    void OnEnable()
    {
        creator = (PathCreator)target;
        if(creator.path ==null)
        {
            creator.CreatePath();  
        }
    }

}