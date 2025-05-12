float hash(float2 value)
{
    return frac(sin(dot(value, float2(12.9898, 78.233))) * 43758.5453);
}

float fade(float t)
{
    return t * t * t * (t * (t * 6 - 15) + 10);
}

float gradientNoise(float2 value)
{
    int ix = int(floor(value.x));
    int iy = int(floor(value.y));
    
    float2 f = value - float2(ix, iy);
    
    float a = hash(float2(ix, iy));
    float b = hash(float2(ix + 1, iy));
    float c = hash(float2(ix, iy + 1));
    float d = hash(float2(ix + 1, iy + 1));
    
    float fadeX = fade(f.x);
    float fadeY = fade(f.y);

    float ab = lerp(a, b, fadeX);
    float cd = lerp(c, d, fadeX);
    
    return lerp(ab, cd, fadeY);
}

float octavedGradientNoise(float2 value, int octaves, float persistence, float frequencyMultiplier)
{
    float total = 0;
    float maxValue = 0;
    float frequency = 1;
    float amplitude = 1;

    for (int i = 0; i < octaves; i++)
    {
        total += gradientNoise(value * frequency) * amplitude;
        maxValue += amplitude;

        frequency *= frequencyMultiplier;
        amplitude *= persistence;
    }

    return total / maxValue;
}
