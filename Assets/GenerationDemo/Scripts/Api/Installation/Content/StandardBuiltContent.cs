using GenerationDemo.Api.Installation.Abstractions;
using GenerationDemo.Api.Placers;
using GenerationDemo.Core.Interfaces;

namespace GenerationDemo.Api.Installation.Content
{
    public sealed class StandardBuiltContent : BuiltContentBase
    {
        public StandardBuiltContent(
            IChunkAroundUseCase generatorUseCase,
            StandardChunkPlacer placer) : base(generatorUseCase)
        {
            _placer = placer;
        }

        private readonly StandardChunkPlacer _placer;

        public override void Dispose()
        {
            if (_placer) UnityEngine.Object.Destroy(_placer);
            base.Dispose();
        }
    }
}
