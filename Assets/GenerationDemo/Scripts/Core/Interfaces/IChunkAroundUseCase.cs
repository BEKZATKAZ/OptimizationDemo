using System;
using System.Collections;
using UnityEngine;

namespace GenerationDemo.Core.Interfaces
{
    public interface IChunkAroundUseCase : IDisposable
    {
        IEnumerator Perform(Vector2 centerPosition);
    }
}
