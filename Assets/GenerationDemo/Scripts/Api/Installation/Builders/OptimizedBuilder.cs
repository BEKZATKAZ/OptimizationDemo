using GenerationDemo.Api.Installation.Abstractions;
using GenerationDemo.Api.Installation.Content;
using GenerationDemo.Api.Placers;
using GenerationDemo.Core;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Core.UseCases;
using GenerationDemo.Data;
using GenerationDemo.Data.Pools;
using GenerationDemo.Data.Services;
using UnityEngine;

namespace GenerationDemo.Api.Installation.Builders
{
    public sealed class OptimizedBuilder : IBuilder
    {
        public OptimizedBuilder(
            GenerationConfig config,
            ComputeShader generationShader,
            ChunkObject chunkPrefab)
        {
            _generationConfig = config;
            _generationShader = generationShader;
            _chunkPrefab = chunkPrefab;
        }

        private readonly GenerationConfig _generationConfig;
        private readonly ComputeShader _generationShader;
        private readonly ChunkObject _chunkPrefab;

        public BuiltContentBase Build()
        {
            IObjectPool<ChunkObject> chunkPool = CreateChunkPool();
            IObjectPool<ChunkBuffer> chunkBufferPool = CreateChunkBufferPool();

            OptimizedChunkGenerator generator = CreateGenerator();
            OptimizedChunkPlacer placer = CreatePlacer(generator, chunkPool);
            OptimizedChunkAroundUseCase useCase = CreateUseCase(generator, chunkBufferPool);

            return new OptimizedBuiltContent(useCase, generator, placer, chunkPool, chunkBufferPool);
        }

        private OptimizedChunkGenerator CreateGenerator()
        {
            OptimizedChunkGenerator generator = new OptimizedChunkGenerator(_generationConfig, _generationShader);
            generator.Initialize();
            return generator;
        }

        private OptimizedChunkPlacer CreatePlacer(
            OptimizedChunkGenerator generator, IObjectPool<ChunkObject> chunkPool)
        {
            GameObject gameObject = new GameObject("OPTIMIZED-PLACER");

            OptimizedChunkPlacer placer = gameObject
                .AddComponent<OptimizedChunkPlacer>()
                .WithChunkPool(chunkPool);

            placer.Initialize(generator);
            return placer;
        }

        private OptimizedChunkAroundUseCase CreateUseCase(
            IChunkGenerator generator, IObjectPool<ChunkBuffer> bufferPool)
        {
            GenerationConfig config = _generationConfig;
            return new OptimizedChunkAroundUseCase(generator, bufferPool, config.Size, config.Range);
        }

        private IObjectPool<ChunkObject> CreateChunkPool()
        {
            var pool = new MonoBehaviourPool<ChunkObject>();
            pool.Initialize(_generationConfig.MaxChunkCount, _chunkPrefab);
            return pool;
        }

        private IObjectPool<ChunkBuffer> CreateChunkBufferPool()
        {
            ChunkBufferPool pool = new ChunkBufferPool();
            pool.Initialize(_generationConfig.MaxChunkCount, _generationConfig.Resolution);
            return pool;
        }
    }
}
