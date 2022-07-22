using Model.Enums;
using Model.ScriptableObjects;
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
            var pos = points[index];

            switch (cp.Type)
            {
                case CriticalPointType.CRITICAL_POINT:
                    DrawingUtility.DrawSphere(pos, transform, Color.blue, bbScale);
                    break;
                
                case CriticalPointType.LOCAL_MINIMUM:
                    DrawingUtility.DrawSphere(pos, transform, Color.black, bbScale);
                    break;
                case CriticalPointType.LOCAL_MAXIMUM:
                    DrawingUtility.DrawSphere(pos, transform, Color.white, bbScale);
                    break;
                case CriticalPointType.SADDLE_POINT:
                    DrawingUtility.DrawSphere(pos, transform, Color.gray, bbScale);
                    break;
            }
            
        }
        
        SetCriticalPointsActive(showCriticalPointsOnStartup);
    }
}
