using ConwayDemo.Interfaces;
using UnityEngine;

namespace ConwayDemo.Api
{
    public sealed class Runner : MonoBehaviour
    {
        private IRunnerService _service;
        private bool _isRunning;

        public void Initialize(IRunnerService runner)
        {
            _service = runner;
            _service.RunNext();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.K)) _isRunning = !_isRunning;
            if (_isRunning) _service.RunNext();
        }
    }
}
