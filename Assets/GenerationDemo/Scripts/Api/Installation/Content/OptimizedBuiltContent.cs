using GenerationDemo.Api.Installation.Abstractions;
using GenerationDemo.Api.Placers;
using GenerationDemo.Core;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Data.Services;

namespace GenerationDemo.Api.Installation.Content
{
    public sealed class OptimizedBuiltContent : BuiltContentBase
    {
        public OptimizedBuiltContent(
            IChunkAroundUseCase generatorUseCase,
            OptimizedChunkGenerator generator,
            OptimizedChunkPlacer placer,
            IObjectPool<ChunkObject> chunkPool,
            IObjectPool<ChunkBuffer> chunkBufferPool) : base(generatorUseCase)
        {
            _generator = generator;
            _placer = placer;
            _chunkPool = chunkPool;
            _chunkBufferPool = chunkBufferPool;
        }

        private readonly OptimizedChunkGenerator _generator;
        private readonly OptimizedChunkPlacer _placer;
        private readonly IObjectPool<ChunkObject> _chunkPool;
        private readonly IObjectPool<ChunkBuffer> _chunkBufferPool;

        public override void Dispose()
        {
            if (_placer) UnityEngine.Object.Destroy(_placer);

            _generator.Dispose();
            _chunkPool.Dispose();
            _chunkBufferPool.Dispose();

            base.Dispose();
        }
    }
}
