using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Curve
{
    [SerializeField]
    private AnimationCurve curve;

    [SerializeField]
    private float inputMin;

    [SerializeField]
    private float inputMax;

    [SerializeField]
    private float outputMin;

    [SerializeField]
    private float outputMax;

    public float Value(float inputValue)
    {
        return MapOutput(curve.Evaluate(MapInput(inputValue)));
    }

    public float MapInput(float rawInput)
    {
        return Map(rawInput, inputMin, inputMax, 0f, 1f);
    }

    public float MapOutput(float rawOutput)
    {
        return Map(rawOutput, 0f, 1f, outputMin, outputMax);
    }

    private float Map(float value, float s1, float e1, float s2, float e2)
    {
        float clamped = Mathf.Clamp(value, s1, e1);

        float offset = s2;

        float ratio = (e2 - s2) / (e1 - s1);

        return ratio * (clamped - s1) + offset;
    }
}