using System;
using UnityEngine;

namespace GenerationDemo.Shared
{
    public static class TextureUtilities
    {
        public static Texture2D BakeToTexture(this AnimationCurve curve, int resolution)
        {
            if (resolution < 2) throw new ArgumentOutOfRangeException(nameof(resolution));

            Texture2D texture = new Texture2D(resolution, 1, TextureFormat.RFloat, false, true);

            for (int i = 0; i < resolution; i++)
            {
                float value = curve.Evaluate((float)i / (resolution - 1));
                texture.SetPixel(i, 0, new Color(value, value, value));
            }

            texture.Apply(false);
            return texture;
        }
    }
}
