using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.ScriptableObjects;
using UnityEngine;
using Utility;
using Vector3 = UnityEngine.Vector3;

namespace FieldGeneration
{
    /// <summary>
    /// Creates a visual representation of contour lines based on imported contour values.
    /// This visualization is created in relation to the scalar field representation (mesh) in the scene and its
    /// corresponding bounding box
    /// </summary>
    public class CreateContourLines : MonoBehaviour
    {
        [Header("Data")]
        public ScalarFieldManager scalarFieldManager;
        
        [Header("Dependencies")]
        public GameObject Field;
        public GameObject boundingBox;
        public List<float> contourValues = new List<float>();
        public Material lineMat;
    
        [Header("Settings")]
        public bool linesVisible;
        public bool showLinesVerticallyInMesh;
        public Vector3 positionOffset = new Vector3(0f, 0.125f, 0f);
        public float lineThicknessMultiplier = 0.05f;

        #region Private members
        
        private Vector3 _bbExtents;
        private readonly List<GameObject> _contourLineObjects = new List<GameObject>();
        private readonly List<GameObject> _tubeObjects = new List<GameObject>();
        private List<List<PointData>> isolineDisplayPointLists;
        private Vector3 parentOrigin;
    
        #endregion Private members
        
        /// <summary>
        /// Toggle contour line visibility in scalar field mesh
        /// </summary>
        public void ToggleContourLines()
        {
            linesVisible = !linesVisible;
            ShowContourLines(linesVisible);
        }
    
        /// <summary>
        /// Map contour lines to their vertically to match their actual position in the y dimension
        /// </summary>
        /// <param name="mapToVertical"></param>
        public void MapVerticalLinePositionsToMesh(bool mapToVertical)
        {
            for (var i = 0; i < isolineDisplayPointLists.Count; i++)
            {
                var pointList = isolineDisplayPointLists[i];
                var newPointList = new List<Vector3>();
            
                for (var j = 0; j < pointList.Count; j++)
                {
                    var p = pointList[j];
     
                    // Flip coordinates to rotate it to the vertical xz plane
                    var flippedVector = new Vector3(p.Raw.x, p.Raw.z, p.Raw.y);

                    var inMinVec = scalarFieldManager.CurrentField.MinRawValues;
                    var inMaxVec = scalarFieldManager.CurrentField.MaxRawValues;
                
                    var scaledX = CalcUtility.MapValueToRange(flippedVector.x, inMinVec.x, inMaxVec.x, 
                        -_bbExtents.x, _bbExtents.x);

                    // Use min and max z values on y coordinate to accomodate coordinate flip
                    var scaledY = CalcUtility.MapValueToRange(flippedVector.y, inMinVec.z, inMaxVec.z, 
                        -_bbExtents.y, _bbExtents.y);
                
                    // USe min and max y values on z coordinate to accomodate coordinate flip
                    var scaledZ = CalcUtility.MapValueToRange(flippedVector.z, inMinVec.y, inMaxVec.y, 
                        -_bbExtents.z, _bbExtents.z);
                
                    var scaledVector = new Vector3(scaledX, scaledY, scaledZ);
                
                    if (mapToVertical)
                    {
                        scaledVector += parentOrigin + new Vector3(0f, 0.25f, 0f);
                    }
                    else
                    {
                        // Null vertical coordinate so all contour lines are on the same xz plane
                        scaledVector = new Vector3(scaledVector.x, 0f, scaledVector.z);
                    
                        // Move contour point to game object origin + custom offset
                        scaledVector += parentOrigin + positionOffset;    
                    }

                    // Add changed point to list
                    newPointList.Add(scaledVector);
                }

            
                var lr = _contourLineObjects[i].GetComponent<LineRenderer>();
                lr.positionCount = newPointList.Count;
                lr.SetPositions(newPointList.ToArray());

                var tanPerpVectors = Enumerable.Repeat(Vector3.up, newPointList.Count).ToList();
                
                var tube = _tubeObjects[i].GetComponent<TubeMesh>();
                var isActive = tube.gameObject.activeSelf;
                if (!isActive)
                {
                    tube.gameObject.SetActive(true);
                    tube.GenerateFieldMesh(newPointList, tanPerpVectors);
                    tube.gameObject.SetActive(false);
                }
                else
                {
                    tube.GenerateFieldMesh(newPointList, tanPerpVectors);
                }
                
            }
        
        }

        /// <summary>
        /// Toggle vertical positions in contour lines
        /// </summary>
        public void ToggleLineVerticalPositions()
        {
            showLinesVerticallyInMesh = !showLinesVerticallyInMesh;
            MapVerticalLinePositionsToMesh(showLinesVerticallyInMesh);
        }
    
        /// <summary>
        /// Unity Start function
        /// ====================
        /// 
        /// This function is called before the first frame update, after Awake
        /// </summary>
        private void Start()
        {
            contourValues = scalarFieldManager.CurrentField.ContourLineValues;

            var sb = new StringBuilder();
            sb.AppendLine("Imported contour line values:");
        
            foreach (var contourVal in contourValues)
            {
                sb.AppendLine(contourVal.ToString());
            }
        
            Debug.Log(sb);
        
            parentOrigin = transform.parent.position;
            _bbExtents = boundingBox.GetComponent<MeshRenderer>().bounds.extents;
            
            CalculateContourLines();
            ShowContourLines(linesVisible);
            MapVerticalLinePositionsToMesh(showLinesVerticallyInMesh);
        }
   
        private void ShowContourLines(bool isVisible)
        {
            foreach (var line in _contourLineObjects)
            {
                line.gameObject.SetActive(isVisible);
            }

            foreach (var tube in _tubeObjects)
            {
                tube.gameObject.SetActive(isVisible);
            }
        }
        
        private void CalculateContourLines()
        {
            isolineDisplayPointLists = new List<List<PointData>>();
            
            for (var i = 0; i < contourValues.Count; i++)
            {
                var isoValue = contourValues[i];
                var importedPointList = scalarFieldManager.CurrentField.ContourLinePoints[i];
            
                // Skip convex hull algorithm and the rest on empty point list
                if (!importedPointList.Any())
                {
                    Debug.LogWarning("No points found for contour value " + isoValue, this);
                    continue;
                }
            
                // var hullPointList = CalcUtility.GetConvexHull(importedPointList);
                // var finalPointList = new List<PointData>();
                //
                // foreach (var point in hullPointList)
                // {
                //     var index = importedPointList.IndexOf(point);
                //     var pd = new PointData
                //     {
                //         Raw = importedPointList[index],
                //         Display = scalarFieldManager.CurrentField.DisplayPoints.Find(p => p == point)  
                //     };
                //     finalPointList.Add(pd);
                // }
                
                var finalPointList = new List<PointData>();

                 for(var j = 0; j < importedPointList.Count; j++)
                 {
                     var point = importedPointList[j];
                     var pd = new PointData
                     {
                         Raw = point,
                         Display = scalarFieldManager.CurrentField.DisplayPoints.Find(p => p == point)
                     };
                         
                     finalPointList.Add(pd);
                 }
                
                finalPointList.Add(finalPointList.First());
                
                isolineDisplayPointLists.Add(finalPointList);
            }
            
            for (var i = 0; i < isolineDisplayPointLists.Count; i++)
            {
                var pointList = isolineDisplayPointLists[i];
                if (pointList.Count == 0) continue;
            
                var go = new GameObject("ContourLine_" + contourValues[i]);
                go.transform.SetParent(transform);
            
                var lr = go.AddComponent<LineRenderer>();
            
                // ToDo: Set contour line color based on color map value color
                lr.material = lineMat;
                lr.widthMultiplier = lineThicknessMultiplier;
                //lr.loop = true;
            
                _contourLineObjects.Add(go);
                
                
                // TubeMesh
                var goTube = new GameObject("TubeMesh_" + contourValues[i]);
                goTube.transform.SetParent(transform);

                var tube = goTube.AddComponent<TubeMesh>();
                tube.tubeMat = lineMat;
                //tube.SetScalingFactor(0.25f);
                //tube.GenerateFieldMesh();

                _tubeObjects.Add(goTube);
            }

            MapVerticalLinePositionsToMesh(showLinesVerticallyInMesh);
        }
    }
}
