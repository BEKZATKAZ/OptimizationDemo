using ConwayDemo.Interfaces;
using System.Linq;
using UnityEngine;

namespace ConwayDemo.Data.Services
{
    public sealed class StandardRunnerService : IRunnerService
    {
        public StandardRunnerService(RenderTexture texture, ConwayConfig config)
        {
            _texture = texture;
            _config = config;
            _cells = new bool[config.Resolution.y, config.Resolution.x];
        }

        private static readonly Vector2Int[] _neighborOffsetsDefinition =
        {
            new(-1, -1), new(0, -1), new(1, -1),
            new(-1, 0), new(1, 0),
            new(-1, 1), new(0, 1), new(1, 1)
        };

        private readonly bool[,] _cells;
        private readonly RenderTexture _texture;
        private readonly ConwayConfig _config;
        
        public void RandomizeCells(float density)
        {
            for (int y = 0; y < _config.Resolution.y; y++)
            {
                for (int x = 0; x < _config.Resolution.x; x++)
                {
                    _cells[y, x] = Random.Range(0, 1f) < density;
                }
            }
        }

        public void RunNext()
        {
            Texture2D tempTexture = new Texture2D(
                _config.Resolution.x, _config.Resolution.y, TextureFormat.RGB24, true, true);

            bool[,] changes = new bool[_config.Resolution.x, _config.Resolution.y];

            for (int y = 0; y < _config.Resolution.y; y++)
            {
                for (int x = 0; x < _config.Resolution.x; x++)
                {
                    int neighborCount = GetNeighborCount(new Vector2Int(x, y));
                    changes[y, x] = neighborCount == 3 || (neighborCount == 2 && _cells[y, x]);
                }
            }

            for (int y = 0; y < _config.Resolution.y; y++)
            {
                for (int x = 0; x < _config.Resolution.x; x++)
                {
                    _cells[y, x] = changes[y, x];
                    tempTexture.SetPixel(x, y, _cells[y, x] ? _config.ActiveColor : _config.DeadColor);
                }
            }

            tempTexture.Apply();

            Graphics.Blit(tempTexture, _texture);
            Object.Destroy(tempTexture);
        }

        private bool HasNeighbor(Vector2Int coords)
        {
            if (coords.x < 0) coords.x = _config.Resolution.x - 1;
            if (coords.y < 0) coords.y = _config.Resolution.y - 1;
            if (coords.x >= _config.Resolution.x) coords.x = 0;
            if (coords.y >= _config.Resolution.y) coords.y = 0;
            return _cells[coords.y, coords.x];
        }

        private int GetNeighborCount(Vector2Int coords)
        {
            return _neighborOffsetsDefinition.Where(x => HasNeighbor(coords + x)).Count();
        }
    }
}
