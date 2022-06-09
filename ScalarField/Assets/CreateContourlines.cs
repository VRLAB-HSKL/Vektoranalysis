using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using Calculation;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class CreateContourlines : MonoBehaviour
{
    public List<float> ContourValues = new List<float>();
    public bool ShowLinesInMesh = true;
    
    public float epsilon = 0.01f;
    public Vector3 positionOffset = new Vector3(0f, 0.125f, 0f);
    public float lineThicknessMultiplier = 0.0125f;

    [Header("Gizmos")]
    public Vector3 PointScale = new Vector3(0.5f, 0.5f, 0.5f);

    public GameObject ArrowPrefab;
    public Vector3 ArrowScale = Vector3.one;

    public GameObject BoundingBox;

    private Vector3 bbExtents;
    

    private List<GameObject> ContourLineObjects = new List<GameObject>();
    private List<List<Vector3>> isolinePointLists;
    
    private Vector3 parentOrigin;
    private Vector3 ScalingVector = GlobalDataModel.DetailMeshScalingVector;

    public void ShowContourLines(bool isVisible)
    {
        foreach (var line in ContourLineObjects)
        {
            line.gameObject.SetActive(isVisible);
        }
        
        
    }

    
    
    // Start is called before the first frame update
    private void Start()
    {
        // ToDo: Import values from init file
        ContourValues = new List<float>()
        {
            -0.75f, -0.5f, -0.25f, 0f, 0.25f, 0.5f, 0.75f
        };

        parentOrigin = transform.parent.position;

        bbExtents = BoundingBox.GetComponent<MeshRenderer>().bounds.extents;
        
            
        CalculateContourLines();
        
        //ShowContourLines(false);

        for (var i = 0; i < ContourLineObjects.Count; i++)
        {
            // Draw example points on contour lines
            DrawSphereOnLine(i, 1);
            DrawArrowOnLine(i, 1, Vector3.up);
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
        isolinePointLists = new List<List<Vector3>>();

        var bb = BoundingBox.GetComponent<MeshRenderer>().bounds.extents;
        
        var x_min = GlobalDataModel.CurrentField.displayPoints.Min(v => v.x);
        var x_max = GlobalDataModel.CurrentField.displayPoints.Max(v => v.x);
        var y_min = GlobalDataModel.CurrentField.displayPoints.Min(v => v.y);
        var y_max = GlobalDataModel.CurrentField.displayPoints.Max(v => v.y);
        var z_min = GlobalDataModel.CurrentField.displayPoints.Min(v => v.z);
        var z_max = GlobalDataModel.CurrentField.displayPoints.Max(v => v.z);    
        
        
        for (var i = 0; i < ContourValues.Count; i++)
        {
            var isoValue = ContourValues[i];
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

            var hullPointList = CalcUtility.GetConvexHull(rawPointList);
            var finalPointList = new List<Vector3>();
            
            foreach (var point in hullPointList)
            {
                var index = rawPointList.IndexOf(point);
                finalPointList.Add(displayPointList[index]);
            }
            
            isolinePointLists.Add(finalPointList);
        }

        
        for (var i = 0; i < isolinePointLists.Count; i++)
        {
            var pointList = isolinePointLists[i];
            if (pointList.Count == 0) continue;
            
            var go = new GameObject("ContourLine_" + ContourValues[i]);
            go.transform.SetParent(transform);
            
            var lr = go.AddComponent<LineRenderer>();

            // Scale points for main scene mesh 
            var newPointList = new List<Vector3>();
            for (var j = 0; j < pointList.Count; j++)
            {
                var p = pointList[j];
                // Scale point to match mesh scaling
                //var newp = Vector3.Scale(p, ScalingVector);

                var x = CalcUtility.MapValueToRange(p.x, x_min, x_max, -bb.x, bb.x);
                var y = CalcUtility.MapValueToRange(p.y, y_min, y_max, -bb.y, bb.y);
                var z = CalcUtility.MapValueToRange(p.z, z_min, z_max, -bb.z, bb.z);
                
                var newp = new Vector3(x, y, z); 
                
                
                if (ShowLinesInMesh)
                {
                    newp += parentOrigin + new Vector3(0f, 0.25f, 0f); //positionOffset;
                }
                else
                {
                    // Null vertical coordinate so all contour lines are on the same xz plane
                    newp = new Vector3(newp.x, 0f, newp.z);
                    
                    // Move contour point to game object origin + custom offset
                    newp += parentOrigin + positionOffset;    
                }

                // Add changed point to list
                newPointList.Add(newp);
            }

            lr.positionCount = pointList.Count;
            lr.SetPositions(newPointList.ToArray());
            // ToDo: Set contour line color based on color map value color
            lr.material.color = Color.green;//Random.ColorHSV();
            lr.widthMultiplier = lineThicknessMultiplier; // 0.0125f;
            lr.loop = true;
            
            ContourLineObjects.Add(go);
        }
    }
}
