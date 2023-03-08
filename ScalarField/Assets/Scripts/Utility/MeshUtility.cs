using System.Collections;
using System.Collections.Generic;
using Model;
using UnityEngine;

namespace Utility
{
    public static class MeshUtility
    {
        public static void UpdateMeshComponents(
            ScalarField sf, GameObject source, GameObject boundingBox, Bounds bounds
            )
        {
            var mesh = GenerateFieldMesh(sf, boundingBox);
            // mesh.RecalculateNormals();
            // mesh.RecalculateBounds();
            
            // Set mesh
            source.GetComponent<MeshFilter>().mesh = mesh;
            
            // Assign mesh to collider
            //var meshCollider = GetComponent<MeshCollider>();
            //collider.convex = true;
            source.GetComponent<MeshCollider>().sharedMesh = mesh;
            
            // if (PositionMeshAtOrigin)
            // {
            //     PositionMeshCenterAtOrigin(tf, bounds);
            // }   
        
            var mat = source.GetComponent<MeshRenderer>().material;
            //mat.color = new Color(r: 0.75f, g: 0.75f, b: 0.75f, a: 1f);
            
            mat.mainTexture = sf.MeshTexture;
        }
        
        
        /// <summary>
        /// Creates the mesh by calculation the topology
        /// </summary>
        public static Mesh GenerateFieldMesh(ScalarField sf, GameObject boundingBox)
        {
            var dVertices = sf.DisplayPoints;
            var yMin = sf.MinDisplayValues.y;//dVertices.Min(v => v.y);
            var yMax = sf.MaxDisplayValues.y;//dVertices.Max(v => v.y);
            var bounds = boundingBox.GetComponent<MeshRenderer>().bounds;
            var displayVertices = CalcUtility.MapDisplayVectors(dVertices, bounds, boundingBox.transform);
            
            var topology = MeshTopology.Triangles;
            List<int> indices = new List<int>();

            // Generate topology indices based on chosen topology
            switch (topology)
            {
                case MeshTopology.Triangles:
                    indices = GenerateTriangleIndices(displayVertices, false, sf.SampleCount);
                    // Draw triangles twice to cover both sides
                    var backIndices = GenerateTriangleIndices(displayVertices, true, sf.SampleCount);
                    displayVertices.AddRange(displayVertices);
                    indices.AddRange(backIndices);    
                    
                    break;
            
                case MeshTopology.Lines:
                    indices = GenerateLineIndices(displayVertices, sf.SampleCount);
                    break;
            }
            
            // Calculate normals
            var normals = CalculateNormals(displayVertices, indices, MeshTopology.Triangles, sf.SampleCount);

            // Calculate UV texture coordinates
            var uvs = new Vector2[displayVertices.Count];
            for (var i = 0; i < dVertices.Count; i++)
            {   
                var y = CalcUtility.MapValueToRange(dVertices[i].y, yMin, yMax, 0f, 1f);
                uvs[i] = new Vector2(y, 0.5f);
            }
        
            var mesh = new Mesh
            {
                name="Scalar field mesh",
                vertices = displayVertices.ToArray(),
                triangles = indices.ToArray(),
                normals = normals.ToArray(),
                uv = uvs
            };
            
            return mesh;
        }
        
        /// <summary>
        /// Generate triangle indices (triangulation of the mesh surface)
        /// </summary>
        /// <param name="vertices">Point collection</param>
        /// <param name="windClockWise">
        ///     True: Set indices clockwise (front-face in unity)
        ///     False: Set indices counterclockwise (back-face in unity)
        /// </param>
        /// <returns></returns>
        private static List<int> GenerateTriangleIndices(ICollection vertices, bool windClockWise, int sampleCount)
        {
            //var sampleCount = ScalarFieldManager.CurrentField.SampleCount;
            var indicesList = new List<int>();

            for (var i = 0; i < vertices.Count; i++)
            {
                // Determine if we have to draw a lower left triangle above the current point
                var isUpperBound = i % sampleCount == (sampleCount - 1);
                var isLastColumn = i >= (sampleCount * sampleCount - sampleCount);
            
                // If we are not in the last column, and not on the upper bound of the column, draw lower left triangle
                // above the current position
                var drawLowerLeftTriangle = !isLastColumn && !isUpperBound;

                if (drawLowerLeftTriangle)
                {
                    /*
                        c
                        -
                        ----
                        -------
                        ----------
                        -------------
                        ----------------
                       a (i)            b                
                    */
                    var a = i;
                    var b = i + sampleCount;
                    var c = i + 1;
                
                    // Choose index order based on winding direction
                    if (windClockWise)
                    {
                        indicesList.Add(a);
                        indicesList.Add(c);
                        indicesList.Add(b);    
                    }
                    else
                    {
                        indicesList.Add(a);
                        indicesList.Add(b);
                        indicesList.Add(c);
                    }
                }

                // Determine if we have to draw a upper right triangle below the current point
                var isLowerBound = i % sampleCount == 0;
                var isFirstColumn = i < sampleCount;
                var drawUpperRightTriangle = !isLowerBound && !isFirstColumn;

                // If we are not in the first column, and not on the lower bound of the column, draw upper right triangle
                // below the current position
                if (drawUpperRightTriangle)
                {
                    /*
                       c                a
                        ----------------
                           -------------
                              ----------
                                 -------
                                    ----                     
                                       -
                                       b              
                    */
                    var a = i;
                    var b = i - 1;
                    var c = i - sampleCount;
                
                    // Choose index order based on winding direction
                    if (windClockWise)
                    {    
                        indicesList.Add(a);
                        indicesList.Add(b);
                        indicesList.Add(c);    
                    }
                    else
                    {
                        indicesList.Add(a);
                        indicesList.Add(c);
                        indicesList.Add(b);
                    }
                    
                }
            }

            return indicesList;
        }

        // ToDo: Test this and try to generate wireframe structure from this ?
        private static List<int> GenerateLineIndices(List<Vector3> vertices, int sampleCount)
        {
            var indices = new List<int>();

            for (int i = 0; i < vertices.Count; i += 2)
            {
                if (i == vertices.Count - 1)
                    continue;
            
                // Left
                indices.Add(i);
                indices.Add(i + 1);
            
                // Up
                indices.Add(i + 1);
                indices.Add(i + sampleCount + 1);
            
                // Right
                indices.Add(i + sampleCount + 1);
                indices.Add(i + sampleCount);
            
                // Down
                indices.Add(i + sampleCount);
                indices.Add(i);
            }

            return indices;
        }

        /// <summary>
        /// Calculates the normals for each vertex in the mesh
        /// </summary>
        /// <param name="vertices"></param>
        /// <param name="indices"></param>
        /// <param name="topology"></param>
        /// <param name="sampleCount"></param>
        /// <returns></returns>
        private static List<Vector3> CalculateNormals(
            List<Vector3> vertices, List<int> indices, MeshTopology topology, int sampleCount)    
        {    
            var normals = new List<Vector3>();

            // Add zero vectors
            for (var i = 0; i < vertices.Count; i++)
            {
                normals.Add(Vector3.zero);
            }

            // Based on chosen topology
            switch (topology)
            {
                case MeshTopology.Triangles:
                    
                    // Recreate triangles based on indices
                    for (var i = 0; i < indices.Count; i+=3)
                    {
                        // Calculate local normals for the given triangle
                        var normal = CalculateTriangleNormal(
                            vertices[indices[i]],
                            vertices[indices[i + 1]],
                            vertices[indices[i + 2]]
                        );

                        // If a vertex already has an assigned normal from a previous triangle it is a part of,
                        // simply add the normal calculated from the new triangle and normalize again
                        normals[indices[i]] = (normals[indices[i]] + normal).normalized;
                        normals[indices[i + 1]] = (normals[indices[i + 1]] + normal).normalized;
                        normals[indices[i + 2]] = (normals[indices[i + 2]] + normal).normalized;
                    }

                    break;
                
                case MeshTopology.Lines:
                    for (int i = 0; i < indices.Count; i++)
                    {
                        var normal = CalculateQuadNormal(
                            vertices[indices[i]],
                            vertices[indices[i + 1]],
                            vertices[indices[i + sampleCount + 1]],
                            vertices[indices[i + sampleCount]]
                        );
                    }

                    break;
            }

            return normals;
        }
    
        /// <summary>
        /// Calculates the normal vector based on three vertices forming a triangle
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <returns></returns>
        private static Vector3 CalculateTriangleNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var u = p2 - p1;
            var v = p3 - p1;

            return new Vector3
            (
                u.y * v.z - u.z * v.y,
                u.z * v.x - u.x * v.z,
                u.x * v.y - u.y * v.x
            ).normalized;
        }

        /// <summary>
        /// Calculates the normal vector based on four vertices forming a 2D square (quad)
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <param name="p3"></param>
        /// <param name="p4"></param>
        /// <returns></returns>
        private static Vector3 CalculateQuadNormal(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            //     v1        v2
            //     +---------+
            //     |         | 
            //     |         |
            //     +---------+
            //     v3        v4
        
            // CrossProduct((v2-v1), (v3-v1))
        
            //     2         3
            //     +---------+
            //     |         | 
            //     |         |
            //     +---------+
            //     1         4
            return Vector3.Cross(p3 - p2, p1 - p2).normalized;
        }
    }
}