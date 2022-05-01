using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{

    public Material SpawnPointMat;
    
    public static class ColorMap
    {
        
        
        
    }
    
    public static class Sequential {

        public enum MultiHue
        {
            BuGn, BuPu, GnBu, OrRd, PuBu, PuBuGn,
            PuRd, RdPu, YlGn, YlGnBu, YlOrBr, YlOrRd    
        }

        public enum SingleHue
        {
            Blues, Greens, Greys, Oranges, Purples, Reds
        }
         
    }

    public enum Diverging
    {
        BrBg, PiYG, PRGn, PuOr, RdBu, RdGy, RdYlBu, RdYlGn, Spectral
    }

    public enum Qualitative
    {
        Accent, Dark2, Paired, Pastel1, Pastel2, Set1, Set2, Set3
    }


    private Vector3 initPos = Vector3.zero;

    private void Awake()
    {
        initPos = transform.position;
        Debug.Log("initPos: " + initPos);
        CustomCalc();
    }

    private void OnEnable()
    {
        
        return;
        
        var mesh = new Mesh
        {
            name="Procedural Mesh"
        };

        
        
        
        mesh.vertices = new[]
        {
            Vector3.zero, Vector3.right, Vector3.up, new Vector3(1f, 1f)
            //new Vector3(1.1f, 0f), new Vector3(0f, 1.1f), new Vector3(1.1f, 1.1f)
        };

        
        
        mesh.triangles = new[]
        {
            0, 2, 1, 1, 2, 3
        };

        
        
        
        
        var normals = new List<Vector3>();
        
        
        mesh.normals = new[]
        {
            Vector3.back, Vector3.back, Vector3.back, Vector3.back 
        };

        mesh.uv = new[]
        {
            Vector2.zero, Vector2.right, Vector2.up, Vector2.one
            
        };

        // mesh.colors = new[]
        // {
        //     Color.red, Color.green, Color.blue
        // };

        mesh.tangents = new[]
        {
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f)
        };
        
        GetComponent<MeshFilter>().mesh = mesh;
    }

    
    
    public int NumerOfSamples = 100;
    
    
    
    private void CustomCalc()
    {
        var mesh = new Mesh
        {
            name="Procedural Mesh",
        };
        
        //const int n = 100;

        var x_lower = -5f * math.PI;
        var x_upper = 5f * math.PI;
        var x_range = x_upper - x_lower;
        var x_step = x_range / NumerOfSamples;

        var y_lower = -5f * math.PI;
        var y_upper = 5f * math.PI;
        var y_range = y_upper - y_lower;
        var y_step = y_range / NumerOfSamples;
        
        var vertices = new List<Vector3>();

        for(int i = 0; i < NumerOfSamples; i++)
        {
            var x = i * x_step;
            float y = 0f;
            for (int j = 0; j < NumerOfSamples; j++)
            {
                y = j * y_step;    
                var z = -math.sin(x) * math.sin(y);
            
                vertices.Add(new Vector3(x, y, z));
            }

        }


        var log = false;
        var sb = new StringBuilder();
        for (int i = 0; i < vertices.Count; i++)
        {
            sb.AppendLine(vertices[i].ToString());
        }

        if (log)
        {
            Debug.Log("Init vertices List:" + vertices.Count + "\n" + sb);
        }
            
        sb.Clear();

        
        // for (int i = 0; i < vertices.Count; i++)
        // {
        //     sb.AppendLine(vertices[i].ToString());
        // }
        //
        // Debug.Log("Ordered vertices List:" + vertices.Count +"\n" + sb);
        
        // // Double vertices
        // orderedVertices.AddRange(orderedVertices);
        //
        
        

        var topology = MeshTopology.Triangles;


        var indices = new List<int>();

        switch (topology)
        {
            default:
            case MeshTopology.Triangles:
                indices = GenerateTriangleIndices(vertices);
                var backIndices = GenerateTriangleIndices(vertices, false);
                vertices.AddRange(vertices);
                indices.AddRange(backIndices);
                break;
            
            case MeshTopology.Lines:
                indices = GenerateLineIndices(vertices);
                break;
        }
            
        mesh.vertices = vertices.ToArray();
        
        // var triangles = new List<int>();
        // for (int i = 0; i < vertices.Count; i+= 3)
        // {
        //     triangles.Add(i);
        //     triangles.Add(i + 1);
        //     triangles.Add(i + 2);
        // }

        //mesh.triangles = triangles.ToArray();

        for (int i = 0; i < indices.Count; i++)
        {
            sb.AppendLine(indices[i].ToString());
        }

        if (log)
        {
            Debug.Log("Indices list:" + indices.Count +"\n" + sb);
        }
        sb.Clear();
        
        //mesh.triangles = indices.ToArray();

        
        mesh.SetIndices(indices.ToArray(), topology, 0, true);



        
        

        // // flip normals on duplicated vertices
        // int normalsHalf = (int)math.floor(normals.Count * 0.5f);
        // for (int i = normalsHalf; i < normals.Count; i++)
        // {
        //     normals[i] *= -1;
        // }

        var normals = CalculateNormals(vertices, indices, MeshTopology.Triangles);
        if (log)
        {
            Debug.Log("Normals:" + normals.Count);
        }
        
        mesh.normals = normals.ToArray();

        var colorList = GenerateColors(vertices);
        mesh.colors = colorList.ToArray();

        // mesh.colors = new[]
        // {
        //     Color.red, Color.green, Color.blue
        // };

        var lineIndices = new List<int>();
        
        //mesh.SetIndices(new int[] {0,1, 2,3, 4,5 }, MeshTopology.Lines, 0, true); 
        
        GetComponent<MeshFilter>().mesh = mesh;

        // Move mesh to target destination
        //var tfp = transform.position;
        var bounds = GetComponent<MeshFilter>().mesh.bounds;
        var target = new Vector3(0f, 1.2f, 0f);
        var transformedPoint = transform.TransformPoint(bounds.center);
        var offset = initPos - transformedPoint;
        
        Debug.Log(
            "initPos: " + initPos + ", target: " + target + ", bounds: " + bounds.center + 
            ", transformedPoint: " + transformedPoint + ", offset: " + offset
        );
        
        transform.position = target + offset;

        var collider = GetComponent<MeshCollider>();
        collider.convex = true;
        collider.sharedMesh = mesh;

    }

    private void OnCollisionEnter(Collision collision)
    {
        var contact = collision.GetContact(0);

        var sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.position = contact.point;
        sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        //Instantiate(new SphereCollider)
        sphere.GetComponent<MeshRenderer>().sharedMaterial = SpawnPointMat;
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
            bool isUpperBound = i % NumerOfSamples == (NumerOfSamples - 1);
            bool isLastColumn = i >= (NumerOfSamples * NumerOfSamples - NumerOfSamples);
            bool drawLowerLeftTriangle = !isLastColumn && !isUpperBound;

            if (drawLowerLeftTriangle)
            {
                var firstIndex = i;
                var secondIndex = i + 1;
                var thirdIndex = i + NumerOfSamples;
                
                // var str = "LowerFirst: [" + firstIndex + "/" + vertices.Count + "]\n" +
                //           "LowerSecond: [" + secondIndex + "/" + vertices.Count + "]\n" +
                //           "LowerThird: [" + thirdIndex + "/" + vertices.Count + "]";
                //
                // Debug.Log(str);
                
                // Bottom left triangle

                if (windClockWise)
                {
                    indicesList.Add(i);
                    indicesList.Add(i + 1);
                    indicesList.Add(i + NumerOfSamples);    
                }
                else
                {
                    indicesList.Add(i);
                    indicesList.Add(i + NumerOfSamples);
                    indicesList.Add(i + 1);
                }
                    
            }

            bool isLowerBound = i % NumerOfSamples == 0;
            bool isFirstColumn = i < NumerOfSamples;
            bool drawUpperRightTriangle = !isLowerBound && !isFirstColumn;

            if (drawUpperRightTriangle)
            {
                var firstIndex = i - 1;
                var secondIndex = i - NumerOfSamples;
                var thirdIndex = i;
                
                // var str =  "UpperFirst: [" + firstIndex + "/" + vertices.Count + "]\n" +
                //                 "UpperSecond: [" + secondIndex + "/" + vertices.Count + "]\n" +
                //                 "UpperThird: [" + thirdIndex + "/" + vertices.Count + "]";
                //
                // Debug.Log(str);
                
                // Upper right triangle

                if (windClockWise)
                {
                    indicesList.Add(i - 1);
                    indicesList.Add(i - NumerOfSamples);
                    indicesList.Add(i);    
                }
                else
                {
                    indicesList.Add(i - 1);
                    indicesList.Add(i);
                    indicesList.Add(i - NumerOfSamples);
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
            indices.Add(i + NumerOfSamples + 1);
            
            // Right
            indices.Add(i + NumerOfSamples + 1);
            indices.Add(i + NumerOfSamples);
            
            // Down
            indices.Add(i + NumerOfSamples);
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
                        vertices[indices[i + NumerOfSamples + 1]],
                        vertices[indices[i + NumerOfSamples]]
                    );
                }

                break;
        }
        
        Debug.Log("vertices: " + vertices.Count + " normals: " + normals.Count);

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
}
