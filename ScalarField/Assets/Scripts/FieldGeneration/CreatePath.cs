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
            var min = GlobalDataModel.CurrentField.MinRawValues;
            var max = GlobalDataModel.CurrentField.MaxRawValues;
            foreach (var vec in path)
            {
                // Skip points outside of the bounds of the scalar field
                if (vec.x < min.x || vec.x > max.x ||
                    vec.y < min.y || vec.y > max.y ||
                    vec.z < min.z || vec.z > max.z)
                {
                    continue;
                }
                    
                var index = CalcUtility.NeareastNeighborIndexXY(GlobalDataModel.CurrentField.RawPoints, vec);
                
                //var idx = GlobalDataModel.CurrentField.RawPoints.FindIndex(p => p.x == vec.x && p.y == vec.y);
                if (index != -1)
                {
                    // var z = GlobalDataModel.CurrentField.RawPoints[index].z;
                    // var v = new Vector3(vec.x, vec.y, z);
                    // Debug.Log("vec: " + vec + ", finalVec: " + v);
                    // meshPointList.Add(v);
                    meshPointList.Add(GlobalDataModel.CurrentField.MeshPoints[index]);
                }
            }
            
            //var finalList = CalcUtility.MapDisplayVectors(meshPointList, BoundingBox.GetComponent<MeshRenderer>().bounds);
            
            DrawingUtility.DrawPath(meshPointList, this.transform, ArrowPrefab, bbScale);
        }
    }
}