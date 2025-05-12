using GenerationDemo.Core.DTOs;
using UnityEngine;

namespace GenerationDemo.Core.Interfaces
{
    public delegate void ChunkGeneratedCallback(VertexDTO[] vertices, int[] indices);

    public interface IChunkGenerator
    {
        event ChunkGeneratedCallback ChunkGenerated;

        void Generate(VertexDTO[] vertices, int[] indices, Vector2 offset);
    }
}
