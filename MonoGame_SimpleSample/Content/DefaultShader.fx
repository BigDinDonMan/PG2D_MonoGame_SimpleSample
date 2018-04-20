#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_3
	#define PS_SHADERMODEL ps_4_0_level_9_3
#endif


Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};


struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float4 MainPS(VertexShaderOutput input) : COLOR
{
    
	
	float4 color = tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;

	//float value = color.r * 0.21 + color.g * 0.72 + color.b * 0.07;
    float2 position = input.TextureCoordinates.xy;
    if (position.x % 0.2 > 0.1)
    {
        float value = color.r * 0.299 + color.g * 0.587 + color.b * 0.114;
        color.rgb = value;
    }

    return color;
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};