using UnityEngine;

namespace ProceduralMesh.Generators
{
    public class CustomMeshGenerator : IMeshGenerator
    {
        public int VertexCount { get; }
        public int IndexCount { get; }
        public int JobLength { get; }
        public Bounds Bounds { get; }
        public int Resolution { get; set; }
        public void Execute<TS>(int i, TS streams) where TS : struct, IMeshStreams
        {
            throw new System.NotImplementedException();
            
            
            
            
        }
    }
}