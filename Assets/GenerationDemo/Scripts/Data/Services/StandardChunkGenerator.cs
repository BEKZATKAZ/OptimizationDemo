using GenerationDemo.Core.DTOs;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Shared;
using Unity.Mathematics;
using UnityEngine;

namespace GenerationDemo.Data.Services
{
    public sealed class StandardChunkGenerator : IChunkGenerator
    {
        public StandardChunkGenerator(GenerationConfig config) => _config = config;

        public event ChunkGeneratedCallback ChunkGenerated;

        private readonly GenerationConfig _config;

        private int GetIndex(int x, int y) => x + _config.Resolution * y;

        private void CalculateIndices(int[] indices)
        {
            int indexOffset = 0;

            for (int i = 0; i < indices.Length; i++)
            {
                int x = i % _config.Resolution;
                int y = i == 0 ? 0 : i / _config.Resolution;

                if (x >= _config.Resolution - 1 || y >= _config.Resolution - 1) continue;

                indices[indexOffset++] = GetIndex(x + 0, y + 0);
                indices[indexOffset++] = GetIndex(x + 0, y + 1);
                indices[indexOffset++] = GetIndex(x + 1, y + 1);

                indices[indexOffset++] = GetIndex(x + 1, y + 1);
                indices[indexOffset++] = GetIndex(x + 1, y + 0);
                indices[indexOffset++] = GetIndex(x + 0, y + 0);
            }
        }

        private void CalculatePositionsAndUVs(VertexDTO[] vertices, Vector2 offset)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                int xIndex = i % _config.Resolution;
                int zIndex = i == 0 ? 0 : i / _config.Resolution;

                float x = xIndex / (float)_config.Resolution + offset.x;
                float z = zIndex / (float)_config.Resolution + offset.y;

                float2 noiseInput = new float2(x, z) * _config.Frequency;
                float y = NoiseUtilities.OctavedGradientNoise(
                    noiseInput, _config.Octaves, _config.Persistence, _config.FrequencyMultiplier);

                vertices[i].Position = new Vector3(
                    x * _config.Size,
                    y * _config.Amplitude * _config.Erosion.Evaluate(y),
                    z * _config.Size);

                vertices[i].UV = new Vector2(xIndex, zIndex);
            }
        }

        public void Generate(VertexDTO[] vertices, int[] indices, Vector2 offset)
        {
            CalculatePositionsAndUVs(vertices, offset);
            CalculateIndices(indices);
            ChunkGenerated?.Invoke(vertices, indices);
        }
    }
}
