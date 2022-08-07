using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

public class AdvancedSingleStreamProceduralMesh : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }
    
    void OnEnable ()
    {
        var vertexAttributeCount = 4;
        var vertexCount = 4;
        var triangleIndexCount = 6;
        
        var meshDataArray = Mesh.AllocateWritableMeshData(1);
        var meshData = meshDataArray[0];

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
            vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory
            );

        // Set different attributes to different streams
        vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, dimension: 3);
        vertexAttributes[1] = new VertexAttributeDescriptor(
            VertexAttribute.Normal, dimension: 3
        );
        vertexAttributes[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension: 4
        );
        vertexAttributes[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension: 2
        );
        
        
        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        NativeArray<Vertex> vertices = meshData.GetVertexData<Vertex>();
        
        float h0 = 0f, h1 = 1f;

        var vertex = new Vertex {
            normal = new float3(0f, 0f, -1f),
            tangent = new float4(h1, h0, h0, -1f)
        };

        vertex.position = 0f;
        vertex.texCoord0 = h0;
        vertices[0] = vertex;

        vertex.position = new float3(1f, 0f, 0f);
        vertex.texCoord0 = new float2(h1, h0);
        vertices[1] = vertex;

        vertex.position = new float3(0f, 1f, 0f);
        vertex.texCoord0 = new float2(h0, h1);
        vertices[2] = vertex;

        vertex.position = new float3(1f, 1f, 0f);
        vertex.texCoord0 = h1;
        vertices[3] = vertex;
        
        meshData.SetIndexBufferParams(triangleIndexCount, IndexFormat.UInt32);

        var triangleIndices = meshData.GetIndexData<uint>();
        triangleIndices[0] = 0;
        triangleIndices[1] = 2;
        triangleIndices[2] = 1;
        triangleIndices[3] = 1;
        triangleIndices[4] = 2;
        triangleIndices[5] = 3;
        
        
        // Set submesh
        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndexCount)
        {
            bounds = new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f)),
            vertexCount = vertexCount
        }, MeshUpdateFlags.DontRecalculateBounds);
        
        
        
        
        var mesh = new Mesh {
            bounds = new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f)),
            name = "MultiStream Procedural Mesh"
        };
        
        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
