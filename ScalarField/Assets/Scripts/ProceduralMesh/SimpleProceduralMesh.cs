using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Calculation;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{
    public Transform RenderingTarget;
    public Vector3 PositionOffset;
    public Vector3 ScalingVector = Vector3.one;

    
    
    public Texture2D Texture;
    public BoxCollider Collider; 
    
    // public static class ColorMap
    // {
    //     
    //     
    //     
    // }
    //
    // public static class Sequential {
    //
    //     public enum MultiHue
    //     {
    //         BuGn, BuPu, GnBu, OrRd, PuBu, PuBuGn,
    //         PuRd, RdPu, YlGn, YlGnBu, YlOrBr, YlOrRd    
    //     }
    //
    //     public enum SingleHue
    //     {
    //         Blues, Greens, Greys, Oranges, Purples, Reds
    //     }
    //      
    // }
    //
    // public enum Diverging
    // {
    //     BrBg, PiYG, PRGn, PuOr, RdBu, RdGy, RdYlBu, RdYlGn, Spectral
    // }
    //
    // public enum Qualitative
    // {
    //     Accent, Dark2, Paired, Pastel1, Pastel2, Set1, Set2, Set3
    // }
    //

    private Vector3 initPos = Vector3.zero;

    private void Start()
    {
        initPos = transform.position;
        GenerateFieldMesh();

        var mat = GetComponent<MeshRenderer>().material;
        mat.mainTexture = Texture != null ? Texture : GlobalDataModel.CurrentField.meshTexture;
    }

    // private void OnEnable()
    // {
    //     
    //     return;
    //     
    //     
    // }


    
    
    
    private void GenerateFieldMesh()
    {
        var mesh = new Mesh
        {
            name="Scalar field mesh",
        };
        
        // var sf = new ScalarField()
        // {
        //     parameterRangeX = new Tuple<float, float>(-2f, 2f),
        //     parameterRangeY = new Tuple<float, float>(-2f, 2f),
        //     CalculatePoint = delegate(float x, float y)
        //     {
        //         var r = Mathf.Sqrt(x * x + y * y);
        //         return Mathf.Exp(-r) * Mathf.Cos(6f * r);
        //     }
        // };
        //
        
        //var (rawVertices, displayVertices) = LocalCalc.CalculateField(sf, ScalingVector);

        var sf = GlobalDataModel.CurrentField;

        var rawVertices = sf.pointVectors;
        var displayVertices = new List<Vector3>();
        
        for (var i = 0; i < rawVertices.Count; i++)
        {
            var rawVector = rawVertices[i];
            
            // Switch y and z axis to create horizontal mesh
            var displayVector = new Vector3(rawVector[0], rawVector[2], rawVector[1]);
            
            // ToDo: Replace this static process by dynamically scaling final mesh until it fits certain bounds
            // Scale points based on set scaling vector
            displayVector = Vector3.Scale(displayVector, ScalingVector);
            
            displayVertices.Add(displayVector);
        }
        
        
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

        switch (topology)
        {
            default:
            case MeshTopology.Triangles:
                indices = GenerateTriangleIndices(displayVertices, true);
                // Draw triangles twice to cover both sides
                var backIndices = GenerateTriangleIndices(displayVertices, false);
                displayVertices.AddRange(displayVertices);
                indices.AddRange(backIndices);
                break;
            
            case MeshTopology.Lines:
                indices = GenerateLineIndices(displayVertices);
                break;
        }
            
        mesh.vertices = displayVertices.ToArray();
        
        // var triangles = new List<int>();
        // for (int i = 0; i < vertices.Count; i+= 3)
        // {
        //     triangles.Add(i);
        //     triangles.Add(i + 1);
        //     triangles.Add(i + 2);
        // }

        //mesh.triangles = triangles.ToArray();

        // for (int i = 0; i < indices.Count; i++)
        // {
        //     sb.AppendLine(indices[i].ToString());
        // }
        //
        // if (log)
        // {
        //     Debug.Log("Indices list:" + indices.Count +"\n" + sb);
        // }
        // sb.Clear();
        
        //mesh.triangles = indices.ToArray();

        
        mesh.SetIndices(indices.ToArray(), topology, 0, true);


        // // flip normals on duplicated vertices
        // int normalsHalf = (int)math.floor(normals.Count * 0.5f);
        // for (int i = normalsHalf; i < normals.Count; i++)
        // {
        //     normals[i] *= -1;
        // }

        var normals = CalculateNormals(displayVertices, indices, MeshTopology.Triangles);
        // if (log)
        // {
        //     Debug.Log("Normals:" + normals.Count);
        // }
        
        mesh.normals = normals.ToArray();

        // var colorList = ColorTransferFunction(displayVertices); //GenerateColors(vertices);
        // mesh.colors = colorList.ToArray();

        
        // mesh.colors = new[]
        // {
        //     Color.red, Color.green, Color.blue
        // };

        Vector2[] uvs = new Vector2[displayVertices.Count];


        var step = 1f / displayVertices.Count;

        var x_min = rawVertices.Min(v => v.x);
        var x_max = rawVertices.Max(v => v.x);
        var y_min = rawVertices.Min(v => v.y);
        var y_max = rawVertices.Max(v => v.y);
        var z_min = rawVertices.Min(v => v.z);
        var z_max = rawVertices.Max(v => v.z);    
        
        var x_range = Mathf.Abs(x_max - x_min);
        var y_range = Mathf.Abs(y_max - y_min);
        var z_range = Mathf.Abs(z_max - z_min);
        
        for (int i = 0; i < rawVertices.Count; i++)
        {
            var vertex = rawVertices[i];
            var x = vertex.x + x_min;
            var y = vertex.y + y_min;
            var z = vertex.z - z_min;
            
            var x_factor = x / x_range;
            var y_factor = y / y_range;
            var z_factor = z / z_range;
            
            var uv_vec = new Vector2(y_factor, x_factor); //new Vector2(vertices[i].x, vertices[i].z);
            //Debug.Log("uv_vec: " + uv_vec);
            uvs[i] = uv_vec;
        }
        mesh.uv = uvs;
        
        
        var lineIndices = new List<int>();
        
        //mesh.SetIndices(new int[] {0,1, 2,3, 4,5 }, MeshTopology.Lines, 0, true); 
        
        GetComponent<MeshFilter>().mesh = mesh;

        // Move mesh to target destination
        
        // # Get mesh bounds
        var bounds = GetComponent<MeshFilter>().mesh.bounds;
        
        // # Create target vector
        var target = RenderingTarget.position + PositionOffset;
        
        // # Transform center of mesh position to global scope
        //var transformedPoint = transform.TransformPoint(bounds.center);
        
        // Calculate offset between initial position and mesh center
        //var offset = initPos - transformedPoint;

        //offset = new Vector3(offset.x, offset.y + 0.25f, offset.z);
        
        // Debug.Log(
        //     "initPos: " + initPos + ", target: " + target + ", bounds: " + bounds.center + 
        //     ", transformedPoint: " + transformedPoint + ", offset: " + offset
        // );
        
        // Position mesh based on offset to position 
        transform.position = target; // + offset;
        
        var collider = GetComponent<MeshCollider>();
        //collider.convex = true;
        collider.sharedMesh = mesh;

        //var plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        //plane.transform.position = transform.position;
        //plane.transform.localScale = Vector3.Scale(plane.transform.localScale, ScalingVector);

    }
    
    private void OrderVerticesTriangle(List<Vector3> vertices)
    {
        int n = (int)math.floor(vertices.Count * 0.5); 
        // Order vertices for triangles
        var orderedVertices = new List<Vector3>();
        for (int i = 0; i < vertices.Count; i++)
        {
            // Lower left triangle
            bool isUpperBound = i % n == (n - 1);
            bool isLastColumn = i >= (n * n - n);
            bool drawLowerLeftTriangle = !isLastColumn && !isUpperBound;
            if (drawLowerLeftTriangle) 
            {
                var firstIndex = i;
                var secondIndex = i + 1;
                var thirdIndex = i + n;
        
                // var str = "LowerFirst: [" + firstIndex + "/" + vertices.Count + "]\n" +
                //           "LowerSecond: [" + secondIndex + "/" + vertices.Count + "]\n" +
                //           "LowerThird: [" + thirdIndex + "/" + vertices.Count + "]";
                //
                // Debug.Log(str);
                
                orderedVertices.Add(vertices[firstIndex]);
                orderedVertices.Add(vertices[secondIndex]);
                orderedVertices.Add(vertices[thirdIndex]);
            }
            
            // Upper right triangle
            bool isLowerBound = i % n == 0;
            bool isFirstColumn = i < n;
            bool drawUpperRightTriangle = !isLowerBound && !isFirstColumn;
        
            if (i == (n * n - 1))
            {
                Debug.Log("isLower:" + isLowerBound);
                Debug.Log("isFirstColumn:" + isFirstColumn);
                Debug.Log("drawUpper:" + drawUpperRightTriangle);
            }
            
            if (drawUpperRightTriangle)
            {
                var firstIndex = i - 1;
                var secondIndex = i - n;
                var thirdIndex = i;
                
                // var str =  "UpperFirst: [" + firstIndex + "/" + vertices.Count + "]\n" +
                //                 "UpperSecond: [" + secondIndex + "/" + vertices.Count + "]\n" +
                //                 "UpperThird: [" + thirdIndex + "/" + vertices.Count + "]";
                //
                // Debug.Log(str);
                
                orderedVertices.Add(vertices[firstIndex]);
                orderedVertices.Add(vertices[secondIndex]);
                orderedVertices.Add(vertices[thirdIndex]);
            }
        
        }
        
    }

    private List<int> GenerateTriangleIndices(List<Vector3> vertices, bool windClockWise = true)
    {
        var indicesList = new List<int>();

        for (int i = 0; i < vertices.Count; i++)
        {
            bool isUpperBound = i % GlobalDataModel.NumberOfSamples == (GlobalDataModel.NumberOfSamples - 1);
            bool isLastColumn = i >= (GlobalDataModel.NumberOfSamples * GlobalDataModel.NumberOfSamples - GlobalDataModel.NumberOfSamples);
            bool drawLowerLeftTriangle = !isLastColumn && !isUpperBound;

            if (drawLowerLeftTriangle)
            {
                var firstIndex = i;
                var secondIndex = i + 1;
                var thirdIndex = i + GlobalDataModel.NumberOfSamples;
                
                // var str = "LowerFirst: [" + firstIndex + "/" + vertices.Count + "]\n" +
                //           "LowerSecond: [" + secondIndex + "/" + vertices.Count + "]\n" +
                //           "LowerThird: [" + thirdIndex + "/" + vertices.Count + "]";
                //
                // Debug.Log(str);
                
                // Bottom left triangle

                if (windClockWise)
                {
                    indicesList.Add(firstIndex);
                    indicesList.Add(secondIndex);
                    indicesList.Add(thirdIndex);    
                }
                else
                {
                    indicesList.Add(firstIndex);
                    indicesList.Add(thirdIndex);
                    indicesList.Add(secondIndex);
                }
                    
            }

            bool isLowerBound = i % GlobalDataModel.NumberOfSamples == 0;
            bool isFirstColumn = i < GlobalDataModel.NumberOfSamples;
            bool drawUpperRightTriangle = !isLowerBound && !isFirstColumn;

            if (drawUpperRightTriangle)
            {
                var firstIndex = i - 1;
                var secondIndex = i - GlobalDataModel.NumberOfSamples;
                var thirdIndex = i;
                
                // var str =  "UpperFirst: [" + firstIndex + "/" + vertices.Count + "]\n" +
                //                 "UpperSecond: [" + secondIndex + "/" + vertices.Count + "]\n" +
                //                 "UpperThird: [" + thirdIndex + "/" + vertices.Count + "]";
                //
                // Debug.Log(str);
                
                // Upper right triangle

                if (windClockWise)
                {    
                    indicesList.Add(firstIndex);
                    indicesList.Add(secondIndex);
                    indicesList.Add(thirdIndex);    
                }
                else
                {
                    indicesList.Add(firstIndex);
                    indicesList.Add(thirdIndex);
                    indicesList.Add(secondIndex);
                }
                    
            }
        }

        return indicesList;
    }

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
            indices.Add(i + GlobalDataModel.NumberOfSamples + 1);
            
            // Right
            indices.Add(i + GlobalDataModel.NumberOfSamples + 1);
            indices.Add(i + GlobalDataModel.NumberOfSamples);
            
            // Down
            indices.Add(i + GlobalDataModel.NumberOfSamples);
            indices.Add(i);
        }

        return indices;
    }
    
    private List<Vector3> CalculateNormals(List<Vector3> vertices, List<int> indices, MeshTopology topology)    
    {    
        var normals = new List<Vector3>();

        for (int i = 0; i < vertices.Count; i++)
        {
            normals.Add(Vector3.zero);
        }

        switch (topology)
        {
            case MeshTopology.Triangles:
                for (int i = 0; i < indices.Count; i+=3)
                {
                    var normal = CalculateTriangleNormal(
                        vertices[indices[i]],
                        vertices[indices[i + 1]],
                        vertices[indices[i + 2]]
                    );


                    normals[indices[i]] = (normals[indices[i]] + normal).normalized;
                    normals[indices[i + 1]] = (normals[indices[i + 1]] + normal).normalized;
                    normals[indices[i + 2]] = (normals[indices[i + 2]] + normal).normalized;
            
                    // // ToDo: Interpolate normals
                    // normals.Add(normal);
                    // normals.Add(normal);
                    // normals.Add(normal);
                }

                break;
                
            case MeshTopology.Lines:
                for (int i = 0; i < indices.Count; i++)
                {
                    var normal = CalculateQuadNormal(
                        vertices[indices[i]],
                        vertices[indices[i + 1]],
                        vertices[indices[i + GlobalDataModel.NumberOfSamples + 1]],
                        vertices[indices[i + GlobalDataModel.NumberOfSamples]]
                    );
                }

                break;
        }
        
        // Debug.Log("vertices: " + vertices.Count + " normals: " + normals.Count);

        return normals;

    }
    
    
    
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

    private List<Color> GenerateColors(List<Vector3> vertices)
    {
        var maxz = vertices.Max(v => v.z);
        var minz = vertices.Min(v => v.z);
        var rangeZ = math.abs(maxz - minz);
        //var step = rangeZ / 255f;
        var colorList = new List<Color>();
        
        for (int i = 0; i < vertices.Count; i++)
        {
            var z = vertices[i].z;

            if (z > 0)
            {
                var redVal = z / maxz; //rangeZ;
                colorList.Add(new Color(redVal, 0f, 0f));    
            }
            else
            {
                var blueVal = z / minz;
                colorList.Add(new Color(0f, 0f, blueVal));
            }
            
        }

        return colorList;
    }


    private Color[] colors = new[]
    {
        Color.black,
        Color.blue,
        Color.magenta,
        Color.red,
        Color.white
    };

    private List<Color> ColorTransferFunction(List<Vector3> vertices)
    {
        var maxz = vertices.Max(v => v.z);
        var minz = vertices.Min(v => v.z);
        
        //Debug.Log("min: " + minz + ", max: " + maxz);
        
        var rangeZ = math.abs(maxz - minz);
        //var step = rangeZ / 255f;
        var colorList = new List<Color>();

        var n = colors.Length;

        for (int i = 0; i < vertices.Count; i++)
        {
            var z = vertices[i].z;
            float raw_ts = ((z - minz) / (maxz - minz)) * (n - 1);
            int ts = (int) raw_ts;
            colorList.Add(colors[ts]);
        }

        return colorList;
    }
    
}
