using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

[RequireComponent(
    typeof(MeshFilter),
    typeof(MeshRenderer),
    typeof(MeshCollider))]
public class TubeMesh : MonoBehaviour
{
    /// <summary>
    /// Material used for the surface of the tube
    /// </summary>
    public Material TubeMat;

    /// <summary>
    /// Material used for the highlighting spheres on the tube
    /// </summary>
    public Material SphereMat;

    /// <summary>
    /// General scaling factor applied to the tube mesh
    /// </summary>
    public float TubeMeshScalingFactor = 1f;

    private float SphereScalingFactor = 2f;

    
    /// <summary>
    /// Radius of the final tube
    /// </summary>
    private float radius = 0.1f;
    
    /// <summary>
    /// Number of point used to sample the flat circle around every curve point, making up the outside of the tube mesh
    /// </summary>
    private int numberOfCirclePoints = 8;

    /// <summary>
    /// Sub-mesh for the outer surface of the tube along the curve
    /// </summary>
    private Mesh tubeMesh;
    
    /// <summary>
    /// Sub-mesh for the bottom lid of the tube, facing outward
    /// </summary>
    private Mesh bottomLidMesh;
    
    /// <summary>
    /// Sub-mesh for the top lid of the tube, facing outward
    /// </summary>
    private Mesh topLidMesh;
    

    private List<Vector3> _curvePoints = new List<Vector3>();

    /// <summary>
    /// Number of points making up the mesh
    /// </summary>
    private List<Vector3> tubePoints = new List<Vector3>();
    
    /// <summary>
    /// Number of spheres highlighting the points on the curve
    /// </summary>
    private List<GameObject> spheres = new List<GameObject>();
    
    /// <summary>
    /// Game object for the bottom lid mesh
    /// </summary>
    private GameObject _bottomLidGameObject;
    
    /// <summary>
    /// Game object for the top lid mesh
    /// </summary>
    private GameObject _topLidGameObject;

    /// <summary>
    /// Calculated amount of degrees between the points along the circle surrounding each curve point
    /// </summary>
    private float _degreeStepSize;

    private int NumberOfSamplingPoints = -1;

    private MeshRenderer _meshRenderer;
    private MeshFilter _meshFilter;
    private MeshCollider _meshCollider;
    
    
    public void Awake()
    {
        // Cache components
        _meshCollider = GetComponent<MeshCollider>();
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        
        // Set tube radius based on scaling factor
        radius = 0.05f * TubeMeshScalingFactor;
        SphereScalingFactor = 0.125f * TubeMeshScalingFactor;
    }

    /// <summary>
    /// Creates the mesh by calculation the topology
    /// </summary>
    public void GenerateFieldMesh()
    {
        CalculateMesh();
    }
    
    /// <summary>
    /// Creates the mesh using a given amount of points to sample the curve
    /// </summary>
    /// <param name="sampleCount">Sample count</param>
    public void GenerateFieldMesh(int sampleCount)
    {
        NumberOfSamplingPoints = sampleCount;
        CalculateMesh();
    }

    /// <summary>
    /// Mesh calculation function
    /// </summary>
    private void CalculateMesh()
    {
        if(GlobalDataModel.DisplayCurveDatasets == null) return;
        if(GlobalDataModel.DisplayCurveDatasets.Count == 0) return;

        tubePoints.Clear();
        _degreeStepSize = 360f / numberOfCirclePoints;
        
        GenerateCurveMesh();
        GenerateBottomLidMesh();
        GenerateTopLidMesh();
    }
    
    
    /// <summary>
    /// Generates the main surface around the curve points
    /// </summary>
    private void GenerateCurveMesh()
    {
        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var curvePoints = curve.WorldPoints; 

        // When a sample count was given, sample curve
        if(NumberOfSamplingPoints != -1)
        {
            var div = curvePoints.Count / NumberOfSamplingPoints;
            _degreeStepSize = NumberOfSamplingPoints;

            var newPointList = new List<Vector3>();
            for(var i = 0; i < curvePoints.Count; i++)
            {
                // Only sample every nth point
                if(i % div == 0)
                {
                    var p = curvePoints[i];
                    //p += transform.position;

                    
                    //Debug.Log("cp: " + p + ", tp: " + transform.position + ", np: " + p);
                    
                    //p *= ScalingFactor;
                    newPointList.Add(p);
                }
            }
            
            //Debug.Log("pointCount: " + newPointList.Count);

            foreach(var sphere in spheres)
            {
                Destroy(sphere);
            }

            spheres.Clear();



            foreach(var point in newPointList)
            {
                var calcPoint = point;
                if(!curve.Is3DCurve)
                {
                    calcPoint = new Vector3(point.x, point.y, 0f);
                }
                
                calcPoint *= TubeMeshScalingFactor;
                var spherePoint = this.transform.position + calcPoint;

                var sphere = DrawingUtility.DrawSphereScaled(spherePoint, transform, Color.black, 
                    SphereScalingFactor,
                    SphereMat);

                // Debug.Log("tfpos: " + this.transform.position + ", point: " + calcPoint + ", sum: " + spherePoint + 
                // ", spherePos: " + sphere.transform.position);

                sphere.GetComponent<MeshRenderer>().sharedMaterial = SphereMat;

                spheres.Add(sphere);
            }

            curvePoints = newPointList;
        }
        
        // Calculate surface mesh points
        for (var i = 0; i < curvePoints.Count; i++)
        {
            // Get curve point and direction vectors
            var centerPoint = curvePoints[i];
            var normal = curve.FresnetApparatuses[i].Normal;
            var tangent = curve.FresnetApparatuses[i].Tangent;

            _curvePoints.Add(centerPoint);

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
                var quaternionRot = Quaternion.AngleAxis(j * _degreeStepSize, tangent);
                var circlePoint = centerPoint + quaternionRot * cpn;
                circlePoint *= TubeMeshScalingFactor;//curve.TableScalingFactor;
                
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
        
        //GetComponent<MeshRenderer>().material = TubeMat;
        _meshRenderer.material = TubeMat;
        
        // Set mesh
        _meshFilter.mesh = tubeMesh;
        
        // Assign mesh to collider
        //collider.convex = true;
        _meshCollider.sharedMesh = tubeMesh;
    }

    // private void GenerateCurveMesh()
    // {
    //     var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
    //     var curvePoints = curve.WorldPoints; 
        
    //     for (var i = 0; i < curvePoints.Count; i++)
    //     {
    //         var centerPoint = curvePoints[i];
    //         var normal = curve.FresnetApparatuses[i].Normal;
    //         var tangent = curve.FresnetApparatuses[i].Tangent;

    //         //  Calculate 3d point of normal target and change vector so the generated circle
    //         //  matches the given radius
    //         var cpn = normal * radius; //(centerPoint + normal).normalized * radius;
            
    //         // On first point, generate tangent pointing to next point since
    //         // we don't have any velocity at the beginning, i.e. the tangent is (0,0,0)
    //         if (i == 0)
    //             tangent = (curvePoints[i + 1] - centerPoint).normalized;// * radius;
            
    //         // Generate circle points
    //         for (var j = 0; j < numberOfCirclePoints; j++)
    //         {
    //             // Generate circle point by rotating the normal vector around the tangent vector
    //             // by a certain degree (step size)
    //             var quatRot = Quaternion.AngleAxis(j * _degreeStepSize, tangent);
    //             var circlePoint = centerPoint + quatRot * cpn;
    //             circlePoint *= ScalingFactor;//curve.TableScalingFactor;
                
    //             tubePoints.Add(circlePoint);    
    //         }
            
    //     }
        
    //     var topology = MeshTopology.Triangles;
    //     var indices = new List<int>();

    //     // Generate topology indices based on chosen topology
    //     switch (topology)
    //     {
    //         default:
    //         case MeshTopology.Triangles:
    //             // indices = GenerateTriangleIndices(
    //             //     tubePoints, beginningLidPoints.Count
    //             // );
    //             // Debug.Log("lidCount: " + beginningLidPoints.Count);
    //             indices = GenerateCurveMeshTriangleIndices(tubePoints, true);//GenerateLidIndices(beginningLidPoints);
                
    //             // // Draw triangles twice to cover both sides
    //             // var backIndices = GenerateTriangleIndices(displayVertices, true);
    //             // displayVertices.AddRange(displayVertices);
    //             // indices.AddRange(backIndices);
    //             break;
            
    //     }

    //     tubeMesh = new Mesh
    //     {
    //         name= "Tube mesh",
    //         vertices = tubePoints.ToArray()
    //     };
        
    //     tubeMesh.SetIndices(indices.ToArray(), topology, 0);
    //     tubeMesh.RecalculateNormals();

        
        
    //     GetComponent<MeshRenderer>().material = TubeMat;

    //     // Set mesh
    //     _meshFilter.mesh = tubeMesh;
        
    //     // Assign mesh to collider
    //     //collider.convex = true;
    //     _meshCollider.sharedMesh = tubeMesh;
        
        
    //     // for (var i = 0; i < tubePoints.Count; i += numberOfCirclePoints)
    //     // {
    //     //     for (var j = 0; j < numberOfCirclePoints; j++)
    //     //     {
    //     //         SpawnCube(
    //     //             tubePoints[i + numberOfCirclePoints + j],
    //     //             Color.red,
    //     //             new Vector3(0.05f, 0.05f, 0.05f),
    //     //             "cp_" + i
    //     //         );
    //     //     }
    //     // }
        
        
    // }

    private void GenerateBottomLidMesh()
    {
        var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];
        var firstPoint = curve.WorldPoints[0];
        var firstCircle = tubePoints.GetRange(0, numberOfCirclePoints);
        
        // var sb = new StringBuilder();
        //
        // for(var i = 0; i < curve.WorldPoints.Count(); i++)
        // {
        //     sb.AppendLine(i + ": " + curve.WorldPoints[i]);
        // }
        // Debug.Log("WorldPoints:\n" + sb.ToString());

        bottomLidMesh = GenerateLidMesh(firstPoint, firstCircle);
        bottomLidMesh.name = "bottom lid mesh";
        
        if (_bottomLidGameObject is null)
        {
            _bottomLidGameObject = new GameObject("Bottom Lid", new[]
            {
                typeof(MeshRenderer), typeof(MeshFilter), typeof(MeshCollider)
            });    
            
            _bottomLidGameObject.transform.parent = transform;
            _bottomLidGameObject.transform.position = _curvePoints[0]; //transform.position;
            //_bottomLidGameObject.transform.localPosition = Vector3.zero;
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
        var lastPoint = curve.WorldPoints[curve.WorldPoints.Count - 1];
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
        var centerPoint = pos + center; // * TubeMeshScalingFactor;
        //curve.TableScalingFactor;
        //centerPoint *= curve.TableScalingFactor;
        
        // string s = "";
        // for(var i = 0; i < circlePoints.Count; i++)
        // {
        //     s += circlePoints[i].ToString("0.0000000") + "\n";
        // }
        //
        // Debug.Log("tfname: " + transform.gameObject.name + ", center: " + center + ", pos: " + transform.position + ", centerPoint: " + centerPoint
        // + "\n" + s);

        var beginningLidPoints = new List<Vector3>();
        for (var i = 0; i < circlePoints.Count - 1; i++)
        {
            // Transform point to local coordinate system since center point is not part of the tube mesh,
            // i.e. it's position has not been transformed to the local mesh coordinate system
            var tv2 = pos + circlePoints[i];
            var tv3 = pos + circlePoints[i + 1];
            
            beginningLidPoints.Add(centerPoint);
            beginningLidPoints.Add(tv2);
            beginningLidPoints.Add(tv3);
        }

        var reverseList = beginningLidPoints.AsEnumerable().Reverse();
        
        // Add last triangle that connects to the first circle point
        beginningLidPoints.Add(centerPoint);
        beginningLidPoints.Add(pos + circlePoints[circlePoints.Count - 1]); //.Last());
        beginningLidPoints.Add(pos + circlePoints[0]);//.First());
        
        //Debug.Log("lidPointCount: " + beginningLidPoints.Count);

        // for (var i = 0; i < beginningLidPoints.Count; i++)
        // {
        //     var sphere = 
        //         DrawingUtility.DrawSphereScaled(beginningLidPoints[i], transform, Color.blue, 0.025f, SphereMat);
        //     sphere.name = "lidPoint_" + i;
        // }
        
        beginningLidPoints.AddRange(reverseList);
        
        beginningLidPoints.Add(centerPoint);
        beginningLidPoints.Add(pos + circlePoints[0]);//.First());
        beginningLidPoints.Add(pos + circlePoints[circlePoints.Count - 1]); //.Last());
        
        // var centerSphere = DrawingUtility.DrawSphereScaled(centerPoint, transform, Color.blue, 0.125f, SphereMat);
        // centerSphere.name = "centerSphere_" + transform.gameObject.name;

        // Triangle 01
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[0]);
        // beginningLidPoints.Add(pos + circlePoints[1]);
        //
        // // Triangle 02
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[1]);
        // beginningLidPoints.Add(pos + circlePoints[2]);
        //
        // // Triangle 03
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[2]);
        // beginningLidPoints.Add(pos + circlePoints[3]);
        //
        // // Triangle 04
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[3]);
        // beginningLidPoints.Add(pos + circlePoints[4]);
        //
        // // Triangle 05
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[4]);
        // beginningLidPoints.Add(pos + circlePoints[5]);
        //
        // // Triangle 06
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[5]);
        // beginningLidPoints.Add(pos + circlePoints[6]);
        //
        // // Triangle 07
        // beginningLidPoints.Add(centerPoint);
        // beginningLidPoints.Add(pos + circlePoints[6]);
        // beginningLidPoints.Add(pos + circlePoints[7]);
        //

        
        var beginningLidIndices = GenerateLidTriangleIndices(beginningLidPoints);
        
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

    private List<int> GenerateLidTriangleIndices(List<Vector3> lidPoints)
    {
        var indicesList = new List<int>()
        {
            //0, 1, 2, 
            //3, 4, 5, 
            // 6, 7, 8,
            // 9, 10, 11,
            // 12, 13, 14,
            // 15, 16, 17,
            // 18, 19, 20,

            // 0, 1, 2,
            // 0, 2, 3,
            // 0, 3, 4,
            // 0, 4, 5,
            // 0, 5, 6,
            // 0, 6, 7,
            // 0, 7, 1,
            //
            // 0, 7, 6,
            // 0, 6, 5,
            // 0, 5, 4,
            // 0, 4, 3,
            // 0, 3, 2,
            // 0, 2, 1,
            // 0, 1, 7
        };

        // var s = new StringBuilder();
        for (var i = 0; i < lidPoints.Count; i++)
        {
            //s.AppendLine(i + "");
            indicesList.Add(i);
        }    
        //
        // Debug.Log(s.ToString());
        
        return indicesList;
    }


    // private void SpawnCube(Vector3 pos, Color color, Vector3 scale, string name = "cube")
    // {
    //     var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
    //     cube.transform.parent = transform;
    //     cube.transform.position = pos;
    //     cube.transform.localScale = scale; //new Vector3(0.005f, 0.005f, 0.005f);
    //     cube.GetComponent<MeshRenderer>().material.color = color;
    //     cube.name = name;
    // }
}
