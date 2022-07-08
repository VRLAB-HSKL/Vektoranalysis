using System.Collections.Generic;
using Model;
using UnityEngine;
using Utility;

namespace FieldGeneration
{
    public class CreatePath : MonoBehaviour
    {
        public GameObject BoundingBox;
        public GameObject ArrowPrefab;
        
        public int PathIndex;
        public bool showPathOnStartup;
        
        public void TogglePath()
        {
            showPathOnStartup = !showPathOnStartup;
            
            SetPathActive(showPathOnStartup);
        }

        private void SetPathActive(bool isActive)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }

        private void Start()
        {
            var bbScale = BoundingBox.transform.lossyScale;
            var points = GlobalDataModel.CurrentField.MeshPoints;
            //var cps = GlobalDataModel.CurrentField.CriticalPoints;




            var path = GlobalDataModel.CurrentField.NelderMeadPaths[PathIndex];
            // foreach(List<Vector3> path in GlobalDataModel.CurrentField.NelderMeadPaths[PathIndex])
            // {
            //     Debug.Log("pathCount: " + path.Count);
            //     for (var i = 0; i < path.Count; i++)
            //     {
            //         Debug.Log(i + " - " + path[i]);
            //     }
            // }

            var meshPointList = new List<Vector3>();
            foreach (var vec in path)
            {
                float smallesDist = float.MaxValue;
                var index = -1;
                for(var i = 0; i < GlobalDataModel.CurrentField.RawPoints.Count; i++)
                {
                    var point = GlobalDataModel.CurrentField.RawPoints[i];
                    var dist = Mathf.Sqrt(Mathf.Pow(vec.x - point.x, 2) + Mathf.Pow(vec.y - point.y, 2));
                    if (dist < smallesDist)
                    {
                        smallesDist = dist;
                        index = i;
                    }
                }
                
                //var idx = GlobalDataModel.CurrentField.RawPoints.FindIndex(p => p.x == vec.x && p.y == vec.y);
                if(index != -1)
                    meshPointList.Add(GlobalDataModel.CurrentField.MeshPoints[index]);
            }
            
            DrawingUtility.DrawPath(meshPointList, this.transform, ArrowPrefab, bbScale);
        }
    }
}