using GenerationDemo.Api.Installation.Abstractions;
using GenerationDemo.Api.Installation.Builders;
using GenerationDemo.Core;
using GenerationDemo.Data;
using UnityEngine;

namespace GenerationDemo.Api.Installation
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private GenerationConfig _generationConfig;
        [SerializeField] private ChunkObject _chunkPrefab;
        [SerializeField] private ChunkObserver _observer;
        [SerializeField] private ComputeShader _generationShader;
        [SerializeField] private bool _isOptimized;

        private BuiltContentBase _content;

        private void Awake()
        {
            IBuilder builder = _isOptimized
                ? new OptimizedBuilder(_generationConfig, _generationShader, _chunkPrefab)
                : new StandardBuilder(_generationConfig, _chunkPrefab);

            _content = builder.Build();

            _observer.Initialize(_content.GeneratorUseCase);
        }

        private void OnDestroy()
        {
            _content.Dispose();
        }
    }
}
