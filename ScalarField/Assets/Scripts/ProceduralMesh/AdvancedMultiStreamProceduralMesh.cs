using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AdvancedMultiStreamProceduralMesh : MonoBehaviour {

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
            VertexAttribute.Normal, dimension: 3, stream: 1
        );
        vertexAttributes[2] = new VertexAttributeDescriptor(
            VertexAttribute.Tangent, dimension: 4, stream: 2
        );
        vertexAttributes[3] = new VertexAttributeDescriptor(
            VertexAttribute.TexCoord0, dimension: 2, stream: 3
        );
        
        
        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        var positions = meshData.GetVertexData<float3>();
        positions[0] = 0f;
        positions[1] = new float3(1f, 0f, 0f); // right
        positions[2] = new float3(0f, 1f, 0f); // up
        positions[3] = new float3(1f, 1f, 0f); // one

        var normals = meshData.GetVertexData<float3>(1);
        normals[0] = normals[1] = normals[2] = normals[3] = new float3(0f, 0f, -1f); 
        
        var tangents = meshData.GetVertexData<float4>(2);
        tangents[0] = tangents[1] = tangents[2] = tangents[3] = new float4(1f, 0f, 0f, -1f); 

        var textCoords = meshData.GetVertexData<float2>(3);
        textCoords[0] = 0f;
        textCoords[1] = new float2(1f, 0f);
        textCoords[2] = new float2(0f, 1f);
        textCoords[3] = 1f;
        
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