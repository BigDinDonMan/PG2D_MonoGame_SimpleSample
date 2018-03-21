#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

// Effect applies normalmapped lighting to a 2D sprite.

float2 lightPos_1;
//float2 lightPos_2;
//float2 lightPos_3;
//float2 lightPos_4;
float2 lightMapSize;
float2 textureSize;


Texture2D SpriteTexture;
Texture2D LightMapTexture;

struct VertexShaderOutput
{
    float4 Position : SV_POSITION;
    float4 Color : COLOR0;
    float2 TextureCoordinates : TEXCOORD0;
};

// Version 1
SamplerState TextureSampler = sampler_state
{
    Texture = <SpriteTexture>;
};

SamplerState LightMapSampler = sampler_state
{
    Texture = <LightMapTexture>;
};


float4 MainPS(float4 pos : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{

    //find absolute position of the shader
    float horizontalPos = floor(texCoord.x * textureSize.x);
    float verticalPos = floor(texCoord.y * textureSize.y);
    float2 lightMapCoords = { 0, 0 };

    float2 lightPos = lightPos_1;

    //first light
    if (horizontalPos > lightPos.x - lightMapSize.x && horizontalPos < lightPos.x + lightMapSize.x)
    {
        float x = horizontalPos - lightPos.x;
        float y = verticalPos - lightPos.y;
        lightMapCoords = float2(x / lightMapSize.x, y / lightMapSize.y);

    }
    //else if (horizontalPos > lightPos_2.x - lightMapSize.x && horizontalPos < lightPos_2.x + lightMapSize.x)
    //{
    //    float x = horizontalPos - lightPos_2.x;
    //    float y = verticalPos - lightPos_2.y;
    //    lightMapCoords = float2(x / lightMapSize.x, y / lightMapSize.y);
    //}

    //else if (horizontalPos > lightPos_3.x - lightMapSize.x && horizontalPos < lightPos_3.x + lightMapSize.x)
    //{
    //    float x = horizontalPos - lightPos_3.x;
    //    float y = verticalPos - lightPos_3.y;
    //    lightMapCoords = float2(x / lightMapSize.x, y / lightMapSize.y);
    //}

    //else if (horizontalPos > lightPos_4.x - lightMapSize.x && horizontalPos < lightPos_4.x + lightMapSize.x)
    //{
    //    float x = horizontalPos - lightPos_4.x;
    //    float y = verticalPos - lightPos_4.y;
    //    lightMapCoords = float2(x / lightMapSize.x, y / lightMapSize.y);
    //}

    //get pixel info from the texture
    float4 tex = SpriteTexture.Sample(TextureSampler, texCoord);
    float map = LightMapTexture.Sample(LightMapSampler, lightMapCoords);
    
	////Look up the lighting value
 //   float4 lightIntensity = LightMapTexture.Sample(LightMapSampler, texCoord);


	//// Compute lighting.
 //   float lightAmount = saturate(dot(normal.xyz, LightDirection));
 //   color.rgb *= AmbientColor + (lightAmount * LightColor);

    return tex * map;
}
//END Version 1


////Version 2
//sampler2D TextureSampler = sampler_state
//{
//    Texture = <SpriteTexture>;
//};

//sampler2D NormalSampler = sampler_state
//{
//    Texture = <NormalTexture>;
//};

//float4 MainPS(VertexShaderOutput input) : SV_TARGET0
//{
//    //get pixel info from the texture
//    //float4 tex = SpriteTexture.Sample(TextureSampler, texCoord);
//    float4 tex = tex2D(TextureSampler, input.TextureCoordinates);
//	//Look up the normalmap value
//    float4 normal = 2 * tex2D(NormalSampler, input.TextureCoordinates) - 1;
//    //float4 normal = 2 * NormalTexture.Sample(NormalSampler, texCoord) - 1;


//	// Compute lighting.
//    float lightAmount = saturate(dot(normal.xyz, LightDirection));
//    input.Color.rgb *= AmbientColor + (lightAmount * LightColor);

//    return input.Color * tex;
//}
//// END Version 2



technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
