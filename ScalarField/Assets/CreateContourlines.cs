using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Calculation;
using Model;
using UnityEngine;
using Color = UnityEngine.Color;
using Vector3 = UnityEngine.Vector3;

public class CreateContourlines : MonoBehaviour
{
    public List<float> ContourValues = new List<float>();

    public Material LineMat;
    
    //private bool _showLinesInMesh;

    public bool ShowLinesInMesh;
    
    
    public float epsilon = 0.01f;
    public Vector3 positionOffset = new Vector3(0f, 0.125f, 0f);
    public float lineThicknessMultiplier = 0.0125f;

    [Header("Gizmos")]
    public Vector3 PointScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject ArrowPrefab;
    public Vector3 ArrowScale = Vector3.one;

    public GameObject BoundingBox;

    private Vector3 bbExtents;
    
    private bool ShowLines = true;

    private List<GameObject> ContourLineObjects = new List<GameObject>();
    
    private List<List<PointData>> isolineDisplayPointLists;
    
    private Vector3 parentOrigin;
    private Vector3 ScalingVector = GlobalDataModel.DetailMeshScalingVector;

    public void ShowContourLines(bool isVisible)
    {
        foreach (var line in ContourLineObjects)
        {
            line.gameObject.SetActive(isVisible);
        }
    }


    private bool _linesVisible = true;
    
    public void ToggleContourLines()
    {
        _linesVisible = !_linesVisible;
        ShowContourLines(_linesVisible);
    }
    
    public void MapVerticalLinePositionsToMesh(bool mapToVertical)
    {
        // foreach (var line in ContourLineObjects)
        // {
        //     var lr = line.GetComponent<LineRenderer>();
        //     var points = new Vector3[lr.positionCount];
        //     lr.GetPositions(points);
        //
        //     var newPoints = new Vector3[lr.positionCount];
        //     foreach (var p in points)
        //     {
        //         var newP = new Vector3();
        //         if (ShowLinesInMesh)
        //         {
        //             newp += parentOrigin + new Vector3(0f, 0.25f, 0f); //positionOffset;
        //             
        //             
        //         }
        //         else
        //         {
        //             // Null vertical coordinate so all contour lines are on the same xz plane
        //             newp = new Vector3(newp.x, 0f, newp.z);
        //             
        //             // Move contour point to game object origin + custom offset
        //             newp += parentOrigin + positionOffset;    
        //         }
        //     }
        //     
        // }

        //Debug.Log("Isoline Count: " + isolinePointLists.Count + ", ContourLineObjectCount: " + ContourLineObjects.Count);

        
        
        
        
        for (var i = 0; i < isolineDisplayPointLists.Count; i++)
        {
            var pointList = isolineDisplayPointLists[i];
            var newPointList = new List<Vector3>();
            
            for (var j = 0; j < pointList.Count; j++)
            {
                var p = pointList[j];
                // Scale point to match mesh scaling
                //var newP = Vector3.Scale(p, ScalingVector);

                // var x = CalcUtility.MapValueToRange(p.x, x_min, x_max, -bb.x, bb.x);
                // var y = CalcUtility.MapValueToRange(p.y, y_min, y_max, -bb.y, bb.y);
                // var z = CalcUtility.MapValueToRange(p.z, z_min, z_max, -bb.z, bb.z);

                // Flip coordinates to rotate it to the vertical xz plane
                var flippedVector = new Vector3(p.Raw.x, p.Raw.z, p.Raw.y);

                
                // var scaledVector = CalcUtility.MapVectorToRange(flippedVector,
                //     GlobalDataModel.CurrentField.MinRawValues, GlobalDataModel.CurrentField.MaxRawValues,
                //     -bbExtents, bbExtents);

                var inMinVec = GlobalDataModel.CurrentField.MinRawValues;
                var inMaxVec = GlobalDataModel.CurrentField.MaxRawValues;
                
                var scaledX = CalcUtility.MapValueToRange(flippedVector.x, inMinVec.x, inMaxVec.x, -bbExtents.x, bbExtents.x);

                // Use min and max z values to accomodate coordinate flip
                var scaledY = CalcUtility.MapValueToRange(flippedVector.y, inMinVec.z, inMaxVec.z, -bbExtents.y, bbExtents.y);
                
                // USe min and max y values to accomodate coordinate flip
                var scaledZ = CalcUtility.MapValueToRange(flippedVector.z, inMinVec.y, inMaxVec.y, -bbExtents.z, bbExtents.z);
                
                
                // Debug.Log("initPoint: " + p + ", scaledPoint: " + newP + " with value mapping from " +
                //           "range: " + GlobalDataModel.CurrentField.MinRawValues + " | " 
                //           + GlobalDataModel.CurrentField.MaxRawValues + " to " + -bbExtents + " | " + bbExtents);

                var scaledVector = new Vector3(scaledX, scaledY, scaledZ);
                
                if (mapToVertical)
                {
                    scaledVector += parentOrigin + new Vector3(0f, 0.25f, 0f); //positionOffset;
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

            
            var lr = ContourLineObjects[i].GetComponent<LineRenderer>();
            lr.positionCount = newPointList.Count;
            lr.SetPositions(newPointList.ToArray());
        }
        
    }


    public void ToggleLineVerticalPositions()
    {
        ShowLinesInMesh = !ShowLinesInMesh;
        MapVerticalLinePositionsToMesh(ShowLinesInMesh);
        
        // Update underlying material of mesh to make it transparent when showing lines inside the mesh
        // mat = TextureUtility.ChangeMaterialRenderMode(mat,
        //     _showLinesInMesh ? TextureUtility.BlendMode.Transparent : TextureUtility.BlendMode.Opaque);
        
        var attenuation = ShowLinesInMesh ? 0.25f : 1f;
        SetMeshTransparency(attenuation);
        
    }

    private void SetMeshTransparency(float attenuation)
    {
        var mr = GameObject.Find("SimpleField").GetComponent<MeshRenderer>();
        var mat = mr.material;
        mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, attenuation);
        mr.material = mat;
    }
    
    
    // Start is called before the first frame update
    private void Start()
    {
        var dataClassesCount = float.Parse(GlobalDataModel.InitFile.color_map_data_classes_count);
        //var numberOfDividingLines = dataClassesCount - 1;
        var minZ = GlobalDataModel.CurrentField.MinRawValues.z;
        var maxZ = GlobalDataModel.CurrentField.MaxRawValues.z;
        var step = Mathf.Abs(maxZ - minZ); 
        step /= dataClassesCount;

        //Debug.Log("Min: " + minZ + ", Max: " + maxZ + ", step: " + step);
        
        ContourValues = new List<float>();
        
        // Draw line for all steps except the first and the last (min and max value has no dividing line)
        for (var i = 1; i < dataClassesCount; i++)
        {
            var value = GlobalDataModel.CurrentField.MinRawValues.z + i * step;
            //Debug.Log("Contour value " + i + " : " + value);
            ContourValues.Add(value);
        }
        
        parentOrigin = transform.parent.position;
        bbExtents = BoundingBox.GetComponent<MeshRenderer>().bounds.extents;
            
        CalculateContourLines();
        ShowContourLines(_linesVisible);

        MapVerticalLinePositionsToMesh(ShowLinesInMesh);
        var attenuation = ShowLinesInMesh ? 0.25f : 1f;
        SetMeshTransparency(attenuation);

        var bbLocalScale = BoundingBox.transform.localScale;
        for (var i = 0; i < ContourLineObjects.Count; i++)
        {
            // Draw example points on contour lines
            var line = ContourLineObjects[i];
            DrawingUtility.DrawSphereOnLine(line, 1, bbLocalScale);
            DrawingUtility.DrawArrowOnLine(line, 1, ArrowPrefab, Vector3.up, bbLocalScale);
        }

        var scalingFactor = 10f;
        var yOffset = 15f;
        
        var pathPoints = new List<Vector3>
        {
            Vector3.zero * scalingFactor + new Vector3(0f, yOffset, 0f),
            Vector3.forward * scalingFactor + new Vector3(0f, yOffset, 0f),
            Vector3.left * scalingFactor + new Vector3(0f, yOffset, 0f),
            Vector3.back * scalingFactor + new Vector3(0f, yOffset, 0f),
            Vector3.right * scalingFactor + new Vector3(0f, yOffset, 0f)
        };
        
        // for (var i = 0; i < isolinePointLists.Count; i++)
        // {
        //     var list = isolinePointLists[i];
        //     if (list.Count == 0) continue;
        //
        //     var point = list.First();
        //     //Debug.Log("iso line " + i + " point: " + point);
        //     
        //     pathPoints.Add(point);
        // }
        
        DrawingUtility.DrawPath(pathPoints, transform, ArrowPrefab, BoundingBox.transform.localScale);
    }

    
    
    
    
    private void CalculateContourLines()
    {
        isolineDisplayPointLists = new List<List<PointData>>();

        //var bb = BoundingBox.GetComponent<MeshRenderer>().bounds.extents;
        
        // var x_min = GlobalDataModel.CurrentField.displayPoints.Min(v => v.x);
        // var x_max = GlobalDataModel.CurrentField.displayPoints.Max(v => v.x);
        // var y_min = GlobalDataModel.CurrentField.displayPoints.Min(v => v.y);
        // var y_max = GlobalDataModel.CurrentField.displayPoints.Max(v => v.y);
        // var z_min = GlobalDataModel.CurrentField.displayPoints.Min(v => v.z);
        // var z_max = GlobalDataModel.CurrentField.displayPoints.Max(v => v.z);    
        //;
        
        for (var i = 0; i < ContourValues.Count; i++)
        {
            var isoValue = ContourValues[i];
            //Debug.Log("contour value: " + isoValue);
            
            var rawPointList = new List<Vector3>();
            var displayPointList = new List<Vector3>();
            for(var j = 0; j < GlobalDataModel.CurrentField.rawPoints.Count; ++j)
            {
                var point = GlobalDataModel.CurrentField.rawPoints[j];
                
                if (Mathf.Abs(point.z - isoValue) <= epsilon)
                {
                    // Debug.Log("Isoline hit !\n" +
                    //           "iso value: " + isoValue + "\n" +
                    //           "point z: " + point.z + "\n" + 
                    //           "display point: " + GlobalDataModel.CurrentField.displayPoints[j]
                    //      s     );
                    rawPointList.Add(point);
                    displayPointList.Add(GlobalDataModel.CurrentField.displayPoints[j]);
                }
            }
            //
            // Debug.Log("contour value: " + isoValue + ", found points: " + displayPointList.Count);

            // Skip convex hull algorithm and the rest on empty point list
            if (!displayPointList.Any())
            {
                Debug.LogWarning("No points found for contour value " + isoValue, this);
                continue;
            }
            
            var hullPointList = CalcUtility.GetConvexHull(rawPointList);
            var finalPointList = new List<PointData>();
            
            foreach (var point in hullPointList)
            {
                var index = rawPointList.IndexOf(point);
                var pd = new PointData()
                {
                    Raw = rawPointList[index],
                    Display = displayPointList[index]
                };
                finalPointList.Add(pd);
            }
            
            isolineDisplayPointLists.Add(finalPointList);
        }
        
        //Debug.Log("isolines: " + isolinePointLists.Count + ", contourObjects: " + ContourLineObjects.Count);
        
        for (var i = 0; i < isolineDisplayPointLists.Count; i++)
        {
            var pointList = isolineDisplayPointLists[i];
            //Debug.Log("pointList " + i + ": " + pointList.Count);
            if (pointList.Count == 0) continue;
            
            var go = new GameObject("ContourLine_" + ContourValues[i]);
            go.transform.SetParent(transform);
            
            var lr = go.AddComponent<LineRenderer>();
            
            // ToDo: Set contour line color based on color map value color
            lr.material = LineMat; //.color = new Color(1f, 255);//Random.ColorHSV();
            lr.widthMultiplier = lineThicknessMultiplier; // 0.0125f;
            lr.loop = true;
            
            ContourLineObjects.Add(go);
        
            // // Scale points for main scene mesh 
            // var newPointList = new List<Vector3>();
            // for (var j = 0; j < pointList.Count; j++)
            // {
            //     var p = pointList[j];
            //     // Scale point to match mesh scaling
            //     //var newp = Vector3.Scale(p, ScalingVector);
            //
            //     // var x = CalcUtility.MapValueToRange(p.x, x_min, x_max, -bb.x, bb.x);
            //     // var y = CalcUtility.MapValueToRange(p.y, y_min, y_max, -bb.y, bb.y);
            //     // var z = CalcUtility.MapValueToRange(p.z, z_min, z_max, -bb.z, bb.z);
            //
            //     var newp = CalcUtility.MapVectorToRange(p, GlobalDataModel.CurrentField.MinValues,
            //         GlobalDataModel.CurrentField.MaxValues, -bb, bb);
            //     
            //     //new Vector3(x, y, z); 
            //     
            //     
            //     if (ShowLinesInMesh)
            //     {
            //         newp += parentOrigin + new Vector3(0f, 0.25f, 0f); //positionOffset;
            //     }
            //     else
            //     {
            //         // Null vertical coordinate so all contour lines are on the same xz plane
            //         newp = new Vector3(newp.x, 0f, newp.z);
            //         
            //         // Move contour point to game object origin + custom offset
            //         newp += parentOrigin + positionOffset;    
            //     }
            //
            //     // Add changed point to list
            //     newPointList.Add(newp);
            // }

            //lr.positionCount = pointList.Count;
            //lr.SetPositions(newPointList.ToArray());
            
        }
        
        
        
        MapVerticalLinePositionsToMesh(ShowLinesInMesh);
    }
}
