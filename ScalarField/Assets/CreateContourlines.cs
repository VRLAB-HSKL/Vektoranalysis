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
using Vector3 = UnityEngine.Vector3;

public class CreateContourlines : MonoBehaviour
{
    public bool ShowLinesInMesh = true;
    public float epsilon = 0.01f;
    public Vector3 positionOffset = new Vector3(0f, 0.125f, 0f);
    public float lineThicknessMultiplier = 0.0125f;
    
    private List<float> ContourValues = new List<float>();

    private List<List<Vector3>> isolinePointArrays;
    private Vector3 parentOrigin;
    private Vector3 ScalingVector = GlobalDataModel.DetailMeshScalingVector;
    
    
    // Start is called before the first frame update
    void Start()
    {
        // ToDo: Import values from init file
        ContourValues = new List<float>()
        {
            -0.75f, -0.5f, -0.25f, 0f, 0.25f, 0.5f, 0.75f
        };

        parentOrigin = transform.parent.position;
        
        CalculateContourLines();
    }

    private void CalculateContourLines()
    {
        isolinePointArrays = new List<List<Vector3>>();
        
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
            
            var go = new GameObject("IsoLine_" + ContourValues[i]);
            go.transform.SetParent(transform);
            
            var lr = go.AddComponent<LineRenderer>();

            // ToDo: Make this dynamic based on loaded scene
            // Scale points for main scene mesh 
            var newPointList = new List<Vector3>();
            for (var j = 0; j < pointList.Count; j++)
            {
                var p = pointList[j];
                // Scale point to match mesh scaling
                var newp = Vector3.Scale(p, ScalingVector);

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
            lr.material.color = Color.green;
            lr.widthMultiplier = lineThicknessMultiplier; // 0.0125f;
            lr.loop = true;
        }
    }
}
