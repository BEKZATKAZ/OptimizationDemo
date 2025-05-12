using UnityEngine;

namespace ConwayDemo.Api
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraAnimation : MonoBehaviour
    {
        [SerializeField] private float _maxOrthographicSize = 51;

        private Camera _camera;
        private bool _isPlaying;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.L)) _isPlaying = true;

            if (_isPlaying == false) return;

            _camera.orthographicSize = Mathf.MoveTowards(
                _camera.orthographicSize, _maxOrthographicSize, Time.deltaTime * 10);
        }
    }
}
