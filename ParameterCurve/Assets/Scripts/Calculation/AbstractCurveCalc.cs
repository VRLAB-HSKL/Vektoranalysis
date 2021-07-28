using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractCurveCalc
{
    /// <summary>
    /// Name of the curve
    /// </summary>
    public string Name;

    public string DisplayString
    {
        get
        {
            return Name;
        }
    }

    /// <summary>
    /// Collection of parameter values, usually as a range
    /// </summary>
    public List<float> ParameterIntervall;

    /// <summary>
    /// Signals wether this curve is 3D oder 2D, i.e. has a non-null z value
    /// </summary>
    public bool Is3DCurve;

    /// <summary>
    /// Function object to calculate a single curve point based on a float parameter
    /// </summary>
    protected Func<float, Vector3> PointCalcFunc;

    /// <summary>
    /// Function object to calculate a sinlge velocity point based on a float parameter
    /// </summary>
    protected Func<float, Vector3> VelocityCalcFunc;

    /// <summary>
    /// Function object to calculate a single acceleration point based on a float parameter
    /// </summary>
    protected Func<float, Vector3> AccelerationCalcFunc;


    public AbstractCurveCalc()
    {
        PointCalcFunc = CalculatePoint;
        VelocityCalcFunc = CalculateVelocityPoint;
        AccelerationCalcFunc = CalculateAccelerationPoint;
    }

    /// <summary>
    /// Calculate the curve as a collection of points, based on parameter intervall set in constructor
    /// </summary>
    /// <returns>Points collection</returns>
    public List<Vector3> CalculatePoints()
    {
        return CalculateAllPointsIntoList(PointCalcFunc);
    }

    public List<Vector3> CalculateVelocity()
    {
        return CalculateAllPointsIntoList(VelocityCalcFunc);
    }

    public List<Vector3> CalculateAcceleration()
    {
        return CalculateAllPointsIntoList(AccelerationCalcFunc);
    }

    public List<FresnetSerretApparatus> CalculateFresnetApparatuses()
    {
        List<FresnetSerretApparatus> fresnetList = new List<FresnetSerretApparatus>();
        List<Vector3> velocityList = CalculateVelocity();
        List<Vector3> accelerationList = CalculateAcceleration();
        for (int i = 0; i < ParameterIntervall.Count; i++)
        {
            FresnetSerretApparatus fsa = new FresnetSerretApparatus();
            Vector3 velVec = velocityList[i];
            Vector3 accVec = accelerationList[i];
            fsa.Tangent = velVec;
            fsa.Normal = accVec;
            fsa.Binormal = Vector3.Cross(velVec, accVec);
            fresnetList.Add(fsa);
        }

        return fresnetList;
    }

    public List<Vector2> CalculateTimeDistancePoints()
    {
        List<Vector2> tdPoints = new List<Vector2>();
        var curvePoints = CalculatePoints();
        int numSteps = ParameterIntervall.Count;

        float maxDistance = CalculateRawDistance(curvePoints);
        float currentDistance = 0f;

        for(int i = 0; i < numSteps; i++)
        {
            float x = i / (float)numSteps;  //ParameterIntervall[i];


            float y;
            if (i == 0)
            {
                y = 0f;
            }
            else
            {
                currentDistance += Vector3.Distance(curvePoints[i], curvePoints[i - 1]);
                y = currentDistance;
                y /= maxDistance;
            }

            tdPoints.Add(new Vector2(x, y));
        }

        return tdPoints;
    }

    public List<Vector2> CalculateTimeVelocityPoints()
    {
        List<Vector2> tvPoints = new List<Vector2>();
        var curvePoints = CalculatePoints();
        int numSteps = ParameterIntervall.Count;

        //float maxDistance =  CalculateRawDistance(curvePoints);
        //float currentDistance = 0f;
        float maxVelocity = 0f;

        for (int i = 0; i < numSteps; i++)
        {
            float x = i / (float)numSteps;

            float y;
            if(i == 0)
            {
                y = 0f;
            }
            else
            {
                float distance = Vector3.Distance(curvePoints[i], curvePoints[i - 1]);
                //Debug.Log("VecDist: " + y);
                //currentDistance += d;
                y = distance;

                if(y > maxVelocity)
                {
                    maxVelocity = y;
                }
                //y /= maxDistance;
                //Debug.Log("ScaledVecDist: " + y);
            }

            tvPoints.Add(new Vector2(x, y));
        }

        foreach(Vector2 v in tvPoints)
        {
            v.Set(v.x, v.y / maxVelocity);
        }
        

        return tvPoints;
    }

    public static float CalculateRawDistance(List<Vector3> pointList)
    {
        if (pointList.Count < 2) return 0f;

        float distance = 0f;
        for(int i = 1; i < pointList.Count; i++)
        {
            distance += Mathf.Abs(Vector3.Distance(pointList[i - 1], pointList[i]));
        }

        return distance;
    }




    /// <summary>
    /// Source: https://stackoverflow.com/questions/17046293/is-there-a-linspace-like-method-in-math-net/67131017#67131017 
    /// </summary>
    /// <param name="startval">start of range</param>
    /// <param name="endval">end of range</param>
    /// <param name="steps">step count</param>
    /// <returns></returns>
    public static float[] linspace(float startval, float endval, int steps)
    {
        float interval = (endval / Mathf.Abs(endval)) * Mathf.Abs(endval - startval) / (steps - 1);
        return (from val in Enumerable.Range(0, steps)
                select startval + (val * interval)).ToArray();
    }




    protected abstract Vector3 CalculatePoint(float t);
    protected abstract Vector3 CalculateVelocityPoint(float t);
    protected abstract Vector3 CalculateAccelerationPoint(float t);

    //protected abstract float CalculateArcLength();

    protected List<Vector3> CalculateAllPointsIntoList(Func<float, Vector3> f)
    {
        List<Vector3> pointList = new List<Vector3>();
        for (int i = 0; i < ParameterIntervall.Count; i++)
        {
            float t = ParameterIntervall[i];
            Vector3 vec = f(t);
            pointList.Add(vec);
        }
        return pointList;
    }

}


