using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NamedCurvesState : AbstractCurveSelectionState
{
    public NamedCurvesState(GameObject content, GameObject prefab, WorldStateController world) 
        : base(content, prefab, world)
    {
        
    }

    //public override void OnStateEntered()
    //{
    //    //GameObject[] children = new GameObject[CurveMenuContent.transform.childCount];

    //    //// Create buttons        
    //    //for (int i = 0; i < Dataset.Count; i++)
    //    //{
    //    //    PointDataset pds = Dataset[i];
    //    //    GameObject tmpButton = MonoBehaviour.Instantiate(CurveMenuButtonPrefab, CurveMenuContent.transform);

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
    //    //    b.onClick.AddListener(() => SwitchToSpecificDataset(pds.Name));
    //    //}
    //}

    

    public override void OnStateUpdate()
    {
        
    }
}
