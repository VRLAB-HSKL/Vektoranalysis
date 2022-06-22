using ProceduralMesh.Generators;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace ProceduralMesh
{
    [BurstCompile(FloatPrecision.Standard, FloatMode.Fast, CompileSynchronously = true)]
    public struct MeshJob<TG, TS> : IJobFor
        where TG : struct, IMeshGenerator
        where TS : struct, IMeshStreams
    {
        private TG generator;
        
        [WriteOnly]
        private TS streams;
        
        
        public void Execute(int index)
        {
            generator.Execute(index, streams);
            
        }

        public static JobHandle ScheduleParallel(Mesh mesh, Mesh.MeshData meshData, 
            int resolution, JobHandle dependency)
        {
            var job = new MeshJob<TG, TS>();
            job.generator.Resolution = resolution;
            job.streams.Setup(
                meshData, job.generator.Bounds, 
                job.generator.VertexCount, job.generator.IndexCount
                );

            return job.ScheduleParallel(job.generator.JobLength, 1, dependency);
            //return MeshJob<TG, TS>.ScheduleParallel(job.generator.JobLength, 1, dependency);
        }
    }
}