using GenerationDemo.Api.Installation.Abstractions;
using GenerationDemo.Api.Installation.Content;
using GenerationDemo.Api.Placers;
using GenerationDemo.Core;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Core.UseCases;
using GenerationDemo.Data;
using GenerationDemo.Data.Services;
using UnityEngine;

namespace GenerationDemo.Api.Installation.Builders
{
    public sealed class StandardBuilder : IBuilder
    {
        public StandardBuilder(GenerationConfig config, ChunkObject chunkPrefab)
        {
            _generationConfig = config;
            _chunkPrefab = chunkPrefab;
        }

        private readonly GenerationConfig _generationConfig;
        private readonly ChunkObject _chunkPrefab;

        public BuiltContentBase Build()
        {
            StandardChunkGenerator generator = CreateGenerator();
            StandardChunkPlacer placer = CreatePlacer(generator);
            StandardChunkAroundUseCase useCase = CreateUseCase(generator);

            return new StandardBuiltContent(useCase, placer);
        }

        private StandardChunkGenerator CreateGenerator()
        {
            return new StandardChunkGenerator(_generationConfig);
        }

        private StandardChunkPlacer CreatePlacer(IChunkGenerator generator)
        {
            GameObject gameObject = new GameObject("REGULAR-PLACER");

            StandardChunkPlacer placer = gameObject
                .AddComponent<StandardChunkPlacer>()
                .WithPrefab(_chunkPrefab);

            placer.Initialize(generator);
            return placer;
        }

        private StandardChunkAroundUseCase CreateUseCase(IChunkGenerator generator)
        {
            GenerationConfig config = _generationConfig;
            return new StandardChunkAroundUseCase(generator, config.Size, config.Resolution, config.Range);
        }
    }
}
