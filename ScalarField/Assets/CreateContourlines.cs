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
    
    //private List<float> ContourValues = new List<float>();

    private List<GameObject> ContourLineObjects = new List<GameObject>();
    
    private List<List<Vector3>> isolinePointArrays;
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
            DrawSphereOnLine(i, 1);
            DrawArrowOnLine(i, 1, Vector3.up);    
        }
        
    }

    
    private void DrawSphereOnLine(int lineIndex, int pointIndex)
    {
        var obj = ContourLineObjects[lineIndex];
        var lr = obj.GetComponent<LineRenderer>();
        var linePoints = new Vector3[lr.positionCount];
        lr.GetPositions(linePoints);

        var point = linePoints[pointIndex];

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        sphere.name = "Sphere_" + lineIndex + "_" + pointIndex;
        sphere.transform.parent = obj.transform;

        // Scale sphere to about 5% the size of the bounding box
        var bbScale = BoundingBox.transform.localScale;
        var maxScaleFactor = Mathf.Max(bbScale.x, bbScale.y, bbScale.z);
        var maxVector = new Vector3(maxScaleFactor, maxScaleFactor, maxScaleFactor);
        // float divX = 1f / maxScale;
        // float divY = 1f / maxScale;
        // float divZ = 1f / maxScale;
        // var divScale = new Vector3(divX, divY, divZ);
        var newScale = maxVector * 0.025f; // * 1f; //0.05f;
        
        sphere.transform.localScale = Vector3.Scale(Vector3.one, newScale);
                                
        // Debug.Log("bbScale: " + bbScale + ", maxScale: " + maxVector +  
        //           // ", divValues: " + divX + " " + divY + " " + divZ +
        //           // ", divScale: " + divScale.x + " " + divScale.y + " " + divScale.z +
        //          ", newScale: " + newScale.x + " " + newScale.y + " " + newScale.z);
        
        // var sphereVertices = new List<Vector3>();
        // sphere.GetComponent<MeshFilter>().mesh.GetVertices(sphereVertices);
        //
        // var newVertices = new List<Vector3>();
        //
        // var x_min = sphereVertices.Min(v => v.x);
        // var x_max = sphereVertices.Max(v => v.x);
        // var y_min = sphereVertices.Min(v => v.y);
        // var y_max = sphereVertices.Max(v => v.y);
        // var z_min = sphereVertices.Min(v => v.z);
        // var z_max = sphereVertices.Max(v => v.z);     
        //
        // for (var i = 0; i < sphereVertices.Count; ++i)
        // {
        //     var nv = CalcUtility.MapVectorToRange(sphereVertices[i],
        //         new Vector3(x_min, y_min, z_min), new Vector3(x_max, y_max, z_max),
        //         -bbExtents, bbExtents
        //     );
        //     
        //     newVertices.Add(nv);
        // }
        //
        // sphere.GetComponent<MeshFilter>().mesh.SetVertices(newVertices);
        
        sphere.GetComponent<MeshRenderer>().material.color = Color.red;

        sphere.transform.position = point;
    }

    private void DrawArrowOnLine(int lineIndex, int pointIndex, Vector3 direction)
    {
        var obj = ContourLineObjects[lineIndex];
        var lr = obj.GetComponent<LineRenderer>();
        var linePoints = new Vector3[lr.positionCount];
        lr.GetPositions(linePoints);

        var point = linePoints[pointIndex];

        var arrow = Instantiate(ArrowPrefab, obj.transform);
        arrow.transform.position = point;
        //arrow.GetComponent<ArrowController>().PointTowards(direction);

        // Scale arrow to about 10% the size of the bounding box
        var bbScale = BoundingBox.transform.localScale;
        var maxScale = Mathf.Max(bbScale.x, bbScale.y, bbScale.z);
        // var newScale = new Vector3(1f / maxScale, 1f / maxScale, 1f / maxScale);
        var maxVector = new Vector3(maxScale, maxScale, maxScale);
        var newScale = maxVector * 0.05f;
        arrow.transform.localScale = Vector3.Scale(Vector3.one, newScale); //newScale;
        
        
        var lookPoint = point + direction;
        
        //Debug.Log("Point: " + point + ", LookPoint: " + lookPoint);
        arrow.transform.LookAt(lookPoint);
    }

    
    
    private void CalculateContourLines()
    {
        isolinePointArrays = new List<List<Vector3>>();

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

            //displayPointList = CalcUtility.GetConvexHull(displayPointList);

            var orderedList = rawPointList.OrderBy(p => p.x).ToList();

            var hullPointList = CalcUtility.GetConvexHull(orderedList);
            var finalPointList = new List<Vector3>();
            
            foreach (var point in hullPointList)
            {
                var index = rawPointList.IndexOf(point);
                finalPointList.Add(displayPointList[index]);
            }
            
            isolinePointArrays.Add(finalPointList);
        }

        
        for (var i = 0; i < isolinePointArrays.Count; i++)
        {
            var pointList = isolinePointArrays[i];
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
