using GenerationDemo.Core.DTOs;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Shared;
using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace GenerationDemo.Data.Services
{
    public sealed class OptimizedChunkGenerator : IChunkGenerator, IDisposable
    {
        public OptimizedChunkGenerator(GenerationConfig config, ComputeShader computeShader)
        {
            _config = config;
            _computeShader = computeShader;
        }

        public event ChunkGeneratedCallback ChunkGenerated;

        private readonly GenerationConfig _config;
        private readonly ComputeShader _computeShader;

        private Texture2D _erosionTexture;
        private ComputeBuffer _vertexBuffer;
        private ComputeBuffer _indexBuffer;
        private int _mainKernelIndex;
        private int _indexKernelIndex;

        public void Generate(VertexDTO[] vertices, int[] indices, Vector2 offset)
        {
            int threads = 16;
            int threadGroups = _config.Resolution / threads;

            _computeShader.SetVector("Offset", offset);
            _computeShader.Dispatch(_mainKernelIndex, threadGroups, threadGroups, 1);
            _computeShader.Dispatch(_indexKernelIndex, threadGroups, threadGroups, 1);

            _vertexBuffer.GetData(vertices);
            _indexBuffer.GetData(indices);

            ChunkGenerated?.Invoke(vertices, indices);
        }

        public void Initialize()
        {
            int dataSize = Marshal.SizeOf(typeof(VertexDTO));
            int resolution = _config.Resolution;

            int vertexBufferSize = GenerationUtilities.GetVertexCount(resolution);
            int indexBufferSize = GenerationUtilities.GetIndexCount(resolution);

            _mainKernelIndex = _computeShader.FindKernel("CalculatePositionsAndUVs");
            _indexKernelIndex = _computeShader.FindKernel("CalculateIndices");

            _erosionTexture = _config.Erosion.BakeToTexture(2048);
            _vertexBuffer = new ComputeBuffer(vertexBufferSize, dataSize);
            _indexBuffer = new ComputeBuffer(indexBufferSize, sizeof(int));

            _computeShader.SetInt("Resolution", resolution);
            _computeShader.SetFloat("Amplitude", _config.Amplitude);
            _computeShader.SetFloat("Size", _config.Size);
            _computeShader.SetFloat("Frequency", _config.Frequency);

            _computeShader.SetFloat("FrequencyMultiplier", _config.FrequencyMultiplier);
            _computeShader.SetFloat("Persistence", _config.Persistence);
            _computeShader.SetInt("Octaves", _config.Octaves);

            _computeShader.SetTexture(_mainKernelIndex, "Erosion", _erosionTexture);
            _computeShader.SetBuffer(_mainKernelIndex, "VertexBuffer", _vertexBuffer);
            _computeShader.SetBuffer(_indexKernelIndex, "IndexBuffer", _indexBuffer);
        }

        public void Dispose()
        {
            _vertexBuffer.Dispose();
            _indexBuffer.Dispose();
            UnityEngine.Object.Destroy(_erosionTexture);
        }
    }
}
