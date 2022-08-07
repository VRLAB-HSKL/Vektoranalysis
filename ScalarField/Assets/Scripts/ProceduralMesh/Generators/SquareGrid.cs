using System;
using ProceduralMesh.Streams;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMesh.Generators
{
    public struct SquareGrid : IMeshGenerator
    {
        public int VertexCount => 4 * Resolution * Resolution;
        public int IndexCount => 6 * Resolution * Resolution;
        public int JobLength => Resolution * Resolution;

        public Bounds Bounds => new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));

        public int Resolution { get; set; }
        
        public void Execute<TS>(int i, TS streams) where TS : struct, IMeshStreams
        {
            // vertex index
            int vi = 4 * i;
            
            // triangle index
            int ti = 2 * i;

            int y = i / Resolution;
            int x = i - Resolution * y;

            var coordinates = new float4(x, x + 0.9f, y, y + 0.9f);
            
            var vertex = new Vertex();
            vertex.position.xy = coordinates.xz;
            streams.SetVertex(vi + 0, vertex);

            vertex.position.xy = coordinates.yz;
            vertex.texCoord0 = new float2(1f, 0f);
            streams.SetVertex(vi + 1, vertex);

            vertex.position.xy = coordinates.xw;
            vertex.texCoord0 = new float2(0f, 1f);
            streams.SetVertex(vi + 2, vertex);

            vertex.position.xy = coordinates.yw;
            vertex.texCoord0 = new float2(1f, 1f);
            streams.SetVertex(vi + 3, vertex);
            
            streams.SetTriangle(ti + 0, vi + new int3(0, 2, 1));
            streams.SetTriangle(ti + 1, vi + new int3(1, 2, 3));
            
            
            
            // 
            // vertex.normal.z = -1f;
            // vertex.tangent.xw = new float2(1f, -1f);
            //
            // streams.SetVertex(0, vertex);
            //
            // vertex.position = new float3(1f, 0f, 0f);
            // vertex.texCoord0 = new float2(1f, 0f);
            // streams.SetVertex(1, vertex);
            //
            // vertex.position = new float3(0f, 1f, 0f);
            // vertex.texCoord0 = new float2(0f, 1f);
            // streams.SetVertex(2, vertex);
            //
            // vertex.position = new float3(1f, 1f, 0f);
            // vertex.texCoord0 = new float2(1f, 1f);
            // streams.SetVertex(3, vertex);
            //
            // streams.SetTriangle(0, new int3(0, 2, 1));
            // streams.SetTriangle(1, new int3(1, 2, 3));
        }
    }
}