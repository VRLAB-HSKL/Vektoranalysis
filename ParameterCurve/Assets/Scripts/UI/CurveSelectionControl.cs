using System;
using System.Collections.Generic;
using System.Linq;
using Controller;
using Model;
using TMPro;
using UI.States;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Control class for the curve selection menu
    /// </summary>
    public class CurveSelectionControl : MonoBehaviour
    {
        public GameObject MainMenuParent;
        public GameObject MainMenuButtonsParent;
        public GameObject MainMenuButtonPrefab;

        public GameObject CurveMenuParent;
        public GameObject CurveMenuContent;
        public GameObject CurveMenuButtonPrefab;

        public WorldStateController world;

        public CurveSelectionStateContext CurveSelectionFSM { get; set; }

        public DisplayCurvesState DisplayState;
        public ExerciseCurvesState exerciseState;

        // Start is called before the first frame update
        void Start()
        {
            DisplayState = new DisplayCurvesState(CurveMenuContent, CurveMenuButtonPrefab, world);
            exerciseState = new ExerciseCurvesState(CurveMenuContent, CurveMenuButtonPrefab, world);
            CurveSelectionFSM = new CurveSelectionStateContext(DisplayState);


            if (!GlobalData.InitFile.ApplicationSettings.SelectMenuSettings.Activated)
            {
                MainMenuParent.SetActive(false);
                CurveMenuParent.SetActive(false);
                return;
            };
        
            string[] displayGrps = Enum.GetNames(typeof(GlobalData.CurveDisplayGroup));
        
        
        
            GlobalData.CurveDisplayGroup[] displayGrpValues = (GlobalData.CurveDisplayGroup[])Enum.GetValues(typeof(GlobalData.CurveDisplayGroup));
            for (int i = 0; i < displayGrps.Length; i++)
            {
                // Get current group name
                string dgrpName = displayGrps[i];

                //Debug.Log(GlobalData.initFile.ApplicationSettings.SelectMenuSettings.ShowExercises);
            
                // Make sure group is activated
                switch (dgrpName)
                {
                    case "Display":
                        if (!GlobalData.InitFile.ApplicationSettings.SelectMenuSettings.ShowDisplayCurves ||
                            !GlobalData.DisplayCurveDatasets.Any())
                        {
                            continue;
                        }
                        break;
                    
                    // case "Parameter":
                    //     if (!GlobalData.initFile.curveSelection.paramCurves) continue;
                    //     break;
                
                    case "Exercises":
                        if (!GlobalData.InitFile.ApplicationSettings.SelectMenuSettings.ShowExercises ||
                            !GlobalData.SelectionExercises.Any())
                        {
                            continue;
                        }
                        
                        break;
                
                    default:
                        break;
                }

            
            
            
                // Create instance of button prefab
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
                        b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Display));
                        break;

                    // case 1:
                    //     //b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Parameter));
                    //     break;

                    case 1:
                        b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Exercises));
                        break;
                }
            }

            SwitchCurveGroup(GlobalData.CurveDisplayGroup.Display);
        }


        public void SwitchCurveGroup(GlobalData.CurveDisplayGroup cdg)
        {
            if (!GlobalData.InitFile.ApplicationSettings.SelectMenuSettings.Activated) return;
        
            // Update current display group
            GlobalData.CurrentDisplayGroup = cdg;

        
            switch(cdg)
            {
                default:
                case GlobalData.CurveDisplayGroup.Display:
                    if (GlobalData.InitFile.ApplicationSettings.SelectMenuSettings.ShowDisplayCurves)
                        CurveSelectionFSM.State = DisplayState;
                    break;
                //
                // case GlobalData.CurveDisplayGroup.Parameter:
                //     
                //     if(GlobalData.initFile.curveSelection.paramCurves)
                //         CurveSelectionFSM.State = paramState;
                //     break;

                case GlobalData.CurveDisplayGroup.Exercises:
                    if(GlobalData.InitFile.ApplicationSettings.SelectMenuSettings.ShowExercises)
                        CurveSelectionFSM.State = exerciseState;
                    break;
            }
        
            // Reset curve and point indices
            GlobalData.CurrentCurveIndex = 0;
            //GlobalData.CurrentPointIndex = 0;

        

            CurveSelectionFSM.State.OnStateUpdate();

        

            if (world.browserWall is null)
            {
                Debug.Log("Browser Wall not initialized!");
            }

            List<CurveInformationDataset> cds = GlobalData.CurrentDataset;

            if(cds is null)
            {
                Debug.Log("Datasets not initialized");
            }

            //Debug.Log("idx: " + GlobalData.CurrentCurveIndex);
            //Debug.Log("cdsCount: " + cds.Count);

            CurveInformationDataset ds = GlobalData.CurrentDataset[GlobalData.CurrentCurveIndex];

            if(ds is null)
            {
                Debug.Log("Current dataset is null");
            }
            else
            {
                // Display html resource
                //if (world.BrowserWall is { }) world.BrowserWall.OpenURL(ds.NotebookURL);
            }
        }


    }
}
