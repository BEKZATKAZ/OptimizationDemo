using GenerationDemo.Core.DTOs;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace GenerationDemo.Core.UseCases
{
    public sealed class StandardChunkAroundUseCase : ChunkAroundUseCaseBase
    {
        public StandardChunkAroundUseCase(
            IChunkGenerator generator,
            float size,
            int resolution,
            int chunkRange) : base(size, chunkRange)
        {
            _generator = generator;
            _vertexCount = GenerationUtilities.GetVertexCount(resolution);
            _indexCount = GenerationUtilities.GetIndexCount(resolution);
            _activeChunks = new Dictionary<Vector2Int, bool>();
        }

        private readonly IChunkGenerator _generator;
        private readonly int _vertexCount;
        private readonly int _indexCount;
        private readonly Dictionary<Vector2Int, bool> _activeChunks;

        protected override bool CanPerform(Vector2Int coords)
        {
            return _activeChunks.ContainsKey(coords) == false;
        }

        protected override void Performed(Vector2Int coords)
        {
            VertexDTO[] vertices = new VertexDTO[_vertexCount];
            int[] indices = new int[_indexCount];

            _generator.Generate(vertices, indices, coords);
            _activeChunks.Add(coords, true);
        }
    }
}
