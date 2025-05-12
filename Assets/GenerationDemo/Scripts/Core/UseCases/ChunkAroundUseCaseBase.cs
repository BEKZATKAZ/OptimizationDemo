using GenerationDemo.Core.Interfaces;
using System.Collections;
using UnityEngine;

namespace GenerationDemo.Core.UseCases
{
    public abstract class ChunkAroundUseCaseBase : IChunkAroundUseCase
    {
        protected ChunkAroundUseCaseBase(float size, int chunkRange)
        {
            _size = size;
            _chunkRange = chunkRange;
        }

        private readonly float _size;
        private readonly int _chunkRange;
        private bool _isDisposed;

        protected abstract bool CanPerform(Vector2Int coords);
        protected abstract void Performed(Vector2Int coords);

        public IEnumerator Perform(Vector2 centerPosition)
        {
            if (_isDisposed) yield break;

            Vector2 normalizedPosition = centerPosition / _size;
            Vector2Int roundedPosition = new Vector2Int(
                Mathf.RoundToInt(normalizedPosition.x),
                Mathf.RoundToInt(normalizedPosition.y));

            int x = 0;
            int y = 0;
            int dx = 0;
            int dy = 1;
            int segmentLength = 1;
            int segmentPassed = 0;
            int changes = 0;
            int maxDistance = _chunkRange * 2 + 1;
            int totalSteps = maxDistance * maxDistance;

            for (int i = 0; i < totalSteps; i++)
            {
                if (_isDisposed) yield break;

                Vector2Int coords = new Vector2Int(x, y) + roundedPosition;

                if (CanPerform(coords) && Mathf.Abs(x) <= _chunkRange && Mathf.Abs(y) <= _chunkRange)
                {
                    Performed(coords);
                    yield return null;
                }

                x += dx;
                y += dy;
                segmentPassed++;

                if (segmentPassed != segmentLength) continue;

                int temp = dx;

                dx = -dy;
                dy = temp;
                changes++;
                segmentPassed = 0;

                if ((changes & 1) == 0) segmentLength++;
            }
        }

        public void Dispose() => _isDisposed = true;
    }
}
