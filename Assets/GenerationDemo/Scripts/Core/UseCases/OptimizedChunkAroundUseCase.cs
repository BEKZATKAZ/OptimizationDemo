using GenerationDemo.Core.Interfaces;
using System.Collections.Generic;
using UnityEngine;

namespace GenerationDemo.Core.UseCases
{
    public sealed class OptimizedChunkAroundUseCase : ChunkAroundUseCaseBase
    {
        public OptimizedChunkAroundUseCase(
            IChunkGenerator generator,
            IObjectPool<ChunkBuffer> bufferPool,
            float size,
            int chunkRange) : base(size, chunkRange)
        {
            _generator = generator;
            _bufferPool = bufferPool;
            _activeChunks = new Dictionary<Vector2Int, ChunkBuffer>();
        }

        private readonly IChunkGenerator _generator;
        private readonly IObjectPool<ChunkBuffer> _bufferPool;
        private readonly Dictionary<Vector2Int, ChunkBuffer> _activeChunks;

        protected override bool CanPerform(Vector2Int coords)
        {
            return _activeChunks.ContainsKey(coords) == false;
        }

        protected override void Performed(Vector2Int coords)
        {
            ChunkBuffer buffer = _bufferPool.Get();
            _generator.Generate(buffer.Vertices, buffer.Indices, coords);
            _activeChunks.Add(coords, buffer);
        }
    }
}
