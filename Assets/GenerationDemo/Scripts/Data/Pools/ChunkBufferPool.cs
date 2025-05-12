using GenerationDemo.Core;
using GenerationDemo.Core.DTOs;
using GenerationDemo.Core.Interfaces;
using GenerationDemo.Shared;
using System.Collections.Generic;

namespace GenerationDemo.Data.Pools
{
    public sealed class ChunkBufferPool : IObjectPool<ChunkBuffer>
    {
        private Queue<ChunkBuffer> _elements;

        public void Initialize(int amount, int resolution)
        {
            _elements = new Queue<ChunkBuffer>(amount);

            for (int i = 0; i < amount; i++)
            {
                Return(new ChunkBuffer
                {
                    Vertices = new VertexDTO[GenerationUtilities.GetVertexCount(resolution)],
                    Indices = new int[GenerationUtilities.GetIndexCount(resolution)]
                });
            }
        }

        public ChunkBuffer Get() => _elements.Dequeue();

        public void Return(ChunkBuffer element) => _elements.Enqueue(element);

        public void Dispose() => _elements.Clear();
    }
}
