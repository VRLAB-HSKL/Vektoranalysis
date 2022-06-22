using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMesh
{
    public interface IMeshStreams
    {
        public void Setup(Mesh.MeshData data, Bounds bounds, int vertexCount, int indexCount);

        public void SetVertex(int index, Vertex vertex);
        public void SetTriangle(int index, int3 triangle);
    }
}