using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurveSelectionControl : MonoBehaviour
{
    public GameObject MainMenuButtonsParent;
    public GameObject MainMenuButtonPrefab;

    public GameObject CurveMenuContent;
    public GameObject CurveMenuButtonPrefab;

    public WorldStateController world;

    public CurveSelectionStateContext CurveSelectionFSM;

    public NamedCurvesState namedState;
    public ParameterCurvesState paramState;
    public ExerciseCurvesState exerciseState;

    // Start is called before the first frame update
    void Start()
    {
        namedState = new NamedCurvesState(CurveMenuContent, CurveMenuButtonPrefab, world);
        paramState = new ParameterCurvesState(CurveMenuContent, CurveMenuButtonPrefab, world);
        exerciseState = new ExerciseCurvesState(CurveMenuContent, CurveMenuButtonPrefab, world);
        CurveSelectionFSM = new CurveSelectionStateContext(namedState);

        
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
            switch(i)
            {
                default:
                case 0:
                    b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Named));
                    break;

                case 1:
                    b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Parameter));
                    break;

                case 2:
                    b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Exercises));
                    break;
            }
        }

        SwitchCurveGroup(GlobalData.CurveDisplayGroup.Named);
    }


    public void SwitchCurveGroup(GlobalData.CurveDisplayGroup cdg)
    {
        // Update current display group
        GlobalData.CurrentDisplayGroup = cdg;

        // Reset curve and point indices
        GlobalData.CurrentCurveIndex = 0;
        GlobalData.CurrentPointIndex = 0;

        

        switch(cdg)
        {
            default:
            case GlobalData.CurveDisplayGroup.Named:
                CurveSelectionFSM.State = namedState;                
                break;

            case GlobalData.CurveDisplayGroup.Parameter:
                CurveSelectionFSM.State = paramState;
                break;

            case GlobalData.CurveDisplayGroup.Exercises:
                CurveSelectionFSM.State = exerciseState;
                break;
        }

        

        CurveSelectionFSM.State.OnStateUpdate();

        

        if (world.BrowserWall is null)
        {
            Debug.Log("Browser Wall not initialized!");
        }

        List<PointDataset> cds = GlobalData.CurrentDataset;

        if(cds is null)
        {
            Debug.Log("Datasets not initialized");
        }

        //Debug.Log("idx: " + GlobalData.CurrentCurveIndex);
        //Debug.Log("cdsCount: " + cds.Count);

        PointDataset ds = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

        if(ds is null)
        {
            Debug.Log("Current dataset is null");
        }
        else
        {
            // Display html resource
            if (world.BrowserWall is { }) world.BrowserWall.OpenURL(ds.NotebookURL);
        }
    }



    //public void UpdateCurveMenuButtons()
    //{
    //    //// Clear old buttons

    //    //GameObject[] children = new GameObject[CurveMenuContent.transform.childCount];
    //    //for (int i = 0; i < CurveMenuContent.transform.childCount; i++)
    //    //{
    //    //    GameObject child = CurveMenuContent.transform.GetChild(i).gameObject;
    //    //    children[i] = child;
    //    //}

    //    //for (int i = 0; i < children.Length; i++)
    //    //{
    //    //    GameObject child = children[i];
    //    //    DestroyImmediate(child);
    //    //}


    //    //// Create buttons        
    //    //for (int i = 0; i < GlobalData.CurrentDataset.Count; i++)
    //    //{
    //    //    PointDataset pds = GlobalData.CurrentDataset[i];
    //    //    GameObject tmpButton = Instantiate(CurveMenuButtonPrefab, CurveMenuContent.transform);

    //    //    tmpButton.name = pds.Name + "Button";

    //    //    // ToDo: Set button curve icon
    //    //    RawImage img = tmpButton.GetComponentInChildren<RawImage>();
    //    //    if (pds.MenuButtonImage != null)
    //    //    {
    //    //        img.texture = pds.MenuButtonImage;
    //    //    }

    //    //    TextMeshProUGUI label = tmpButton.GetComponentInChildren<TextMeshProUGUI>();
    //    //    label.text = pds.DisplayString;

    //    //    Button b = tmpButton.GetComponent<Button>();
    //    //    b.onClick.AddListener(() => world.SwitchToSpecificDataset(pds.Name));
    //    //}
    //}


}
