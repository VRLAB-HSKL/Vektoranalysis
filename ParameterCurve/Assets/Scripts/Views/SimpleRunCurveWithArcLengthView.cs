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

        HasTravelPoint = true;
        HasArcLengthPoint = true;

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
        if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints.Count)
        {
            //Debug.Log("Stop");
            GlobalData.IsDriving = false;
            return;
        }

        int pointIndex = GlobalData.CurrentCurveIndex;
        ArcLengthTravelObject.position =
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];

        Vector3 nextPos = Vector3.zero;
        if (pointIndex < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints.Count - 1)
        {
            nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex + 1];
        }
        else
        {
            nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];
        }

        Vector3[] arcTangentArr = new Vector3[2];
        arcTangentArr[0] = ArcLengthTravelObject.position;
        arcTangentArr[1] = ArcLengthTravelObject.position +
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[GlobalData.CurrentPointIndex].Tangent;
        ArcLengthTangentLR.SetPositions(arcTangentArr);

        Vector3[] arcNormalArr = new Vector3[2];
        arcNormalArr[0] = ArcLengthTravelObject.position;
        arcNormalArr[1] = ArcLengthTravelObject.position +
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[GlobalData.CurrentPointIndex].Normal;
        ArcLengthNormalLR.SetPositions(arcNormalArr);

        Vector3[] arcBinormalArr = new Vector3[2];
        arcBinormalArr[0] = ArcLengthTravelObject.position;
        arcBinormalArr[1] = ArcLengthTravelObject.position +
            GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[GlobalData.CurrentPointIndex].Binormal;
        ArcLengthBinormalLR.SetPositions(arcBinormalArr);


        ArcLengthTravelObject.transform.LookAt(nextPos, (arcBinormalArr[0] + arcBinormalArr[1]).normalized);
    }
}
