using Cinemachine;
using UnityEngine;

namespace GenerationDemo.Api
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public sealed class ConstantRotation : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 5;

        private CinemachineOrbitalTransposer _transposer;

        private void Awake()
        {
            CinemachineVirtualCamera camera = GetComponent<CinemachineVirtualCamera>();
            _transposer = camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        }

        private void LateUpdate()
        {
            if (_transposer == false) return;
            _transposer.m_Heading.m_Bias += Time.deltaTime * _rotationSpeed;
        }
    }
}
