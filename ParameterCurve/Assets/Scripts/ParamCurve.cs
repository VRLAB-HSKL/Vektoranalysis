using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
//using System.Text.Json;

using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;


using TMPro;
using UnityEngine.Events;

public class ParamCurve : MonoBehaviour
{
    public GameObject RootElement;
    public LineRenderer DisplayLR;
    //public TextAsset CSV_File;
    
    
    //public bool SwapYZCoordinates = false;

    public Transform TravelObject;
    

    public TextMeshProUGUI SourceLabel;
    public TextMeshProUGUI IndexLabel;
    public TextMeshProUGUI TLabel;
    public TextMeshProUGUI XLabel;
    public TextMeshProUGUI YLabel;
    public TextMeshProUGUI ZLabel;


    public SimpleWebBrowser.WebBrowser IngameBrowser;
    

    public bool IsDriving = true;


    public GameObject MainMenuButtonsParent;
    public GameObject MainMenuButtonPrefab;

    public GameObject CurveMenuContent;
    public GameObject CurveMenuButtonPrefab;


    [Header("TimeDistance")]
    public GameObject TimeDistanceStart;
    public GameObject TimeDistanceXAxis;
    public GameObject TimeDistanceYAxis;
    private LineRenderer TimeDistLR;


    

    //private List<PointDataset> NamedCurveDatasets = new List<PointDataset>();
    //private List<PointDataset> paramCurveDatasets = new List<PointDataset>();
    //private List<PointDataset> exerciseCurveDatasets = new List<PointDataset>();

    //private int currentCurveIndex = 0;
    //private int currentPointIndex = 0;

    

    private Vector3 InitTravelObjPos;

    private float pointStepDuration = 0f;

    private LineRenderer TangentLR;
    private Vector3[] tangentArr = new Vector3[2];

    private LineRenderer NormalLR;
    private Vector3[] normalArr = new Vector3[2];

    private LineRenderer BinormalLR;
    private Vector3[] binormalArr = new Vector3[2];

    // Start is called before the first frame update
    void Start()
    {
        pointStepDuration = 
            0f //(1f / 30f) //60f) 
            * GlobalData.RunSpeedFactor;

        InitTravelObjPos = TravelObject.position;
        if(TravelObject.childCount > 0)
        {
            GameObject firstChild = TravelObject.GetChild(0).gameObject;
            TangentLR = firstChild.GetComponent<LineRenderer>();
            TangentLR.positionCount = 2;
        }

        if (TravelObject.childCount > 1)
        {
            GameObject secondChild = TravelObject.GetChild(1).gameObject;
            NormalLR = secondChild.GetComponent<LineRenderer>();
            NormalLR.positionCount = 2;
        }

        if (TravelObject.childCount > 2)
        {
            GameObject thirdChild = TravelObject.GetChild(2).gameObject;
            BinormalLR = thirdChild.GetComponent<LineRenderer>();
            BinormalLR.positionCount = 2;
        }

        var xrenderer = TimeDistanceXAxis.GetComponent<MeshRenderer>();
        var yrenderer = TimeDistanceYAxis.GetComponent<MeshRenderer>();

        DataImport.TimeDistanceXAxisLength = xrenderer.bounds.size.x;
        DataImport.TimeDistanceYAxisLength = yrenderer.bounds.size.y;
        TimeDistLR = TimeDistanceStart.GetComponent<LineRenderer>();

        //Debug.Log("TDXAxisLength: " + DataImport.TimeDistanceXAxisLength);
        //Debug.Log("TDYAxisLength: " + DataImport.TimeDistanceYAxisLength);

        GlobalData.InitializeData();

        


        // Display html resource
        IngameBrowser.OpenCommentFile(
            GlobalData.NamedCurveDatasets[GlobalData.currentCurveIndex].NotebookURL);


        UpdateCurveMenuButtons();
        UpdateWorldObjects();


        string[] displayGrps = Enum.GetNames(typeof(GlobalData.CurveDisplayGroup));
        GlobalData.CurveDisplayGroup[] displayGrpValues = (GlobalData.CurveDisplayGroup[])Enum.GetValues(typeof(GlobalData.CurveDisplayGroup));
        for (int i = 0; i < displayGrps.Length; i++)
        {
            string dgrpName = displayGrps[i];
            GlobalData.CurveDisplayGroup dgrpVal = displayGrpValues[i];
            GameObject tmpButton = Instantiate(MainMenuButtonPrefab, MainMenuButtonsParent.transform);

            tmpButton.name = dgrpName + "GrpButton";
            Destroy(tmpButton.GetComponent<RawImage>());

            TextMeshProUGUI label = tmpButton.GetComponentInChildren<TextMeshProUGUI>();
            label.text = dgrpName;

            Button b = tmpButton.GetComponent<Button>();

            b.onClick.AddListener(() => SwitchCurveGroup(dgrpVal));

        }


        

    }

    private float updateTimer = 0f;

    private void Update()
    {
        if (IsDriving)
        {
            updateTimer += Time.deltaTime;
            if(updateTimer >= pointStepDuration)
            {
                updateTimer = 0f;
                SetTravelPointAndDisplay();                
            }
            
            
        }
            
    }

    public void UpdateLineRenderer()
    {        
        if (GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints is null) return;

        if (DisplayLR is null)
            Debug.Log("Failed to get line renderer component");

        DisplayLR.positionCount = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.Count;
        DisplayLR.SetPositions(GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.ToArray());

        TimeDistLR.positionCount = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints.Count;
        for (int i = 0; i < GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints.Count; i++)
        {
            Vector2 p = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].timeDistancePoints[i];
            Vector3 newPos = TimeDistanceStart.transform.position;
            newPos.x += p.x;
            newPos.y += p.y;
            TimeDistLR.SetPosition(i, newPos);
        }


    }

    public void StartRun()
    {
        GlobalData.currentPointIndex = 0;
        IsDriving = true;        
    }

    public void SwitchToNextDataset()
    {
        // Stop driving
        if(IsDriving)
        {
            IsDriving = false;            
        }

        // Increment data set index, reset to 0 on overflow
        ++GlobalData.currentCurveIndex;
        if (GlobalData.currentCurveIndex >= GlobalData.CurrentDataset.Count)
            GlobalData.currentCurveIndex = 0;

        // Reset point index
        GlobalData.currentPointIndex = 0;

        // Display html resource
        IngameBrowser.OpenCommentFile(
            GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateWorldObjects();
    }

    public void SwitchToPreviousDataset()
    {
        // Stop driving
        if (IsDriving)
        {
            IsDriving = false;
        }

        // Decrement data set index, reset to last element on negative index
        --GlobalData.currentCurveIndex;
        if (GlobalData.currentCurveIndex < 0)
            GlobalData.currentCurveIndex = GlobalData.CurrentDataset.Count - 1;

        // Reset point index
        GlobalData.currentPointIndex = 0;

        // Display html resource
        IngameBrowser.OpenCommentFile(
            GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateWorldObjects();
    }


    public void SwitchToSpecificDataset(string name)
    {
        // Stop driving
        if (IsDriving)
        {
            IsDriving = false;
        }

        //// Increment data set index, reset to 0 on overflow
        //++currentDataSetIndex;
        //if (currentDataSetIndex >= CurrentDataset.Count)
        //    currentDataSetIndex = 0;

        int index = GlobalData.CurrentDataset.FindIndex(x => x.Name.Equals(name));
        if (index == -1) return;

        GlobalData.currentCurveIndex = index;

        // Reset point index
        GlobalData.currentPointIndex = 0;

        // Display html resource
        IngameBrowser.OpenCommentFile(
            GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateWorldObjects();
    }

    public void SwitchCurveGroup(GlobalData.CurveDisplayGroup cdg)
    {
        // Update current display group
        GlobalData.CurrentDisplayGroup = cdg;

        // Reset curve and point indices
        GlobalData.currentCurveIndex = 0;
        GlobalData.currentPointIndex = 0;


        //Debug.Log("CurrDataSetCount: " + GlobalData.CurrentDataset.Count);
        //Debug.Log("CurrentCurveIndex: " + GlobalData.currentCurveIndex);

        // Display html resource
        IngameBrowser.OpenCommentFile(
            GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateCurveMenuButtons();
        UpdateWorldObjects();
    }
    

    private void UpdateWorldObjects()
    {
        UpdateLineRenderer();
        SetTravelPointAndDisplay();
    }

    private void SetTravelPointAndDisplay()
    {
        // Null checks
        if (TravelObject is null) return;
        if (GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints is null) return;
        if (GlobalData.currentPointIndex < 0 ) return;
        
        // On arrival at the last point, stop driving
        if (GlobalData.currentPointIndex >= GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints.Count)
        {
            IsDriving = false;
            return;
        }

        
        int pointIndex = GlobalData.currentPointIndex;

        TravelObject.position = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].worldPoints[pointIndex];
        
        tangentArr[0] = TravelObject.position;
        tangentArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].fresnetApparatuses[pointIndex].Tangent;
        TangentLR.SetPositions(tangentArr);

        normalArr[0] = TravelObject.position;
        normalArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].fresnetApparatuses[pointIndex].Normal;
        NormalLR.SetPositions(normalArr);

        binormalArr[0] = TravelObject.position;
        binormalArr[1] = TravelObject.position + GlobalData.CurrentDataset[GlobalData.currentCurveIndex].fresnetApparatuses[pointIndex].Binormal;
        BinormalLR.SetPositions(binormalArr);
        

        IndexLabel.text = (pointIndex + 1) + 
            " / " +
            GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points.Count;

        SourceLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].Name;
        TLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].paramValues[pointIndex].ToString("0.#####");
        XLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points[pointIndex].x.ToString("0.#####");
        YLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points[pointIndex].y.ToString("0.#####");
        ZLabel.text = GlobalData.CurrentDataset[GlobalData.currentCurveIndex].points[pointIndex].z.ToString("0.#####");

        ++GlobalData.currentPointIndex;

    }


    private void UpdateCurveMenuButtons()
    {
        // Clear old buttons
        //while (CurveMenuContent.transform.childCount > 0)
        //{
        //    DestroyImmediate(transform.GetChild(0).gameObject);
        //}

        GameObject[] children = new GameObject[CurveMenuContent.transform.childCount];
        for (int i = 0; i < CurveMenuContent.transform.childCount; i++)
        {
            GameObject child = CurveMenuContent.transform.GetChild(i).gameObject;
            children[i] = child;
        }

        for(int i = 0; i < children.Length; i++)
        {
            GameObject child = children[i];
            DestroyImmediate(child);
        }


        // Create buttons        
        for (int i = 0; i < GlobalData.CurrentDataset.Count; i++)
        {
            PointDataset pds = GlobalData.CurrentDataset[i];
            GameObject tmpButton = Instantiate(CurveMenuButtonPrefab, CurveMenuContent.transform);

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
            b.onClick.AddListener(() => SwitchToSpecificDataset(pds.Name));
        }
    }

    

    

    

}



[Serializable]
public class JsonRoot
{
    public string name { get; set; }

    public List<PointData> pointData { get; set; } = new List<PointData>();
}


[Serializable]
public class PointData
{
    public string t { get; set; }
    public string x { get; set; }
    public string y { get; set; }
    public List<string> tan { get; set; } = new List<string>();
    public List<string> norm { get; set; } = new List<string>();
    public List<string> binorm { get; set; } = new List<string>();
}




