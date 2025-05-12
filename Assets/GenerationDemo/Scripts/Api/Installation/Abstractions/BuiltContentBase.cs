using GenerationDemo.Core.Interfaces;
using System;

namespace GenerationDemo.Api.Installation.Abstractions
{
    public abstract class BuiltContentBase : IDisposable
    {
        protected BuiltContentBase(IChunkAroundUseCase generatorUseCase)
        {
            GeneratorUseCase = generatorUseCase;
        }

        public IChunkAroundUseCase GeneratorUseCase { get; }

        public virtual void Dispose()
        {
            GeneratorUseCase.Dispose();
        }
    }
}
