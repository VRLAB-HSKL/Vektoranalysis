using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractCurveView
{
    public LineRenderer DisplayLR;
    //public Transform TravelObject;
    //public Transform ArcLengthTravelObject;

    public bool HasTravelPoint;
    public bool HasArcLengthPoint;
    
    public AbstractCurveView(LineRenderer displayLR)
    {
        DisplayLR = displayLR;
        //UpdateView();
    }

    public virtual void UpdateView()
    {
        if(DisplayLR is null)
        {
            Debug.Log("Faild to get line renderer component");
        }
        
        PointDataset curve = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];
        DisplayLR.positionCount = curve.worldPoints.Count;
        DisplayLR.SetPositions(curve.worldPoints.ToArray());
    }

    private void UpdateWorldObjects()
    {
        //UpdateLineRenderers();
        SetTravelPointAndDisplay();
    }

    //public void UpdateLineRenderers()
    //{
    //    //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;

    //    if (DisplayLR is null)
    //        Debug.Log("Failed to get line renderer component");

    //    DisplayLR.positionCount = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count;
    //    DisplayLR.SetPositions(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.ToArray());

    //    //InfoWall.UpdatePlotLineRenderers();
    //}

    private void SetTravelPointAndDisplay()
    {
        //// Null checks
        //if (TravelObject is null) return;
        //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
        //if (GlobalData.CurrentPointIndex < 0) return;

        //// On arrival at the last point, stop driving
        //if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
        //{
        //    GlobalData.IsDriving = false;
        //    return;
        //}


        //int pointIndex = GlobalData.CurrentPointIndex;

        //TravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
        //ArcLengthTravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];

        

        //Vector3[] arcLengthTangentArr = new Vector3[2];
        //arcLengthTangentArr[0] = ArcLengthTravelObject.position;
        //arcLengthTangentArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Tangent;
        //ArcLengthTangentLR.SetPositions(arcLengthTangentArr);

        //Vector3[] arcLengthNormalArr = new Vector3[2];
        //arcLengthNormalArr[0] = ArcLengthTravelObject.position;
        //arcLengthNormalArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Normal;
        //ArcLengthNormalLR.SetPositions(arcLengthNormalArr);

        //Vector3[] arcLengthBinormalArr = new Vector3[2];
        //arcLengthBinormalArr[0] = ArcLengthTravelObject.position;
        //arcLengthBinormalArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Binormal;
        //ArcLengthBinormalLR.SetPositions(arcLengthBinormalArr);



        


        // ToDo: Add arc length travel object rotation ?


        //InfoWall.UpdateInfoLabels();
        //InfoWall.UpdatePlotTravelObjects();


        ++GlobalData.CurrentPointIndex;
    }


    //protected void SetMovingFrame()
    //{
    //    Vector3[] tangentArr = new Vector3[2];
    //    tangentArr[0] = TravelObject.position;
    //    tangentArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[pointIndex].Tangent;
    //    TangentLR.SetPositions(tangentArr);

    //    Vector3[] normalArr = new Vector3[2];
    //    normalArr[0] = TravelObject.position;
    //    normalArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[pointIndex].Normal;
    //    NormalLR.SetPositions(normalArr);

    //    Vector3[] binormalArr = new Vector3[2];
    //    binormalArr[0] = TravelObject.position;
    //    binormalArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].fresnetApparatuses[pointIndex].Binormal;
    //    BinormalLR.SetPositions(binormalArr);
    //}
}
