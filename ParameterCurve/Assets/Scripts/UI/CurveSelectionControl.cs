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


            if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.Activated)
            {
                MainMenuParent.SetActive(false);
                CurveMenuParent.SetActive(false);
                return;
            };
        
            string[] displayGrps = Enum.GetNames(typeof(GlobalDataModel.CurveDisplayGroup));
        
        
        
            GlobalDataModel.CurveDisplayGroup[] displayGrpValues = (GlobalDataModel.CurveDisplayGroup[])Enum.GetValues(typeof(GlobalDataModel.CurveDisplayGroup));
            for (int i = 0; i < displayGrps.Length; i++)
            {
                // Get current group name
                string dgrpName = displayGrps[i];

                //Debug.Log(GlobalData.initFile.ApplicationSettings.SelectMenuSettings.ShowExercises);
            
                // Make sure group is activated
                switch (dgrpName)
                {
                    case "Display":
                        if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowDisplayCurves ||
                            !GlobalDataModel.DisplayCurveDatasets.Any())
                        {
                            continue;
                        }
                        break;
                    
                    // case "Parameter":
                    //     if (!GlobalData.initFile.curveSelection.paramCurves) continue;
                    //     break;
                
                    case "Exercises":
                        if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowExercises ||
                            !GlobalDataModel.SelectionExercises.Any())
                        {
                            continue;
                        }
                        
                        break;
                
                    default:
                        break;
                }

            
            
            
                // Create instance of button prefab
                GlobalDataModel.CurveDisplayGroup dgrpVal = displayGrpValues[i];
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
                        b.onClick.AddListener(() => SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Display));
                        break;

                    // case 1:
                    //     //b.onClick.AddListener(() => SwitchCurveGroup(GlobalData.CurveDisplayGroup.Parameter));
                    //     break;

                    case 1:
                        b.onClick.AddListener(() => SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Exercises));
                        break;
                }
            }

            SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup.Display);
        }


        public void SwitchCurveGroup(GlobalDataModel.CurveDisplayGroup cdg)
        {
            if (!GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.Activated) return;
        
            // Update current display group
            GlobalDataModel.CurrentDisplayGroup = cdg;

        
            switch(cdg)
            {
                default:
                case GlobalDataModel.CurveDisplayGroup.Display:
                    if (GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowDisplayCurves)
                        CurveSelectionFSM.State = DisplayState;
                    break;
                //
                // case GlobalData.CurveDisplayGroup.Parameter:
                //     
                //     if(GlobalData.initFile.curveSelection.paramCurves)
                //         CurveSelectionFSM.State = paramState;
                //     break;

                case GlobalDataModel.CurveDisplayGroup.Exercises:
                    if(GlobalDataModel.InitFile.ApplicationSettings.SelectMenuSettings.ShowExercises)
                        CurveSelectionFSM.State = exerciseState;
                    break;
            }
        
            // Reset curve and point indices
            GlobalDataModel.CurrentCurveIndex = 0;
            //GlobalData.CurrentPointIndex = 0;

        

            CurveSelectionFSM.State.OnStateUpdate();

        

            if (world.browserWall is null)
            {
                Debug.Log("Browser Wall not initialized!");
            }

            List<CurveInformationDataset> cds = GlobalDataModel.CurrentDataset;

            if(cds is null)
            {
                Debug.Log("Datasets not initialized");
            }

            //Debug.Log("idx: " + GlobalData.CurrentCurveIndex);
            //Debug.Log("cdsCount: " + cds.Count);

            CurveInformationDataset ds = GlobalDataModel.CurrentDataset[GlobalDataModel.CurrentCurveIndex];

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
