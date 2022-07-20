using System.Collections.Generic;
using Model.Enums;
using UnityEngine;
using Utility;

namespace FieldGeneration
{
    /// <summary>
    /// Creates a visual representation of the steps of the minimization algorithm of the scipy library.
    /// This visualization is created in relation to the scalar field representation (mesh) in the scene and its
    /// corresponding bounding box
    /// </summary>
    public class CreateAlgorithmPath : MonoBehaviour
    {
        public ScalarFieldManager ScalarFieldManager;
        
        public GameObject BoundingBox;
        public GameObject ArrowPrefab;

        public OptimizationAlgorithm AlgorithmPath = OptimizationAlgorithm.NELDER_MEAD ;
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
            var points = ScalarFieldManager.CurrentField.MeshPoints;
            //var cps = ScalarFieldManager.CurrentField.CriticalPoints;

            var path = new List<Vector3>();
            switch (AlgorithmPath)
            {
                case OptimizationAlgorithm.STEEPEST_DESCENT:
                    path = ScalarFieldManager.CurrentField.SteepestDescentPaths[PathIndex];
                    break;
                
                case OptimizationAlgorithm.NELDER_MEAD:
                    path = ScalarFieldManager.CurrentField.NelderMeadPaths[PathIndex];
                    break;
                
                case OptimizationAlgorithm.NEWTON:
                    path = ScalarFieldManager.CurrentField.NewtonPaths[PathIndex];
                    break;
                
                case OptimizationAlgorithm.NEWTON_DISCRETE:
                    path = ScalarFieldManager.CurrentField.NewtonDiscretePaths[PathIndex];
                    break;
                
                case OptimizationAlgorithm.NEWTON_TRUSTED:
                    path = ScalarFieldManager.CurrentField.NewtonTrustedPaths[PathIndex];
                    break;
                
                case OptimizationAlgorithm.BFGS:
                    path = ScalarFieldManager.CurrentField.BFGSPaths[PathIndex];
                    break;
            }
            
            // foreach(List<Vector3> path in ScalarFieldManager.CurrentField.NelderMeadPaths[PathIndex])
            // {
            //     Debug.Log("pathCount: " + path.Count);
            //     for (var i = 0; i < path.Count; i++)
            //     {
            //         Debug.Log(i + " - " + path[i]);
            //     }
            // }

            var meshPointList = new List<Vector3>();
            var min = ScalarFieldManager.CurrentField.MinRawValues;
            var max = ScalarFieldManager.CurrentField.MaxRawValues;
            foreach (var vec in path)
            {
                // Skip points outside of the bounds of the scalar field
                if (vec.x < min.x || vec.x > max.x ||
                    vec.y < min.y || vec.y > max.y ||
                    vec.z < min.z || vec.z > max.z)
                {
                    continue;
                }
                    
                var index = CalcUtility.NeareastNeighborIndexXY(ScalarFieldManager.CurrentField.RawPoints, vec);
                
                //var idx = ScalarFieldManager.CurrentField.RawPoints.FindIndex(p => p.x == vec.x && p.y == vec.y);
                if (index != -1)
                {
                    // var z = ScalarFieldManager.CurrentField.RawPoints[index].z;
                    // var v = new Vector3(vec.x, vec.y, z);
                    // Debug.Log("vec: " + vec + ", finalVec: " + v);
                    // meshPointList.Add(v);
                    meshPointList.Add(ScalarFieldManager.CurrentField.MeshPoints[index]);
                }
            }
            
            //var finalList = CalcUtility.MapDisplayVectors(meshPointList, BoundingBox.GetComponent<MeshRenderer>().bounds);
            
            DrawingUtility.DrawPath(meshPointList, this.transform, ArrowPrefab, bbScale);
        }
    }
    
    
}