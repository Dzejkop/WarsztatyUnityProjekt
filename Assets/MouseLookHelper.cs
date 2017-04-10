using System;
using UnityEngine;

[Serializable]
public class MouseLookHelper
{
    public string xAxisName;
    public string yAxisName;

    public float xAxisSensitivity;
    public float yAxisSensitivity;

    [Range(0f, 90f)]
    public float inclinationMax;

    [Range(-90f, 0f)]
    public float inclinationMin;

    public Vector2 ProcessInput(Vector2 i) {
        return new Vector2(
            i.x * xAxisSensitivity,
            i.y * yAxisSensitivity
        );
    }    

    public Quaternion Clamp(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, inclinationMin, inclinationMax);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }

}