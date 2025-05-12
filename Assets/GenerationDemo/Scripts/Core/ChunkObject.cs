using UnityEngine;

namespace GenerationDemo.Core
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public sealed class ChunkObject : MonoBehaviour
    {
        public Mesh Mesh { get; private set; }

        private MeshFilter _filter;
        private MeshRenderer _renderer;

        private void Awake()
        {
            Mesh = new Mesh();

            _filter = gameObject.GetComponent<MeshFilter>();
            _renderer = gameObject.GetComponent<MeshRenderer>();

            _filter.sharedMesh = Mesh;
        }

        private void OnEnable()
        {
            _renderer.material.SetFloat("_StartTime", Time.time);
        }
    }
}
