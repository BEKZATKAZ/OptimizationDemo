using GenerationDemo.Core.DTOs;
using GenerationDemo.Core.Interfaces;
using UnityEngine;

namespace GenerationDemo.Api.Placers
{
    public abstract class ChunkPlacerBase : MonoBehaviour
    {
        private IChunkGenerator _generator;

        protected abstract void OnChunkGenerated(VertexDTO[] vertices, int[] indices);

        public void Initialize(IChunkGenerator generator)
        {
            _generator = generator;
            _generator.ChunkGenerated += OnChunkGenerated;
        }

        private void OnDestroy()
        {
            _generator.ChunkGenerated -= OnChunkGenerated;
        }
    }
}
