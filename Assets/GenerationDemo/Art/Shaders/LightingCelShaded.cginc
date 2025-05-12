#pragma multi_compile _MAIN_LIGHT_SHADOWS
#pragma multi_compile _MAIN_LIGHT_SHADOWS_CASCADES
#pragma multi_compile _SHADOWS_SOFT
#pragma multi_compile _ADDITIONAL_LIGHTS
#pragma multi_compile _ADDITIONAL_LIGHT_SHADOWS

#ifndef SHADERGRAPH_PREVIEW
struct SurfaceVariables {
    float3 normal;
    float3 view;
    float smoothness;
    float shininess;
    float rimThreshold;
    float shadowStrength;
};

float3 CalculateCelShading(Light light, SurfaceVariables variables) {
    float edgeDiffuse = 0.1f;
    float edgeSpecular = 0.1f;
    float edgeSpecularOffset = 0.05f;
    float edgeShadowAttenuation = 0.3f;
    float edgeDistanceAttenuation = 0.3f;
    float edgeRim = 0.65f;
    float edgeRimOffset = 0.1f;
    
    float shadowAttenuation = smoothstep(
        edgeShadowAttenuation, 1 - edgeShadowAttenuation, light.shadowAttenuation);
    
    float distanceAttenuation = smoothstep(0, edgeDistanceAttenuation, light.distanceAttenuation);
    
    float attenuation = lerp(1, shadowAttenuation * distanceAttenuation, variables.shadowStrength);
    
    float diffuse = saturate(dot(variables.normal, light.direction)) * attenuation;
    
    float3 h = SafeNormalize(light.direction + variables.view);
    float specular = saturate(dot(variables.normal, h));
    specular = pow(specular, variables.shininess);
    specular *= diffuse * variables.smoothness;
    
    float rim = 1 - dot(variables.view, variables.normal);
    rim *= pow(diffuse, variables.rimThreshold);
    
    diffuse = smoothstep(0, edgeDiffuse, diffuse);
    
    specular = variables.smoothness * smoothstep(
        (1 - variables.smoothness) * edgeSpecular + edgeSpecularOffset,
        edgeSpecular + edgeSpecularOffset,
        specular);
    
    rim = variables.smoothness * smoothstep(
        edgeRim - 0.5f * edgeRimOffset,
        edgeRim + 0.5f * edgeRimOffset,
        rim);
    
    return light.color * (diffuse + max(specular, rim));
}
#endif

float4 GetShadowCoord(float3 worldPosition) {
#if defined(SHADERGRAPH_PREVIEW)
    return 1;
#else
#if SHADOWS_SCREEN
    float4 clipPos = TransformWorldToHClip(worldPosition);
    return ComputeScreenPos(clipPos);
#else
    return TransformWorldToShadowCoord(worldPosition);
#endif
#endif
}

void LightingCelShaded_float(
    float Smoothness, float RimThreshold, float ShadowStrength,
    float3 WorldPosition, float3 Normal, float3 View, out float3 Color) {
#if defined(SHADERGRAPH_PREVIEW)
    Color = float3(1, 1, 0);
#else
    SurfaceVariables variables;
    
    variables.normal = normalize(Normal);
    variables.view = SafeNormalize(View);
    variables.smoothness = Smoothness;
    variables.shininess = exp2(10 * Smoothness + 1);
    variables.rimThreshold = RimThreshold;
    variables.shadowStrength = ShadowStrength;
    
    float4 shadowCoord = GetShadowCoord(WorldPosition);
    
    Light light = GetMainLight(shadowCoord);
    Color = CalculateCelShading(light, variables);
    
    int pixelLightCount = GetAdditionalLightsCount();
    for (int i = 0; i < pixelLightCount; i++) {
        light = GetAdditionalLight(i, WorldPosition, 1);
        Color += CalculateCelShading(light, variables);
    }
#endif
}
