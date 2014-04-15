/// <class>ColorToneEffect</class>

/// <description>An effect that blends between partial desaturation and a two-color ramp.</description>

//-----------------------------------------------------------------------------------------
// Shader constant register mappings (scalars - float, double, Point, Color, Point3D, etc.)
//-----------------------------------------------------------------------------------------

/// <summary>The amount of desaturation to apply.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Desaturation : register(C0);

/// <summary>The amount of color toning to apply.</summary>
/// <minValue>0</minValue>
/// <maxValue>1</maxValue>
/// <defaultValue>0.5</defaultValue>
float Toned : register(C1);

/// <summary>The first color to apply to input. This is usually a light tone.</summary>
/// <defaultValue>Yellow</defaultValue>
float4 LightColor : register(C2);

/// <summary>The second color to apply to the input. This is usuall a dark tone.</summary>
/// <defaultValue>Navy</defaultValue>
float4 DarkColor : register(C3);

//--------------------------------------------------------------------------------------
// Sampler Inputs (Brushes, including Texture1)
//--------------------------------------------------------------------------------------

sampler2D Texture1Sampler : register(S0);

//--------------------------------------------------------------------------------------
// Pixel Shader
//--------------------------------------------------------------------------------------

float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(Texture1Sampler, uv);
    float3 scnColor = LightColor * (color.rgb / color.a);
    float gray = dot(float3(0.3, 0.59, 0.11), scnColor);
    
    float3 muted = lerp(scnColor, gray.xxx, Desaturation);
    float3 middle = lerp(DarkColor, LightColor, gray);
    
    scnColor = lerp(muted, middle, Toned);
    return float4(scnColor * color.a, color.a);
}


