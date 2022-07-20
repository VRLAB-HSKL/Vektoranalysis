using System;
using System.Linq;
using log4net;
using Model;
using Model.InitFile;
using UnityEngine;
using Utility;

namespace FieldGeneration
{
    public class CreateGradients : MonoBehaviour
    {
        public ScalarFieldManager ScalarFieldManager;
        
        public GameObject BoundingBox;
        public GameObject ArrowPrefab;

        public bool showGradientsOnStartup;
        public int stepsBetweenArrows;
        
        public void ToggleGradients()
        {
            showGradientsOnStartup = !showGradientsOnStartup;
            
            Debug.Log("showGrads: " + showGradientsOnStartup);
            
            SetGradientsActive(showGradientsOnStartup);
        }

        private void SetGradientsActive(bool isActive)
        {
            for (var i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(isActive);
            }
        }
        
        
        private void Start()
        {
            var startIndex = 0;
            var lastIndexBefore = ScalarFieldManager.CurrentField.Gradients.Count;
            var meshVectors = ScalarFieldManager.CurrentField.MeshPoints;
            for(var i = startIndex; i < lastIndexBefore; i++)
            {
                if (i % stepsBetweenArrows != 0) continue;
                
                var gradient = ScalarFieldManager.CurrentField.Gradients[i];
                // flip coordinates to match display vector ordering
                gradient = new Vector3(gradient.x, gradient.z, gradient.y);
                var start = ScalarFieldManager.CurrentField.MeshPoints[i];
                var end = start + gradient;

                // Debug.Log(
                //     "index: " + i + "\n" +
                //     "gradient: " + gradient + "\n" +
                //     "start: " + start + "\n" +
                //     "end: " + end + "\n" +
                //     "target: " + end
                // );

                // if (Physics.Raycast(end, Vector3.up, out RaycastHit upHit))
                // {
                //     end = new Vector3(end.x, upHit.point.y, end.z);
                // }
                // else if (Physics.Raycast(end, Vector3.down, out RaycastHit downHit))
                // {
                //     end = new Vector3(end.x, downHit.point.y, end.z);
                // }

                var tolerance = 0.125f;
                var similiarPointsinMesh = meshVectors.Where(p => Mathf.Abs(p.x - end.x) < tolerance)
                    .Where(p => Mathf.Abs(p.z - end.z) < tolerance).ToList();

                if (similiarPointsinMesh.Any())
                {
                    //Debug.Log("Found points with approximately the same x and z coordinate");
                    var index = meshVectors.IndexOf(similiarPointsinMesh[0]);
                    end = new Vector3(end.x, meshVectors[index].y, end.z);
                }
                
                DrawingUtility.DrawArrow(start, end, transform, ArrowPrefab, BoundingBox.transform.localScale);
            }
         
            SetGradientsActive(showGradientsOnStartup);
        }
    }
}