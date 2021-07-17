using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimaconCurveCalc : AbstractCurveCalc
{
    public float C = 1f;

    public LimaconCurveCalc()
    {
        Name = "Limacon";
        ParameterIntervall = new List<float>(linspace(-Mathf.PI, Mathf.PI, 200));
        Is3DCurve = false;
    }

    protected override Vector3 CalculatePoint(float t)
    {
        float phi = t;
        float r = 1f + C * Mathf.Sin(t);

        Tuple<float, float> absTuple = PolarUtil.PolarHelper(r, phi);
        Tuple<float, float> cartesianTuple = PolarUtil.Polar2Cartesian(absTuple.Item1, absTuple.Item2);
        float x = cartesianTuple.Item1;
        float y = cartesianTuple.Item2;

        return new Vector3(x, y, 0f);
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        float phi = 1f;
        float r = C * Mathf.Cos(t);

        Tuple<float, float> absTuple = PolarUtil.PolarHelper(r, phi);
        Tuple<float, float> cartesianTuple = PolarUtil.Polar2CartesianFirstDerivative(absTuple.Item1, absTuple.Item2);
        float x = cartesianTuple.Item1;
        float y = cartesianTuple.Item2;

        return new Vector3(x, y, 0f).normalized;
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        float phi = 1f;
        float r = -C * Mathf.Sin(t);

        Tuple<float, float> absTuple = PolarUtil.PolarHelper(r, phi);
        Tuple<float, float> cartesianTuple = PolarUtil.Polar2Cartesian(absTuple.Item1, absTuple.Item2);
        float x = cartesianTuple.Item1;
        float y = cartesianTuple.Item2;

        return new Vector3(x, y, 0f).normalized;
    }
}
