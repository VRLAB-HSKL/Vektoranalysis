using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SimpleProceduralMesh : MonoBehaviour
{
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
    
    
    private void OnEnable()
    {
        CustomCalc();
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
    
    private void CustomCalc()
    {
        var mesh = new Mesh
        {
            name="Procedural Mesh"
        };
        
        const int n = 100;

        var x_lower = -5f * math.PI;
        var x_upper = 5f * math.PI;
        var x_range = x_upper - x_lower;
        var x_step = x_range / n;

        var y_lower = -5f * math.PI;
        var y_upper = 5f * math.PI;
        var y_range = y_upper - y_lower;
        var y_step = y_range / n;
        
        var vertices = new List<Vector3>();

        for(int i = 0; i < n; i++)
        {
            var x = i * x_step;
            float y = 0f;
            for (int j = 0; j < n; j++)
            {
                y = j * y_step;    
                var z = -math.sin(x) * math.sin(y);
            
                vertices.Add(new Vector3(x, y, z));
            }

        }

        var sb = new StringBuilder();
        for (int i = 0; i < vertices.Count; i++)
        {
            sb.AppendLine(vertices[i].ToString());
        }
        
        Debug.Log("Init vertices List:\n" + sb);
        sb.Clear();

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

        
        for (int i = 0; i < orderedVertices.Count; i++)
        {
            sb.AppendLine(orderedVertices[i].ToString());
        }
        
        Debug.Log("Ordered vertices List:" + orderedVertices.Count +"\n" + sb);
        
        // // Double vertices
        // orderedVertices.AddRange(orderedVertices);
        //
        
        mesh.vertices = orderedVertices.ToArray();

        
        var triangles = new List<int>();
        for (int i = 0; i < orderedVertices.Count; i+= 3)
        {
            triangles.Add(i);
            triangles.Add(i + 1);
            triangles.Add(i + 2);
        }

        mesh.triangles = triangles.ToArray();

        var normals = new List<Vector3>();
        for (int i = 0; i < triangles.Count; i+=3)
        {
            var normal = CalculateTriangleNormal(
                orderedVertices[triangles[i]],
                orderedVertices[triangles[i + 1]],
                orderedVertices[triangles[i + 2]]
            );
            
            normals.Add(normal);
            normals.Add(normal);
            normals.Add(normal);
        }

        // // flip normals on duplicated vertices
        // int normalsHalf = (int)math.floor(normals.Count * 0.5f);
        // for (int i = normalsHalf; i < normals.Count; i++)
        // {
        //     normals[i] *= -1;
        // }

        
        Debug.Log("Normals:" + normals.Count);
        
        mesh.normals = normals.ToArray();

        var colorList = new List<Color>();

        var maxz = orderedVertices.Max(v => v.z);
        var minz = orderedVertices.Min(v => v.z);
        var rangeZ = math.abs(maxz - minz);
        //var step = rangeZ / 255f;
        
        for (int i = 0; i < orderedVertices.Count; i++)
        {
            var z = orderedVertices[i].z;

            if (z > 0)
            {
                colorList.Add(new Color(z / rangeZ, 0f, 0f));    
            }
            else
            {
                colorList.Add(new Color(0f, 0f, math.abs(z / rangeZ)));
            }
            
        }

        mesh.colors = colorList.ToArray();

        // mesh.colors = new[]
        // {
        //     Color.red, Color.green, Color.blue
        // };
        
        
        GetComponent<MeshFilter>().mesh = mesh;

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
}
