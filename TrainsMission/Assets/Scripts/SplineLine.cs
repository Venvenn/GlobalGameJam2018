using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineLine : MonoBehaviour {
    public BezierSpline spline;

    public int frequency;

    public bool lookForward;

    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
        lineRenderer.widthMultiplier = 0.2f;
        lineRenderer.positionCount = frequency;
        float alpha = 1.0f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.blue, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
            );
        lineRenderer.colorGradient = gradient;

        if (frequency <= 0)
        {
            return;
        }
        float stepSize = frequency * 100;
        if (spline.Loop || stepSize == 1)
        {
            stepSize = 1f / stepSize;
        }
        else
        {
            stepSize = 1f / (stepSize - 1);
        }
        for (int p = 1; p < frequency; p++)
        {
                Debug.Log(lineRenderer.positionCount);
                Debug.Log(p);
                lineRenderer.SetPosition(p, spline.GetPoint(p * stepSize));
                lineRenderer.SetPosition(p-1, spline.GetPoint(p-1 * stepSize));

        }
    }
}
