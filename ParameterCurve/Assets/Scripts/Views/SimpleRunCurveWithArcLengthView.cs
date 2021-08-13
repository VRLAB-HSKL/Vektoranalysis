using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRunCurveWithArcLength : SimpleRunCurveView
{
    public Transform ArcLengthTravelObject;

    public LineRenderer ArcLengthTangentLR;
    public LineRenderer ArcLengthNormalLR;
    public LineRenderer ArcLengthBinormalLR;

    private Vector3 initArcLenghtTravelObjPos;

    public SimpleRunCurveWithArcLength(
        LineRenderer displayLR, 
        Transform travelObject, 
        Transform arcLengthTravelObject) : base(displayLR, travelObject)
    {
        ArcLengthTravelObject = arcLengthTravelObject;

        // Setup arc length travel object
        initArcLenghtTravelObjPos = ArcLengthTravelObject.position;
        if (ArcLengthTravelObject.childCount > 0)
        {
            GameObject firstChild = ArcLengthTravelObject.GetChild(0).gameObject;
            ArcLengthTangentLR = firstChild.GetComponent<LineRenderer>();
            ArcLengthTangentLR.positionCount = 2;
        }

        if (ArcLengthTravelObject.childCount > 1)
        {
            GameObject secondChild = ArcLengthTravelObject.GetChild(1).gameObject;
            ArcLengthNormalLR = secondChild.GetComponent<LineRenderer>();
            ArcLengthNormalLR.positionCount = 2;
        }

        if (ArcLengthTravelObject.childCount > 2)
        {
            GameObject thirdChild = ArcLengthTravelObject.GetChild(2).gameObject;
            ArcLengthBinormalLR = thirdChild.GetComponent<LineRenderer>();
            ArcLengthBinormalLR.positionCount = 2;
        }
    }

    public override void UpdateView()
    {
        base.UpdateView();
        UpdateArcLengthTravelObject();
    }

    private void UpdateArcLengthTravelObject()
    {
        int pointIndex = GlobalData.CurrentCurveIndex;
        ArcLengthTravelObject.position = 
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];
    }
}
