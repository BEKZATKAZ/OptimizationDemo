using UnityEngine;

namespace GenerationDemo.Data
{
    [CreateAssetMenu(fileName = "Generation", menuName = "GenerationDemo/Config")]
    public sealed class GenerationConfig : ScriptableObject
    {
        [Header("Main")]
        [SerializeField] private int _resolution = 16;
        [SerializeField] private int _size = 10;
        [SerializeField] private int _range = 5;
        [SerializeField] private float _amplitude = 5;
        [SerializeField] private float _frequency = 1;
        [SerializeField] private AnimationCurve _erosion;

        [Header("Fractal")]
        [SerializeField] private float _frequencyMultiplier = 3;
        [SerializeField] private float _persistence = 0.5f;
        [SerializeField] private int _octaves = 4;

        public int Resolution => _resolution;
        public int Size => _size;
        public int Range => _range;
        public float Amplitude => _amplitude;
        public float Frequency => _frequency;
        public AnimationCurve Erosion => _erosion;

        public float FrequencyMultiplier => _frequencyMultiplier;
        public float Persistence => _persistence;
        public int Octaves => _octaves;

        public int MaxChunkCount => (_range * 2 + 1) * (_range * 2 + 1);
    }
}
