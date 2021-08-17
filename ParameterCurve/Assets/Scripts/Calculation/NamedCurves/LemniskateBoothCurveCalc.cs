using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LemniskateBoothCurveCalc : AbstractCurveCalc
{
    // ToDo: Implement this class
    public LemniskateBoothCurveCalc()
    {
        Name = "LemniskateBooth";
    }

    protected override Vector3 CalculatePoint(float t)
    {
        throw new System.NotImplementedException();
    }

    protected override Vector3 CalculateVelocityPoint(float t)
    {
        throw new System.NotImplementedException();
    }

    protected override Vector3 CalculateAccelerationPoint(float t)
    {
        throw new System.NotImplementedException();
    }

    public override List<float> CalculateArcLengthParamRange()
    {
        return new List<float>(
            linspace(0f,
            CalculateRawDistance(CalculatePoints()),
            ParameterIntervall.Count));
    }

    public override List<Vector3> CalculateArcLengthParameterizedPoints()
    {
        List<Vector3> retList = new List<Vector3>();
        for (int i = 0; i < NumOfSamples; i++)
        {
            retList.Add(Vector3.up);
        }
        return retList;
    }

}
