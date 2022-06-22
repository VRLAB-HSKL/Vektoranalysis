using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralMesh.Streams
{
    public struct SingleMeshStream : IMeshStreams
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Stream0
        {
            public float3 position;
            public float3 normal;
            public float4 tangent;
            public float2 texCoord0;
        }

        [NativeDisableContainerSafetyRestriction]
        private NativeArray<Stream0> stream0;
        
        [NativeDisableContainerSafetyRestriction]
        public NativeArray<TriangleUInt16> triangles;

        public void Setup(Mesh.MeshData meshData, Bounds bounds, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(
                4, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );
            
            // Set different attributes to different streams
            descriptor[0] = new VertexAttributeDescriptor(VertexAttribute.Position, dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(
                VertexAttribute.Normal, dimension: 3
            );
            descriptor[2] = new VertexAttributeDescriptor(
                VertexAttribute.Tangent, dimension: 4
            );
            descriptor[3] = new VertexAttributeDescriptor(
                VertexAttribute.TexCoord0, dimension: 2
            );

            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt16);

            // Set submesh
            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount)
                {
                    bounds = bounds,
                    vertexCount = vertexCount
                }, 
                MeshUpdateFlags.DontRecalculateBounds | MeshUpdateFlags.DontValidateIndices);

            stream0 = meshData.GetVertexData<Stream0>();
            triangles = meshData.GetIndexData<ushort>().Reinterpret<TriangleUInt16>(2);

            //triangles = new NativeArray<int3>();
            // triangles[0] = new int3(0, 2, 1);
            // triangles[1] = new int3(1, 2, 3);

            // Debug.Log("TriangleLength: " + triangles.Length);
            // for (int i = 0; i < triangles.Length; i++)
            // {
            //     Debug.Log("triangle " + i + ": " + triangles[i]);
            // }
            
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex vertex)
        {
            var s0 = new Stream0
            {
                position = vertex.position,
                normal = vertex.normal,
                tangent = vertex.tangent,
                texCoord0 = vertex.texCoord0
            };

            stream0[index] = s0;
        }

        public void SetTriangle(int index, int3 triangle)
        {
            triangles[index] = triangle;
        }

    }
}