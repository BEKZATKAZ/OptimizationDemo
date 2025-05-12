using UnityEngine;

namespace ConwayDemo.Data
{
    [CreateAssetMenu(fileName = "ConwayConfig", menuName = "ConwayDemo/Config")]
    public sealed class ConwayConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int _resolution;
        [SerializeField] private Color _activeColor;
        [SerializeField] private Color _deadColor;

        public int InlineResolution => _resolution.x * _resolution.y;
        public Vector2Int Resolution => _resolution;
        public Color ActiveColor => _activeColor;
        public Color DeadColor => _deadColor;
    }
}
