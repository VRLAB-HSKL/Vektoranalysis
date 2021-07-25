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

    // Start is called before the first frame update
    void Start()
    {
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


    public void SwitchCurveGroup(GlobalData.CurveDisplayGroup cdg)
    {
        // Update current display group
        GlobalData.CurrentDisplayGroup = cdg;

        // Reset curve and point indices
        GlobalData.currentCurveIndex = 0;
        GlobalData.CurrentPointIndex = 0;

        // Display html resource
        world.BrowserWall.OpenURL(GlobalData.CurrentDataset[GlobalData.currentCurveIndex].NotebookURL);

        UpdateCurveMenuButtons();



        world.UpdateWorldObjects();
    }



    public void UpdateCurveMenuButtons()
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
            b.onClick.AddListener(() => world.SwitchToSpecificDataset(pds.Name));
        }
    }


}
