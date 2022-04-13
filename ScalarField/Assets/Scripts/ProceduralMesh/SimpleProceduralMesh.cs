using System;
using System.Collections;
using System.Collections.Generic;
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
}
