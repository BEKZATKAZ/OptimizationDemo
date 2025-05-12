using ConwayDemo.Interfaces;
using System;
using UnityEngine;

namespace ConwayDemo.Data.Services
{
    public sealed class OptimizedRunnerService : IRunnerService, IDisposable
    {
        public OptimizedRunnerService(ComputeShader shader, RenderTexture texture, ConwayConfig config)
        {
            _shader = shader;
            _texture = texture;
            _config = config;
        }

        private readonly ComputeShader _shader;
        private readonly RenderTexture _texture;
        private readonly ConwayConfig _config;
        private ComputeBuffer _neighborOffsetsBuffer;
        private ComputeBuffer _resultBuffer;
        private int _runNextIndex;
        private int _applyChangesIndex;

        private static readonly Vector2Int[] _neighborOffsetsDefinition =
        {
            new(-1, -1), new(0, -1), new(1, -1),
            new(-1, 0), new(1, 0),
            new(-1, 1), new(0, 1), new(1, 1)
        };

        public void Initialize()
        {
            _runNextIndex = _shader.FindKernel("RunNext");
            _applyChangesIndex = _shader.FindKernel("ApplyChanges");
            
            _neighborOffsetsBuffer = new ComputeBuffer(8, sizeof(int) * 2);
            _neighborOffsetsBuffer.SetData(_neighborOffsetsDefinition);
            _resultBuffer = new ComputeBuffer(_config.InlineResolution, sizeof(uint));

            _shader.SetVector("BufferSize", (Vector2)_config.Resolution);
            _shader.SetVector("ActiveCellColor", _config.ActiveColor);
            _shader.SetVector("DeadCellColor", _config.DeadColor);
            _shader.SetInt("NeighborOffsetCount", _neighborOffsetsDefinition.Length);

            _shader.SetBuffer(_runNextIndex, "NeighborOffsets", _neighborOffsetsBuffer);
            _shader.SetBuffer(_runNextIndex, "ResultBuffer", _resultBuffer);
            _shader.SetBuffer(_applyChangesIndex, "ResultBuffer", _resultBuffer);

            _shader.SetTexture(_runNextIndex, "Texture", _texture);
            _shader.SetTexture(_applyChangesIndex, "Texture", _texture);
        }

        public void RandomizeCells(float density)
        {
            uint[] cells = new uint[_config.InlineResolution];

            for (int i = 0; i < cells.Length; i++)
            {
                cells[i] = UnityEngine.Random.Range(0, 1f) < density ? 1u : 0u;
            }

            _resultBuffer.SetData(cells);
        }

        public void RunNext()
        {
            int threads = 16;
            int threadGroupsX = _config.Resolution.x / threads;
            int threadGroupsY = _config.Resolution.y / threads;

            _shader.Dispatch(_runNextIndex, threadGroupsX, threadGroupsY, 1);
            _shader.Dispatch(_applyChangesIndex, threadGroupsX, threadGroupsY, 1);
        }

        public void Dispose()
        {
            _neighborOffsetsBuffer?.Dispose();
            _resultBuffer?.Dispose();
        }
    }
}
