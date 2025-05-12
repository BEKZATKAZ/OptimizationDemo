using GenerationDemo.Core.Interfaces;
using System.Collections;
using UnityEngine;

namespace GenerationDemo.Api
{
    public sealed class ChunkObserver : MonoBehaviour
    {
        private IChunkAroundUseCase _generatorUseCase;
        private Transform _transform;
        private bool _isObserving;

        public void Initialize(IChunkAroundUseCase generatorUseCase)
        {
            _generatorUseCase = generatorUseCase;
        }

        private IEnumerator Observing()
        {
            _isObserving = true;

            Vector3 position = _transform.position;
            yield return StartCoroutine(_generatorUseCase.Perform(new Vector2(position.x, position.z)));

            _isObserving = false;
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K) && _isObserving == false)
                StartCoroutine(Observing());
        }
    }
}
