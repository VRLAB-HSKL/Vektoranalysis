using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TubeMesh : MonoBehaviour
{
    public Material TubeMat;
    
    private float radius = 0.125f;
    private int numberOfCirclePoints = 8;

    private void Start()
    {
        GenerateFieldMesh();
    }
    
    /// <summary>
    /// Creates the mesh by calculation the topology
    /// </summary>
    public void GenerateFieldMesh()
    {
        var mesh = new Mesh
        {
            name= "Param curve mesh",
        };

        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];

        var curvePoints = curve.WorldPoints; 
        var tubePoints = new List<Vector3>();

        var step = 360f / numberOfCirclePoints;
        for (var i = 0; i < curvePoints.Count; i++)
        {
            var centerPoint = curvePoints[i];
            var normal = curve.FresnetApparatuses[i].Normal;

            for (var j = 0; j < numberOfCirclePoints; j++)
            {
                var rad = (j * step) * 2f * Mathf.PI / 360;
                var x = centerPoint.x + Mathf.Cos(rad) * radius;
                // Debug.Log("rad: " + rad + ", x: " + x);
                var circlePoint = new Vector3(
                    x,
                    centerPoint.y ,
                    centerPoint.z + Mathf.Sin(rad) * radius
                );

                circlePoint *= curve.TableScalingFactor;
                
                tubePoints.Add(circlePoint);    
            }
            
        }
        
        var firstPoint = curve.WorldPoints.First();
        var firstCircle = tubePoints.GetRange(0, numberOfCirclePoints);

        var beginningLidPoints = new List<Vector3>();
        for (var i = 0; i < firstCircle.Count - 1; i++)
        {
            var tv1 = firstPoint ;
            var tv2 = firstPoint + firstCircle[i]; //(firstCircle[i] - firstPoint).normalized * radius;
            var tv3 = firstPoint + firstCircle[i + 1];//(firstCircle[i + 1] - firstPoint).normalized * radius;

            tv1 *= curve.TableScalingFactor;
            tv2 *= curve.TableScalingFactor;
            tv3 *= curve.TableScalingFactor;
            
            Debug.Log("Triangle 01: " + tv1.ToString("F8") + "\n" +
                      "Triangle 02: " + tv2.ToString("F8") + "\n" +
                      "Triangle 03: " + tv3.ToString("F8") + "\n"
            );
            
            SpawnCube(tv1, Color.red);
            SpawnCube(tv2, Color.green);
            SpawnCube(tv3, Color.blue);
            
            beginningLidPoints.Add(tv1);
            beginningLidPoints.Add(tv2);
            beginningLidPoints.Add(tv3);
            break;
        }
        
        tubePoints.InsertRange(0, beginningLidPoints);

        var lastPoint = curve.WorldPoints.Last();
        var lastCircle = tubePoints.GetRange(tubePoints.Count - numberOfCirclePoints, numberOfCirclePoints);
        var endingLidPoints = new List<Vector3>();
        for (var i = 0; i < lastCircle.Count - 1; i++)
        {
            endingLidPoints.Add(lastPoint);
            endingLidPoints.Add(lastCircle[i]);
            endingLidPoints.Add(lastCircle[i + 1]);
        }
        
        tubePoints.AddRange(lastCircle);
        
        var topology = MeshTopology.Triangles;
        var indices = new List<int>();

        // Generate topology indices based on chosen topology
        switch (topology)
        {
            default:
            case MeshTopology.Triangles:
                // indices = GenerateTriangleIndices(
                //     tubePoints, beginningLidPoints.Count
                // );
                Debug.Log("lidCount: " + beginningLidPoints.Count);
                indices = GenerateLidIndices(beginningLidPoints);
                
                // // Draw triangles twice to cover both sides
                // var backIndices = GenerateTriangleIndices(displayVertices, true);
                // displayVertices.AddRange(displayVertices);
                // indices.AddRange(backIndices);
                break;
            
        }

        mesh.vertices = beginningLidPoints.ToArray(); //tubePoints.ToArray();
        
        //Debug.Log("Number of points: " + displayVertices.Count);
        
        mesh.SetIndices(indices.ToArray(), topology, 0, true);

        //ScalarFieldManager.CurrentField.MeshPoints = displayVertices;
        

        // Calculate normals
        //var normals = CalculateNormals(displayVertices, indices, MeshTopology.Triangles);
        //mesh.normals = normals.ToArray();

        mesh.RecalculateNormals();

        var lineIndices = new List<int>();

        GetComponent<MeshRenderer>().material = TubeMat;

        // Set mesh
        GetComponent<MeshFilter>().mesh = mesh;
        
        // Assign mesh to collider
        var meshCollider = GetComponent<MeshCollider>();
        //collider.convex = true;
        meshCollider.sharedMesh = mesh;
        
        
    }

    private List<int> GenerateTriangleIndices(List<Vector3> tubePoints, int lidPointCount)
    {
        var indicesList = new List<int>();

        var firstLid = tubePoints.GetRange(0, lidPointCount);
        for (var i = 0; i < lidPointCount; i++)
        {
            indicesList.Add(i);
            indicesList.Add(i + 1);
            indicesList.Add(i + 2);
        }
        
        //var prevSet = tubePoints.GetRange(0, numberOfCirclePoints);
        for (var i = lidPointCount; i < tubePoints.Count - lidPointCount; i += numberOfCirclePoints)
        {
            // if (i >= (tubePoints.Count - numberOfCirclePoints - 2)) continue;
            
            Debug.Log("i: " + i + ", i + numCPs: " + (i+numberOfCirclePoints)
                + ", tubePointCount: " + tubePoints.Count);
            var currSet = tubePoints.GetRange(i, numberOfCirclePoints);

            for (var j = 0; j < numberOfCirclePoints; ++j)
            {
                var baseIndex = i + j;

                if (baseIndex >= (tubePoints.Count - numberOfCirclePoints - 1)) continue;
                
                Debug.Log(
                    "baseIndex: " + baseIndex +
                    ", baseIndex + 1: " + (baseIndex + 1) +
                    ", baseIndex + Num: " + (baseIndex + numberOfCirclePoints) +
                    ", baseIndex + Num + 1: " + (baseIndex + numberOfCirclePoints + 1)
                );
                
                // upper right triangle
                indicesList.Add(baseIndex);
                indicesList.Add(baseIndex + numberOfCirclePoints);
                indicesList.Add(baseIndex + numberOfCirclePoints + 1);        
                
                // lower left triangle
                indicesList.Add(baseIndex + numberOfCirclePoints + 1);
                indicesList.Add(baseIndex + 1);
                indicesList.Add(baseIndex);
            }
            
            
            
            
            
            
            for (var j = 0; j < currSet.Count; j++)
            {
                
                
                
                //     // Determine if we have to draw a lower left triangle above the current point
                //     var isUpperBound = i % sampleCount == (sampleCount - 1);
                //     var isLastColumn = i >= (sampleCount * sampleCount - sampleCount);
                //
                //     // If we are not in the last column, and not on the upper bound of the column, draw lower left triangle
                //     // above the current position
                //     var drawLowerLeftTriangle = !isLastColumn && !isUpperBound;
                //
                //     if (drawLowerLeftTriangle)
                //     {
                //         /*
                //             c
                //             -
                //             ----
                //             -------
                //             ----------
                //             -------------
                //             ----------------
                //            a (i)            b                
                //         */
                //         var a = i;
                //         var b = i + ScalarFieldManager.CurrentField.SampleCount;
                //         var c = i + 1;
                //     
                //         // Choose index order based on winding direction
                //         if (windClockWise)
                //         {
                //             indicesList.Add(a);
                //             indicesList.Add(c);
                //             indicesList.Add(b);    
                //         }
                //         else
                //         {
                //             indicesList.Add(a);
                //             indicesList.Add(b);
                //             indicesList.Add(c);
                //         }
                //     }
                //
                //     // Determine if we have to draw a upper right triangle below the current point
                //     var isLowerBound = i % ScalarFieldManager.CurrentField.SampleCount == 0;
                //     var isFirstColumn = i < ScalarFieldManager.CurrentField.SampleCount;
                //     var drawUpperRightTriangle = !isLowerBound && !isFirstColumn;
                //
                //     // If we are not in the first column, and not on the lower bound of the column, draw upper right triangle
                //     // below the current position
                //     if (drawUpperRightTriangle)
                //     {
                //         /*
                //            c                a
                //             ----------------
                //                -------------
                //                   ----------
                //                      -------
                //                         ----                     
                //                            -
                //                            b              
                //         */
                //         var a = i;
                //         var b = i - 1;
                //         var c = i - ScalarFieldManager.CurrentField.SampleCount;
                //     
                //         // Choose index order based on winding direction
                //         if (windClockWise)
                //         {    
                //             indicesList.Add(a);
                //             indicesList.Add(b);
                //             indicesList.Add(c);    
                //         }
                //         else
                //         {
                //             indicesList.Add(a);
                //             indicesList.Add(c);
                //             indicesList.Add(b);
                //         }
                //         
                //     }
                // }
            }
            //
            // prevSet = currSet;
        }

        var lowerBound = tubePoints.Count - lidPointCount;
        var upperBound = tubePoints.Count;

        var lastLid = tubePoints.GetRange(tubePoints.Count - lidPointCount, lidPointCount);
        for (var i = lowerBound; i < upperBound; i++)
        {
            indicesList.Add(i);
            indicesList.Add(i + 1);
            indicesList.Add(i + 2);
        }
        
        return indicesList;
    }

    private List<int> GenerateLidIndices(List<Vector3> lidPoints)
    {
        var indicesList = new List<int>();
        //var firstLid = tubePoints.GetRange(0, lidPointCount);

        for (var i = 0; i < lidPoints.Count; i++)
        {
            var lidPoint = lidPoints[i];
            Debug.Log(i + ": " + lidPoint.ToString("F8"));
            
        }
        
        for (var i = 0; i < lidPoints.Count; i++)
        {
            indicesList.Add(i);
            // indicesList.Add(i + 1);
            // indicesList.Add(i + 2);
        }

        return indicesList;
    }

    private void GenerateTriangleIndices()
    {
        
    }

    private void SpawnCube(Vector3 pos, Color color)
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = transform;
        cube.transform.position = pos;
        cube.transform.localScale = new Vector3(0.005f, 0.005f, 0.005f);
        cube.GetComponent<MeshRenderer>().material.color = color;    
    }
}
