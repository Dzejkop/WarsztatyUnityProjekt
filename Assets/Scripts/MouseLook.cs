using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class MouseLook
{
    public string xAxisName;
    public string yAxisName;

    public float xSensitivity;
    public float ySensitivity;

    [Range(0.0f, 90.0f)]
    public float inclinationClampMax = 90f;

    [Range(-90.0f, 0.0f)]
    public float inclinationClampMin = -90f;

    public Vector2 GetInput()
    {
        float x = Input.GetAxis(xAxisName) * xSensitivity;
        float y = -Input.GetAxis(yAxisName) * ySensitivity;

        return new Vector2(x, y);
    }

    public Quaternion Clamp(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, inclinationClampMin, inclinationClampMax);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }
}

