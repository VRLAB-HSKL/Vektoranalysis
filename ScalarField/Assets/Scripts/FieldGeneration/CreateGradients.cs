using System.Linq;
using Model.ScriptableObjects;
using UnityEngine;
using Utility;

namespace FieldGeneration
{
    /// <summary>
    /// Creates a visual representation of gradient direction vectors in specific points.
    /// This visualization is created in relation to the scalar field representation (mesh) in the scene and its
    /// corresponding bounding box
    /// </summary>
    public class CreateGradients : MonoBehaviour
    {
        [Header("Data")]
        public ScalarFieldManager scalarFieldManager;
        
        [Header("Dependencies")]
        public GameObject boundingBox;
        public GameObject arrowPrefab;

        [Header("Settings")]
        public bool showGradientsOnStartup;
        public int stepsBetweenArrows;
        
        public void ToggleGradients()
        {
            showGradientsOnStartup = !showGradientsOnStartup;
            
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
            var lastIndexBefore = scalarFieldManager.CurrentField.Gradients.Count;
            var meshVectors = scalarFieldManager.CurrentField.MeshPoints;
            for(var i = startIndex; i < lastIndexBefore; i++)
            {
                if (i % stepsBetweenArrows != 0) continue;
                
                var gradient = scalarFieldManager.CurrentField.Gradients[i];
                // flip coordinates to match display vector ordering
                gradient = new Vector3(gradient.x, gradient.z, gradient.y);
                var start = scalarFieldManager.CurrentField.MeshPoints[i];
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

                const float tolerance = 0.125f;
                var similarPointsInMesh = meshVectors.Where(p => Mathf.Abs(p.x - end.x) < tolerance)
                    .Where(p => Mathf.Abs(p.z - end.z) < tolerance).ToList();

                if (similarPointsInMesh.Any())
                {
                    //Debug.Log("Found points with approximately the same x and z coordinate");
                    var index = meshVectors.IndexOf(similarPointsInMesh[0]);
                    end = new Vector3(end.x, meshVectors[index].y, end.z);
                }
                
                DrawingUtility.DrawArrow(start, end, transform, arrowPrefab, boundingBox.transform.localScale);
            }
         
            SetGradientsActive(showGradientsOnStartup);
        }
    }
}