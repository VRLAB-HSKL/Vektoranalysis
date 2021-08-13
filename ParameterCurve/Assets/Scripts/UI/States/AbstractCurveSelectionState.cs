using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRKL.MBU;

public abstract class AbstractCurveSelectionState : State 
{
    private static GameObject CurveMenuContent;
    private static GameObject CurveMenuButtonPrefab;
    public static WorldStateController World;

    //public LineRenderer DisplayLR;
    //public Transform TravelObject;
    //public Transform ArcLengthTravelObject;

    //public LineRenderer TangentLR;
    //public LineRenderer NormalLR;
    //public LineRenderer BinormalLR;

    //public LineRenderer ArcLengthTangentLR;
    //public LineRenderer ArcLengthNormalLR;
    //public LineRenderer ArcLengthBinormalLR;

    //public List<PointDataset> GlobalData.CurrentDataset;

    public AbstractCurveSelectionState(
        GameObject menuContent, GameObject prefab, WorldStateController world)
    {
        CurveMenuContent = menuContent;
        CurveMenuButtonPrefab = prefab;
        World = world;
    }

    public override void OnStateEntered()
    {
        GameObject[] children = new GameObject[CurveMenuContent.transform.childCount];

        // Create buttons        
        for (int i = 0; i < GlobalData.CurrentDataset.Count; i++)
        {
            PointDataset pds = GlobalData.CurrentDataset[i];
            GameObject tmpButton = MonoBehaviour.Instantiate(CurveMenuButtonPrefab, CurveMenuContent.transform);

            tmpButton.name = pds.Name + "Button";

            // ToDo: Set button curve icon
            RawImage img = tmpButton.GetComponentInChildren<RawImage>();
            if (pds.MenuButtonImage != null)
            {
                img.texture = pds.MenuButtonImage;
            }

            TextMeshProUGUI label = tmpButton.GetComponentInChildren<TextMeshProUGUI>();
            label.text = pds.DisplayString;

            Button b = tmpButton.GetComponent<Button>();
            b.onClick.AddListener(() => World.SwitchToSpecificDataset(pds.Name));
        }
    }

    public override void OnStateQuit()
    {
        // Clear old buttons
        GameObject[] children = new GameObject[CurveMenuContent.transform.childCount];
        for (int i = 0; i < CurveMenuContent.transform.childCount; i++)
        {
            GameObject child = CurveMenuContent.transform.GetChild(i).gameObject;
            children[i] = child;
        }

        for (int i = 0; i < children.Length; i++)
        {
            GameObject child = children[i];
            Object.DestroyImmediate(child);
        }
    }

    //protected void SwitchToSpecificDataset(string name)
    //{
    //    // Stop driving
    //    if (GlobalData.IsDriving)
    //    {
    //        GlobalData.IsDriving = false;
    //    }

    //    //// Increment data set index, reset to 0 on overflow
    //    //++currentDataSetIndex;
    //    //if (currentDataSetIndex >= CurrentDataset.Count)
    //    //    currentDataSetIndex = 0;

    //    int index = GlobalData.CurrentDataset.FindIndex(x => x.Name.Equals(name));
    //    if (index == -1) return;

    //    GlobalData.CurrentCurveIndex = index;

    //    // Reset point index
    //    GlobalData.CurrentPointIndex = 0;

    //    // Display html resource
    //    BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);

    //    //UpdateWorldObjects();

    //    InfoWall.UpdatePlotLineRenderers();
    //}

    //public void UpdateWorldObjects()
    //{
    //    UpdateLineRenderers();
    //    SetTravelPointAndDisplay();
    //}

    //public void UpdateLineRenderers()
    //{
    //    //if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;

    //    if (DisplayLR is null)
    //        Debug.Log("Failed to get line renderer component");

    //    DisplayLR.positionCount = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count;
    //    DisplayLR.SetPositions(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.ToArray());

    //    InfoWall.UpdatePlotLineRenderers();

    //}

    //private void SetTravelPointAndDisplay()
    //{
    //    // Null checks
    //    if (TravelObject is null) return;
    //    if (GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints is null) return;
    //    if (GlobalData.CurrentPointIndex < 0) return;

    //    // On arrival at the last point, stop driving
    //    if (GlobalData.CurrentPointIndex >= GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count)
    //    {
    //        GlobalData.IsDriving = false;
    //        return;
    //    }


    //    int pointIndex = GlobalData.CurrentPointIndex;

    //    TravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
    //    ArcLengthTravelObject.position = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthWorldPoints[pointIndex];

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

    //    Vector3[] arcLengthTangentArr = new Vector3[2];
    //    arcLengthTangentArr[0] = ArcLengthTravelObject.position;
    //    arcLengthTangentArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Tangent;
    //    ArcLengthTangentLR.SetPositions(arcLengthTangentArr);

    //    Vector3[] arcLengthNormalArr = new Vector3[2];
    //    arcLengthNormalArr[0] = ArcLengthTravelObject.position;
    //    arcLengthNormalArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Normal;
    //    ArcLengthNormalLR.SetPositions(arcLengthNormalArr);

    //    Vector3[] arcLengthBinormalArr = new Vector3[2];
    //    arcLengthBinormalArr[0] = ArcLengthTravelObject.position;
    //    arcLengthBinormalArr[1] = ArcLengthTravelObject.position + GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].arcLengthFresnetApparatuses[pointIndex].Binormal;
    //    ArcLengthBinormalLR.SetPositions(arcLengthBinormalArr);



    //    Vector3 nextPos = Vector3.zero;
    //    if (pointIndex < GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints.Count - 1)
    //    {
    //        nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex + 1];
    //    }
    //    else
    //    {
    //        nextPos = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].worldPoints[pointIndex];
    //    }

    //    TravelObject.transform.LookAt(nextPos, (binormalArr[0] + binormalArr[1]).normalized);


    //    // ToDo: Add arc length travel object rotation ?


    //    InfoWall.UpdateInfoLabels();
    //    InfoWall.UpdatePlotTravelObjects();


    //    ++GlobalData.CurrentPointIndex;
    //}

    //public void SwitchCurveGroup(GlobalData.CurveDisplayGroup cdg)
    //{
    //    // Update current display group
    //    //GlobalData.CurrentDisplayGroup = cdg;

    //    // Reset curve and point indices
    //    GlobalData.CurrentCurveIndex = 0;
    //    GlobalData.CurrentPointIndex = 0;

    //    //GlobalData.fsm

    //    // Display html resource
    //    //world.BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex].NotebookURL);

    //    UpdateCurveMenuButtons();



    //    world.UpdateWorldObjects();
    //}

}
