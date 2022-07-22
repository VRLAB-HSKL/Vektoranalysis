using System.Collections.Generic;
using Model.Enums;
using Model.ScriptableObjects;
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
        [Header("Data")]
        [SerializeField]
        public ScalarFieldManager scalarFieldManager;
        
        [Header("Path Elements")]
        [SerializeField]
        public GameObject boundingBox;
        
        [SerializeField]
        public GameObject arrowPrefab;

        [Header("Settings")]
        [SerializeField]
        public OptimizationAlgorithm minimizationStrategy = OptimizationAlgorithm.NelderMead ;
        
        [SerializeField]
        public int pathIndex;
        
        [SerializeField]
        public bool showPathOnStartup;
        
        /// <summary>
        /// Toggle path visibility in field
        /// </summary>
        public void TogglePath()
        {
            showPathOnStartup = !showPathOnStartup;
            
            SetPathActive(showPathOnStartup);
        }

        /// <summary>
        /// De-/activates the algorithm path and its components
        /// </summary>
        /// <param name="setActive"></param>
        private void SetPathActive(bool setActive)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(setActive);
            }
        }

        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after Awake
        /// </summary>
        private void Start()
        {
            // Get imported path based on chosen strategy
            var path = new List<Vector3>();
            switch (minimizationStrategy)
            {
                case OptimizationAlgorithm.SteepestDescent:
                    path = scalarFieldManager.CurrentField.SteepestDescentPaths[pathIndex];
                    break;
                
                case OptimizationAlgorithm.NelderMead:
                    path = scalarFieldManager.CurrentField.NelderMeadPaths[pathIndex];
                    break;
                
                case OptimizationAlgorithm.Newton:
                    path = scalarFieldManager.CurrentField.NewtonPaths[pathIndex];
                    break;
                
                case OptimizationAlgorithm.NewtonDiscrete:
                    path = scalarFieldManager.CurrentField.NewtonDiscretePaths[pathIndex];
                    break;
                
                case OptimizationAlgorithm.NewtonTrusted:
                    path = scalarFieldManager.CurrentField.NewtonTrustedPaths[pathIndex];
                    break;
                
                case OptimizationAlgorithm.Bfgs:
                    path = scalarFieldManager.CurrentField.BFGSPaths[pathIndex];
                    break;
            }
            
            var meshPointList = new List<Vector3>();
            var min = scalarFieldManager.CurrentField.MinRawValues;
            var max = scalarFieldManager.CurrentField.MaxRawValues;
            foreach (var vec in path)
            {
                // Skip points outside of the bounds of the scalar field (observable universe)
                if (vec.x < min.x || vec.x > max.x ||
                    vec.y < min.y || vec.y > max.y ||
                    vec.z < min.z || vec.z > max.z)
                {
                    continue;
                }
                
                // Add point to path if nearest XY neighbour was found in raw point collection
                var index = CalcUtility.NearestNeighborIndexXY(scalarFieldManager.CurrentField.RawPoints, vec);
                if (index != -1)
                {
                    meshPointList.Add(scalarFieldManager.CurrentField.MeshPoints[index]);
                }
            }
            
            // Create path
            var bbScale = boundingBox.transform.lossyScale;
            DrawingUtility.DrawPath(meshPointList, transform, arrowPrefab, bbScale);
            
            SetPathActive(showPathOnStartup);
        }
    }
    
    
}