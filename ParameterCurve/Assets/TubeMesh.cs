using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model;
using UnityEngine;

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
        private void GenerateFieldMesh()
        {
            var mesh = new Mesh
            {
                name= "Param curve mesh",
            };

            var curve = GlobalDataModel.DisplayCurveDatasets[GlobalDataModel.CurrentCurveIndex];

            var curvePoints = curve.WorldPoints;
            // var displayVertices = new List<Vector3>();
            //
            // var x_min = dVertices.Min(v => v.x);
            // var x_max = dVertices.Max(v => v.x);
            // var y_min = dVertices.Min(v => v.y);
            // var y_max = dVertices.Max(v => v.y);
            // var z_min = dVertices.Min(v => v.z);
            // var z_max = dVertices.Max(v => v.z);    
            //

            var tubePoints = new List<Vector3>();
            
            var step = 360f / numberOfCirclePoints;
            
            for (var i = 0; i < curvePoints.Count; i++)
            {
                var centerPoint = curvePoints[i];
                var normal = curve.FresnetApparatuses[i].Normal;

                for (var j = 0; j < numberOfCirclePoints; j++)
                {
                    var rad = (j * step) * 2f * Mathf.PI / 360;
                    var circlePoint = new Vector3(
                        centerPoint.x + Mathf.Cos(rad) * radius,
                        centerPoint.y ,
                        centerPoint.z + Mathf.Sin(rad) * radius
                    );

                    circlePoint *= curve.TableScalingFactor;
                    
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
                    indices = GenerateTriangleIndices(tubePoints);
                    // // Draw triangles twice to cover both sides
                    // var backIndices = GenerateTriangleIndices(displayVertices, true);
                    // displayVertices.AddRange(displayVertices);
                    // indices.AddRange(backIndices);
                    break;
            
                // case MeshTopology.Quads:
                //     indices = GenerateQuadIndices(tubePoints);
                //     break;
                    
            }
            
            mesh.vertices = tubePoints.ToArray();
            
            //Debug.Log("Number of points: " + displayVertices.Count);
            
            mesh.SetIndices(indices.ToArray(), topology, 0, true);

            //ScalarFieldManager.CurrentField.MeshPoints = displayVertices;
            

            // Calculate normals
            //var normals = CalculateNormals(displayVertices, indices, MeshTopology.Triangles);
            //mesh.normals = normals.ToArray();

            mesh.RecalculateNormals();

            var colorList = new List<Color>();
            for(var i = 0; i < tubePoints.Count; i++)
            {
                colorList.Add(Color.red);
            }

            mesh.colors = colorList.ToArray();
            
            // if (log)
            // {
            //     Debug.Log("Normals:" + normals.Count);
            // }
            
            // var colorList = ColorTransferFunction(displayVertices); //GenerateColors(vertices);
            // mesh.colors = colorList.ToArray();

            // mesh.colors = new[]
            // {
            //     Color.red, Color.green, Color.blue
            // };

            // // Calculate UV texture coordinates
            // var uvs = new Vector2[displayVertices.Count];
            //
            // for (var i = 0; i < dVertices.Count; i++)
            // {
            //     var vertex = dVertices[i];
            //     
            //     // Map x and z coordinates to texture coordinate space
            //     // (z is used because y and z are flipped for display vectors)
            //     var x = CalcUtility.MapValueToRange(vertex.x, x_min, x_max, 0f, 1f);
            //     var z = CalcUtility.MapValueToRange(vertex.z, z_min, z_max, 0f, 1f);
            //     uvs[i] = new Vector2(x, z);
            // }
            //
            //mesh.uv = uvs;
        
            var lineIndices = new List<int>();
            //mesh.SetIndices(new int[] {0,1, 2,3, 4,5 }, MeshTopology.Lines, 0, true); 
        
            // mesh.RecalculateNormals();
            // mesh.RecalculateBounds();

            GetComponent<MeshRenderer>().material = TubeMat;

            // Set mesh
            GetComponent<MeshFilter>().mesh = mesh;
            
            // Assign mesh to collider
            var meshCollider = GetComponent<MeshCollider>();
            //collider.convex = true;
            meshCollider.sharedMesh = mesh;
            
            
        }

        private List<int> GenerateTriangleIndices(List<Vector3> tubePoints)
        {
            var indicesList = new List<int>();
            //var prevSet = tubePoints.GetRange(0, numberOfCirclePoints);
            for (var i = 0; i < tubePoints.Count; i += numberOfCirclePoints)
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

            return indicesList;
        }


        private void GenerateTriangleIndices()
        {
            
        }
}
