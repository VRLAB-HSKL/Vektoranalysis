using System;
using ProceduralMesh;
using ProceduralMesh.Generators;
using ProceduralMesh.Streams;
using Unity.Collections;
using UnityEngine;

namespace DefaultNamespace
{
    
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class ProceduralMesh: MonoBehaviour
    {
        [SerializeField, Range(1, 10)]
        public int resolution = 1;
        
        private Mesh mesh;

        private void Awake()
        {
            mesh = new Mesh
            {
                name = "Procedural Mesh"
            };
            //GenerateMesh();
            GetComponent<MeshFilter>().mesh = mesh;
        }

        private void OnValidate()
        {
            enabled = true;
        }

        private void Uppdate()
        {
            GenerateMesh();
            enabled = false;
        }

        private void GenerateMesh()
        {
            var meshDataArray = Mesh.AllocateWritableMeshData(1);
            var meshData = meshDataArray[0];
            
            MeshJob<SquareGrid, MultiMeshStream>.ScheduleParallel(
                mesh, meshData, resolution, default).Complete();
            
            var idxArray = new NativeArray<int>(6, Allocator.Temp);
            var vertexArray = new NativeArray<Vector3>(4, Allocator.Temp);
            
            meshData.GetIndices(idxArray, 0);
            meshData.GetVertices(vertexArray);
            
            // Debug.Log("Index count: " + idxArray.Length);
            // for (int i = 0; i < idxArray.Length; i++)
            // {
            //     Debug.Log("Index " + i + ": " + idxArray[i]);
            // }
            //
            // Debug.Log("Vertex count: " + vertexArray.Length);
            // for (int i = 0; i < vertexArray.Length; i++)
            // {
            //     Debug.Log("Vertex " + i + ": " + vertexArray[i]);
            // }
            
            Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        }

        private void GenerateCustomMesh()
        {
            
        }
    }
}