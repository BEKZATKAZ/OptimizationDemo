using GenerationDemo.Core;
using GenerationDemo.Core.DTOs;
using GenerationDemo.Core.Interfaces;
using UnityEngine;
using UnityEngine.Rendering;

namespace GenerationDemo.Api.Placers
{
    public sealed class OptimizedChunkPlacer : ChunkPlacerBase
    {
        private IObjectPool<ChunkObject> _chunkPool;

        public OptimizedChunkPlacer WithChunkPool(IObjectPool<ChunkObject> pool)
        {
            _chunkPool = pool;
            return this;
        }

        protected override void OnChunkGenerated(VertexDTO[] vertices, int[] indices)
        {
            Mesh mesh = _chunkPool.Get().Mesh;

            mesh.SetVertexBufferParams(vertices.Length,
                new VertexAttributeDescriptor(
                    VertexAttribute.Position, VertexAttributeFormat.Float32, 3),
                new VertexAttributeDescriptor(
                    VertexAttribute.TexCoord0, VertexAttributeFormat.Float32, 2));

            mesh.SetIndexBufferParams(indices.Length, IndexFormat.UInt32);

            mesh.SetVertexBufferData(vertices, 0, 0, vertices.Length);
            mesh.SetIndexBufferData(indices, 0, 0, indices.Length);
            mesh.SetSubMesh(0, new SubMeshDescriptor(0, indices.Length));

            mesh.RecalculateNormals();
            mesh.RecalculateTangents();
            mesh.RecalculateBounds();
        }
    }
}
