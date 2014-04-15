// new HLSL shader
sampler2D  Texture1Sampler : register(s0);  //take ImageSampler from S0 register. 

// 'uv' vector from TEXCOORD0 semantics is our         texture coordinate, two floating point numbers in the range 0-1.
float4 main(float2 uv : TEXCOORD) : COLOR
{
    float4 color = tex2D(Texture1Sampler, uv);
    float gray = dot(float3(0.3, 0.59, 0.11), color);
    return gray;
}
