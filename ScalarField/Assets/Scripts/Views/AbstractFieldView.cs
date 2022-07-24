using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Model.ScriptableObjects;
using UnityEngine;
using Utility;

namespace Views
{
    public abstract class AbstractFieldView
    {
        private readonly ScalarFieldManager _data;
        
        private readonly MeshRenderer _mr;
        private readonly MeshFilter _mf;
        private readonly MeshCollider _mc;

        private readonly Transform _tf;
        private readonly Bounds _bounds;

        private readonly GameObject _boundingBox;
        
        protected AbstractFieldView(ScalarFieldManager data, GameObject mesh, GameObject boundingBox)
        {
            _data = data;
            
            _mr = mesh.GetComponent<MeshRenderer>();
            _mf = mesh.GetComponent<MeshFilter>();
            _mc = mesh.GetComponent<MeshCollider>();

            _tf = mesh.transform;
            _bounds = _mr.bounds;

            _boundingBox = boundingBox;
        }
        
        public virtual void UpdateView()
        {
            //Debug.Log("Generating field mesh...");
            GenerateFieldMesh();
            
            if (PositionMeshAtOrigin)
            {
                PositionMeshCenterAtOrigin(_tf, _bounds);
            }   
        
            var mat = _mr.material;
            mat.color = new Color(r: 0.75f, g: 0.75f, b: 0.75f, a: 1f);
            
            mat.mainTexture = _data.CurrentField.MeshTexture;
        }
        
        public bool PositionMeshAtOrigin;

        private bool _isMeshTransparent;

        public void ToogleMeshTransparency()
        {
            _isMeshTransparent = !_isMeshTransparent;
            
            var attenuation = _isMeshTransparent ? 0.25f : 1f;
            SetMeshTransparency(attenuation);   
        }
        
        private void SetMeshTransparency(float attenuation)
        {
            //var mr = GetComponent<MeshRenderer>();
            var mat = _mr.material;
            mat.color = new Color(mat.color.r, mat.color.g, mat.color.b, attenuation);
            _mr.material = mat;
        }
        
    
        /// <summary>
        /// Creates the mesh by calculation the topology
        /// </summary>
        private void GenerateFieldMesh()
        {
            var mesh = new Mesh
            {
                name="Scalar field mesh",
            };
        
            
            var sf = _data.CurrentField;

            
            var dVertices = sf.DisplayPoints;
            // var displayVertices = new List<Vector3>();
            //
            var x_min = dVertices.Min(v => v.x);
            var x_max = dVertices.Max(v => v.x);
            var y_min = dVertices.Min(v => v.y);
            var y_max = dVertices.Max(v => v.y);
            var z_min = dVertices.Min(v => v.z);
            var z_max = dVertices.Max(v => v.z);    

            var bounds = _boundingBox.GetComponent<MeshRenderer>().bounds;
            var displayVertices = CalcUtility.MapDisplayVectors(dVertices, bounds, _boundingBox.transform);
            
            // var log = false;
            // var sb = new StringBuilder();
            // for (int i = 0; i < displayVertices.Count; i++)
            // {
            //     sb.AppendLine(displayVertices[i].ToString());
            // }
            //
            // if (log)
            // {
            //     Debug.Log("Init vertices List:" + displayVertices.Count + "\n" + sb);
            // }
            
            //sb.Clear();

            var topology = MeshTopology.Triangles;
            var indices = new List<int>();

            // Generate topology indices based on chosen topology
            switch (topology)
            {
                default:
                case MeshTopology.Triangles:
                    indices = GenerateTriangleIndices(displayVertices, false);
                    // Draw triangles twice to cover both sides
                    var backIndices = GenerateTriangleIndices(displayVertices, true);
                    displayVertices.AddRange(displayVertices);
                    indices.AddRange(backIndices);
                    break;
            
                case MeshTopology.Lines:
                    indices = GenerateLineIndices(displayVertices);
                    break;
            }
            
            mesh.vertices = displayVertices.ToArray();
            
            //Debug.Log("Number of points: " + displayVertices.Count);
            
            mesh.SetIndices(indices.ToArray(), topology, 0, true);

            _data.CurrentField.MeshPoints = displayVertices;
            
            // Calculate normals
            var normals = CalculateNormals(displayVertices, indices, MeshTopology.Triangles);
            mesh.normals = normals.ToArray();

            // if (log)
            // {
            //     Debug.Log("Normals:" + normals.Count);
            // }
            
            // Calculate UV texture coordinates
            var uvs = new Vector2[displayVertices.Count];
        
            for (var i = 0; i < dVertices.Count; i++)
            {
                var vertex = dVertices[i];
                
                // Map x and z coordinates to texture coordinate space
                // (z is used because y and z are flipped for display vectors)
                var x = CalcUtility.MapValueToRange(vertex.x, x_min, x_max, 0f, 1f);
                var z = CalcUtility.MapValueToRange(vertex.z, z_min, z_max, 0f, 1f);
                uvs[i] = new Vector2(x, z);
            }
        
            mesh.uv = uvs;
        
            var lineIndices = new List<int>();
            //mesh.SetIndices(new int[] {0,1, 2,3, 4,5 }, MeshTopology.Lines, 0, true); 
        
            // mesh.RecalculateNormals();
            // mesh.RecalculateBounds();
            
            // Set mesh
            _mf.mesh = mesh;
            
            
            
            // Assign mesh to collider
            //var meshCollider = GetComponent<MeshCollider>();
            //collider.convex = true;
            _mc.sharedMesh = mesh;
        }
    
    
        /// <summary>
        /// Reposition the center of the mesh to the origin point of the scene
        /// </summary>
        private static void PositionMeshCenterAtOrigin(Transform transform, Bounds bounds)
        {
            var tmp = transform.position - bounds.center; //MonoBehaviour.GetComponent<MeshRenderer>().bounds.center;
            //var tmp = BoundingBox.GetComponent<MeshRenderer>().bounds.center;
            //Debug.Log("MeshPositioningVector: " + tmp);
            //transform.position += tmp;
            transform.parent.position = tmp;
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
        private List<int> GenerateTriangleIndices(ICollection vertices, bool windClockWise)
        {
            var sampleCount = _data.CurrentField.SampleCount;
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
                    var b = i + _data.CurrentField.SampleCount;
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
                var isLowerBound = i % _data.CurrentField.SampleCount == 0;
                var isFirstColumn = i < _data.CurrentField.SampleCount;
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
                    var c = i - _data.CurrentField.SampleCount;
                
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
        private List<int> GenerateLineIndices(List<Vector3> vertices)
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
                indices.Add(i + _data.CurrentField.SampleCount + 1);
            
                // Right
                indices.Add(i + _data.CurrentField.SampleCount + 1);
                indices.Add(i + _data.CurrentField.SampleCount);
            
                // Down
                indices.Add(i + _data.CurrentField.SampleCount);
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
        /// <returns></returns>
        private List<Vector3> CalculateNormals(List<Vector3> vertices, List<int> indices, MeshTopology topology)    
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
                            vertices[indices[i + _data.CurrentField.SampleCount + 1]],
                            vertices[indices[i + _data.CurrentField.SampleCount]]
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
        private Vector3 CalculateTriangleNormal(Vector3 p1, Vector3 p2, Vector3 p3)
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
        private Vector3 CalculateQuadNormal(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
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