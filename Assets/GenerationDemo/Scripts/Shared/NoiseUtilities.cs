using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;

namespace GenerationDemo.Shared
{
    public static class NoiseUtilities
    {
        public static float OctavedGradientNoise(
            float2 value, int octaves, float persistence, float frequencyMultiplier)
        {
            float total = 0;
            float maxValue = 0;
            float frequency = 1;
            float amplitude = 1;

            for (int i = 0; i < octaves; i++)
            {
                total += GradientNoise(value * frequency) * amplitude;
                maxValue += amplitude;

                frequency *= frequencyMultiplier;
                amplitude *= persistence;
            }

            return total / maxValue;
        }

        private static float Hash(float2 value)
        {
            return frac(sin(dot(value, new float2(12.9898f, 78.233f))) * 43758.5453f);
        }

        private static float Fade(float t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        private static float GradientNoise(float2 p)
        {
            int ix = Mathf.FloorToInt(p.x);
            int iy = Mathf.FloorToInt(p.y);

            float2 f = p - new float2(ix, iy);

            float a = Hash(new float2(ix, iy));
            float b = Hash(new float2(ix + 1, iy));
            float c = Hash(new float2(ix, iy + 1));
            float d = Hash(new float2(ix + 1, iy + 1));

            float fadeX = Fade(f.x);
            float fadeY = Fade(f.y);

            float ab = lerp(a, b, fadeX);
            float cd = lerp(c, d, fadeX);

            return lerp(ab, cd, fadeY);
        }
    }
}
