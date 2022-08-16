using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(
    typeof(MeshFilter),
    typeof(MeshRenderer),
    typeof(MeshCollider))]
public class TubeMesh : MonoBehaviour
{
    public Material TubeMat;

    public float ScalingFactor = 1f;
    
    private float radius = 0.1f;
    private int numberOfCirclePoints = 8;

    private Mesh tubeMesh;
    private Mesh bottomLidMesh;
    private Mesh topLidMesh;
    
    private List<Vector3> tubePoints = new List<Vector3>();
    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;

    private GameObject _bottomLidGameObject;
    private GameObject _topLidGameObject;

    private float _degreeStepSize;

    private void Awake()
    {
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        //GenerateFieldMesh();
    }

    /// <summary>
    /// Creates the mesh by calculation the topology
    /// </summary>
    public void GenerateFieldMesh()
    {
        Create();
    }
    
    protected void Create()
    {
        tubePoints.Clear();
        _degreeStepSize = 360f / numberOfCirclePoints;
        
        GenerateCurveMesh();
        GenerateBottomLidMesh();
        GenerateTopLidMesh();
    }
    
    private void GenerateCurveMesh()
    {
        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var curvePoints = curve.WorldPoints; 
        
        for (var i = 0; i < curvePoints.Count; i++)
        {
            var centerPoint = curvePoints[i];
            var normal = curve.FresnetApparatuses[i].Normal;
            var tangent = curve.FresnetApparatuses[i].Tangent;

            //  Calculate 3d point of normal target and change vector so the generated circle
            //  matches the given radius
            var cpn = normal * radius; //(centerPoint + normal).normalized * radius;
            
            // On first point, generate tangent pointing to next point since
            // we don't have any velocity at the beginning, i.e. the tangent is (0,0,0)
            if (i == 0)
                tangent = (curvePoints[i + 1] - centerPoint).normalized;// * radius;
            
            // Generate circle points
            for (var j = 0; j < numberOfCirclePoints; j++)
            {
                // Generate circle point by rotating the normal vector around the tangent vector
                // by a certain degree (step size)
                var quatRot = Quaternion.AngleAxis(j * _degreeStepSize, tangent);
                var circlePoint = centerPoint + quatRot * cpn;
                circlePoint *= ScalingFactor;//curve.TableScalingFactor;
                
                tubePoints.Add(circlePoint);    
            }
            
        }
        
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
                // Debug.Log("lidCount: " + beginningLidPoints.Count);
                indices = GenerateCurveMeshTriangleIndices(tubePoints, true);//GenerateLidIndices(beginningLidPoints);
                
                // // Draw triangles twice to cover both sides
                // var backIndices = GenerateTriangleIndices(displayVertices, true);
                // displayVertices.AddRange(displayVertices);
                // indices.AddRange(backIndices);
                break;
            
        }

        tubeMesh = new Mesh
        {
            name= "Tube mesh",
            vertices = tubePoints.ToArray()
        };
        
        tubeMesh.SetIndices(indices.ToArray(), topology, 0);
        tubeMesh.RecalculateNormals();

        
        
        GetComponent<MeshRenderer>().material = TubeMat;

        // Set mesh
        _meshFilter.mesh = tubeMesh;
        
        // Assign mesh to collider
        //collider.convex = true;
        _meshCollider.sharedMesh = tubeMesh;
        
        
        // for (var i = 0; i < tubePoints.Count; i += numberOfCirclePoints)
        // {
        //     for (var j = 0; j < numberOfCirclePoints; j++)
        //     {
        //         SpawnCube(
        //             tubePoints[i + numberOfCirclePoints + j],
        //             Color.red,
        //             new Vector3(0.05f, 0.05f, 0.05f),
        //             "cp_" + i
        //         );
        //     }
        // }
        
        
    }

    private void GenerateBottomLidMesh()
    {
        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var firstPoint = curve.WorldPoints[0];
        var firstCircle = tubePoints.GetRange(0, numberOfCirclePoints);
        
        bottomLidMesh = GenerateLidMesh(firstPoint, firstCircle);
        bottomLidMesh.name = "bottom lid mesh";
        
        if (_bottomLidGameObject is null)
        {
            _bottomLidGameObject = new GameObject("Bottom Lid", new[]
            {
                typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
            });    
            
            _bottomLidGameObject.transform.parent = transform;
        }

        _bottomLidGameObject.GetComponent<MeshRenderer>().material = TubeMat;

        // Set mesh
        _bottomLidGameObject.GetComponent<MeshFilter>().mesh = bottomLidMesh;
        
        // Assign mesh to collider
        _bottomLidGameObject.GetComponent<MeshCollider>().sharedMesh = bottomLidMesh;
        //collider.convex = true;
    }

    private void GenerateTopLidMesh()
    {
        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var lastPoint = curve.WorldPoints.Last();
        var lastCircle = tubePoints.GetRange(
            tubePoints.Count - numberOfCirclePoints - 1, numberOfCirclePoints);

        //
        // for (var i = 0; i < lastCircle.Count; i++)
        // {
        //     SpawnCube(
        //         lastCircle[i],
        //         Color.red,
        //         new Vector3(0.05f, 0.05f, 0.05f),
        //         "cp_" + i
        //         );
        // }
        
        // Debug.Log(
        //     "count: " + tubePoints.Count +
        //     ", idx: " + (tubePoints.Count - numberOfCirclePoints - 1) +
        //     ", numberOf: " + numberOfCirclePoints);
        
        //bottomLidMesh.SetIndices(beginningLidIndices, MeshTopology.Triangles, 0);

        topLidMesh = GenerateLidMesh(lastPoint, lastCircle);

        topLidMesh.name = "top lid mesh";
        
        if (_topLidGameObject is null)
        {
            _topLidGameObject = new GameObject("Top Lid", new[]
            {
                typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
            });    
            
            _topLidGameObject.transform.parent = transform;
        }

        _topLidGameObject.GetComponent<MeshRenderer>().material = TubeMat;

        // Set mesh
        _topLidGameObject.GetComponent<MeshFilter>().mesh = topLidMesh;
        
        // Assign mesh to collider
        _topLidGameObject.GetComponent<MeshCollider>().sharedMesh = topLidMesh;
        //collider.convex = true;




        
        //var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        
        // var lastPoint = curve.WorldPoints.Last();
        // var lastCircle = tubePoints.GetRange(tubePoints.Count - numberOfCirclePoints, numberOfCirclePoints);
        // var endingLidPoints = new List<Vector3>();
        // for (var i = 0; i < lastCircle.Count - 1; i++)
        // {
        //     endingLidPoints.Add(lastPoint);
        //     endingLidPoints.Add(lastCircle[i]);
        //     endingLidPoints.Add(lastCircle[i + 1]);
        // }
    }

    private Mesh GenerateLidMesh(Vector3 center, List<Vector3> circlePoints)
    {
        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var pos = transform.position;
        var centerPoint = pos + center * ScalingFactor;//curve.TableScalingFactor;
        //centerPoint *= curve.TableScalingFactor;
        
        var beginningLidPoints = new List<Vector3>();
        for (var i = 0; i < circlePoints.Count - 1; i++)
        {
            // Transform point to local coordinate system since center point is not port of the tube mesh,
            // i.e. it's position has not been transformed to the local mesh coordinate system

             //transform.InverseTransformPoint(firstPoint) ;
            var tv2 = pos + circlePoints[i]; //(firstCircle[i] - firstPoint).normalized * radius;
            var tv3 = pos + circlePoints[i + 1];//(firstCircle[i + 1] - firstPoint).normalized * radius;

            // tv1 *= curve.TableScalingFactor;
            // tv2 *= curve.TableScalingFactor;
            // tv3 *= curve.TableScalingFactor;

            
            
            // Debug.Log("Triangle 01: " + centerPoint.ToString("F8") + "\n" +
            //           "Triangle 02: " + tv2.ToString("F8") + "\n" +
            //           "Triangle 03: " + tv3.ToString("F8") + "\n"
            // );
            
            // SpawnCube(centerPoint, Color.red, new Vector3(0.005f, 0.005f, 0.005f), "center_" + i);
            // SpawnCube(tv2, Color.green, new Vector3(0.005f, 0.005f, 0.005f), "cp_" + i);
            // SpawnCube(tv3, Color.blue, new Vector3(0.005f, 0.005f, 0.005f), "cp_" + (i+1));
            
            beginningLidPoints.Add(centerPoint);
            beginningLidPoints.Add(tv2);
            beginningLidPoints.Add(tv3);
        }

        // Add last triangle that connects to the first circle point
        beginningLidPoints.Add(centerPoint);
        beginningLidPoints.Add(pos + circlePoints.Last());
        beginningLidPoints.Add(pos + circlePoints.First());
        
        
        var beginningLidIndices = GenerateBottomLidTriangleIndices(beginningLidPoints);
        
        bottomLidMesh = new Mesh
        {
            vertices = beginningLidPoints.ToArray(),
        };

        bottomLidMesh.SetIndices(beginningLidIndices, MeshTopology.Triangles, 0);
        bottomLidMesh.RecalculateNormals();
        
        return bottomLidMesh;
    }
    
    
    private List<int> GenerateCurveMeshTriangleIndices(List<Vector3> tubePoints, bool windClockwise)
    {
        var indicesList = new List<int>();
        for (var i = 0; i < tubePoints.Count - 1; i += numberOfCirclePoints)
        {
            var currSet = tubePoints.GetRange(i, numberOfCirclePoints);

            for (var j = 0; j < numberOfCirclePoints; ++j)
            {
                var baseIndex = i + j;

                if (baseIndex >= (tubePoints.Count - numberOfCirclePoints - 1)) continue;
                
                // Debug.Log(
                //     "baseIndex: " + baseIndex +
                //     ", baseIndex + 1: " + (baseIndex + 1) +
                //     ", baseIndex + Num: " + (baseIndex + numberOfCirclePoints) +
                //     ", baseIndex + Num + 1: " + (baseIndex + numberOfCirclePoints + 1)
                // );
                
                // upper right triangle
                indicesList.Add(baseIndex);

                if (windClockwise)
                {
                    indicesList.Add(baseIndex + numberOfCirclePoints + 1);
                    indicesList.Add(baseIndex + numberOfCirclePoints);
                }
                else
                {
                    indicesList.Add(baseIndex + numberOfCirclePoints);
                    indicesList.Add(baseIndex + numberOfCirclePoints + 1);    
                }
                        
                
                
                // lower left triangle
                indicesList.Add(baseIndex);
                
                

                if (windClockwise)
                {
                    indicesList.Add(baseIndex + 1);
                    indicesList.Add(baseIndex + numberOfCirclePoints + 1);
                }
                else
                {
                    indicesList.Add(baseIndex + numberOfCirclePoints + 1);
                    indicesList.Add(baseIndex + 1);
                }
                
                //
                // indicesList.Add(baseIndex + 1);
                // indicesList.Add(baseIndex);
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

        // var lastLid = tubePoints.GetRange(tubePoints.Count - lidPointCount, lidPointCount);
        // for (var i = lowerBound; i < upperBound; i++)
        // {
        //     indicesList.Add(i);
        //     indicesList.Add(i + 1);
        //     indicesList.Add(i + 2);
        // }
        
        return indicesList;
    }

    private List<int> GenerateBottomLidTriangleIndices(List<Vector3> lidPoints)
    {
        var indicesList = new List<int>();

        for (var i = 0; i < lidPoints.Count; i++)
        {
            indicesList.Add(i);
        }    
        
        return indicesList;
    }


    private void SpawnCube(Vector3 pos, Color color, Vector3 scale, string name = "cube")
    {
        var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.parent = transform;
        cube.transform.position = pos;
        cube.transform.localScale = scale; //new Vector3(0.005f, 0.005f, 0.005f);
        cube.GetComponent<MeshRenderer>().material.color = color;
        cube.name = name;
    }
}
