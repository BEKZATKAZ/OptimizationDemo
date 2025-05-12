using ConwayDemo.Interfaces;
using ConwayDemo.Data;
using ConwayDemo.Data.Services;
using System;
using UnityEngine;

namespace ConwayDemo.Api
{
    public sealed class Bootstrap : MonoBehaviour
    {
        [SerializeField] private MeshRenderer _plane;
        [SerializeField] private ConwayConfig _config;
        [SerializeField] private ComputeShader _computeShader;
        [SerializeField] private bool _isOptimized;

        private IRunnerService _runner;
        private RenderTexture _texture;

        private IRunnerService CreateRunner(RenderTexture texture)
        {
            if (_isOptimized == false) return new StandardRunnerService(texture, _config);
            OptimizedRunnerService runner = new OptimizedRunnerService(_computeShader, texture, _config);
            runner.Initialize();
            return runner;
        }

        private void Awake()
        {
            Vector2Int resolution = _config.Resolution;

            _texture = new RenderTexture(resolution.x, resolution.y, 24, RenderTextureFormat.Default, 4)
            {
                enableRandomWrite = true,
                filterMode = FilterMode.Point
            };

            _plane.transform.localScale = new Vector3(resolution.x * 0.1f, resolution.y * 0.1f, 1);
            _plane.sharedMaterial.SetTexture("_MainTexture", _texture);
            _plane.sharedMaterial.SetVector("_Resolution", (Vector2)resolution);

            _runner = CreateRunner(_texture);
            _runner.RandomizeCells(0.5f);

            Runner runnerObject = new GameObject("RUNNER", typeof(Runner)).GetComponent<Runner>();
            runnerObject.Initialize(_runner);
        }

        private void OnDestroy()
        {
            if (_runner is IDisposable disposable) disposable.Dispose();
            Destroy(_texture);
        }
    }
}
