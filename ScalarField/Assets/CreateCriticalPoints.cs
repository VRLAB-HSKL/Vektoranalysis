using System;
using System.Collections;
using System.Collections.Generic;
using Model;
using Model.InitFile;
using UnityEngine;
using Utility;

public class CreateCriticalPoints : MonoBehaviour
{
    public ScalarFieldManager ScalarFieldManager;
    
    public GameObject BoundingBox;
    
    public bool showCriticalPointsOnStartup;
        
    public void ToggleCriticalPoints()
    {
        showCriticalPointsOnStartup = !showCriticalPointsOnStartup;
            
        SetCriticalPointsActive(showCriticalPointsOnStartup);
    }

    private void SetCriticalPointsActive(bool isActive)
    {
        for (var i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(isActive);
        }
    }

    private void Start()
    {
        var bbScale = BoundingBox.transform.lossyScale;
        var points = ScalarFieldManager.CurrentField.MeshPoints;
        var cps = ScalarFieldManager.CurrentField.CriticalPoints;
        for(var i = 0; i < cps.Count; i++)
        {
            var cp = cps[i];
            var index = cp.PointIndex;
            
            // Debug.Log("it " + i + ", index: " + index + "/" + points.Count);
            
            var pos = points[index];

            switch (cp.Type)
            {
                case CriticalPointType.CRITICAL_POINT:
                    DrawingUtility.DrawSphere(pos, this.transform, Color.blue, bbScale);
                    break;
                
                case CriticalPointType.LOCAL_MINIMUM:
                    break;
                case CriticalPointType.LOCAL_MAXIMUM:
                    break;
                case CriticalPointType.SADDLE_POINT:
                    break;
            }
            
        }
        
        SetCriticalPointsActive(showCriticalPointsOnStartup);
    }
}
