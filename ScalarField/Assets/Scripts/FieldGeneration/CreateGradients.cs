using System.Collections;
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
            StartCoroutine(CreateGradientsRoutine());
        }


        private IEnumerator CreateGradientsRoutine()
        {
            var meshVectors = scalarFieldManager.CurrentField.MeshPoints;
            var yieldStep = Mathf.FloorToInt(scalarFieldManager.CurrentField.Gradients.Count * 0.001f);

            for(var i = 0; i < scalarFieldManager.CurrentField.Gradients.Count; i++)
            {
                var gradient = scalarFieldManager.CurrentField.Gradients[i];
                // flip coordinates to match display vector ordering
                var gradientDirection = new Vector3(gradient.Direction.x, gradient.Direction.z, gradient.Direction.y);
                var start = scalarFieldManager.CurrentField.MeshPoints[gradient.Index];
                var end = start + gradientDirection.normalized;

                const float tolerance = 0.125f;
                var similarPointsInMesh = meshVectors.Where(p => Mathf.Abs(p.x - end.x) < tolerance)
                    .Where(p => Mathf.Abs(p.z - end.z) < tolerance).ToList();
                
                if (similarPointsInMesh.Any())
                {
                    var index = meshVectors.IndexOf(similarPointsInMesh[0]);
                    end = new Vector3(end.x, meshVectors[index].y, end.z);
                }
                
                var arrow = 
                    DrawingUtility.DrawArrow(start, end, transform, arrowPrefab, boundingBox.transform.localScale);
                arrow.SetActive(showGradientsOnStartup);
                
                if(i % yieldStep == 0) yield return null;
            }
        }
    }
}