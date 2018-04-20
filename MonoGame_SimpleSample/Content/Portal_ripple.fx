#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_3
	#define PS_SHADERMODEL ps_4_0_level_9_3
#endif


/// <description>Applies water defraction effect.</description>
// from Affirma Consulting Blog
// http://affirmaconsulting.wordpress.com/2010/12/30/tool-for-developing-hlsl-pixel-shaders-for-wpf-and-silverlight/


//struct VertexShaderOutput
//{
//    float4 Position : SV_POSITION;
//    float4 Color : COLOR0;
//    float2 TextureCoordinates : TEXCOORD0;
//};

Texture2D SpriteTexture;

SamplerState TextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

//float tint;

float Timer;

/// <summary>Refraction Amount.</summary>
/// <minValue>20</minValue>
/// <maxValue>60</maxValue>
/// <defaultValue>50</defaultValue>
float Refracton;

/// <summary>Vertical trough</summary>
/// <minValue>20</minValue>
/// <maxValue>30</maxValue>
/// <defaultValue>23</defaultValue>
float VerticalTroughWidth;

/// <summary>Center X of the Zoom.</summary>
/// <minValue>20</minValue>
/// <maxValue>30</maxValue>
/// <defaultValue>23</defaultValue>
float Wobble2;

static const float2 poisson[12] =
{
    float2(-0.326212f, -0.40581f),
	float2(-0.840144f, -0.07358f),
	float2(-0.695914f, 0.457137f),
	float2(-0.203345f, 0.620716f),
	float2(0.96234f, -0.194983f),
	float2(0.473434f, -0.480026f),
	float2(0.519456f, 0.767022f),
	float2(0.185461f, -0.893124f),
	float2(0.507431f, 0.064425f),
	float2(0.89642f, 0.412458f),
	float2(-0.32194f, -0.932615f),
	float2(-0.791559f, -0.59771f)
};




float4 MainPS(float4 pos : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{

    float2 uv = texCoord;
    float delta_x = sin(Timer + uv.x * VerticalTroughWidth + uv.y * uv.y * Wobble2) * 0.02;
    float delta_y = cos(Timer + uv.y * 32 + uv.x * uv.x * 13) * 0.02;
    float2 Delta = float2(delta_x, delta_y);

    float2 NewUV = uv + Delta;

    float4 Color = 0;

    for (int i = 0; i < 12; i++)
    {
        float2 Coord = NewUV + (poisson[i] / Refracton);
        Color += SpriteTexture.Sample(TextureSampler, Coord) / 12.0;
    }
    Color += SpriteTexture.Sample(TextureSampler, uv) / 4;
    Color.a = 1.0;
    //Color.b += tint;
    return Color;



}



technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};



