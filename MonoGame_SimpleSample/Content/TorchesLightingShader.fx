#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_3
	#define PS_SHADERMODEL ps_4_0_level_9_3
#endif

// Effect applies normalmapped lighting to a 2D sprite.

float2 lightPos_1;
float2 lightPos_2;
float2 lightPos_3;
float2 lightPos_4;
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

bool isInsideRect(float2 pointToCheck, float2 middlePosition, float2 size)
{
    if (pointToCheck.x > middlePosition.x - size.x / 2 && pointToCheck.x < middlePosition.x + size.x / 2
		&& pointToCheck.y > middlePosition.y - size.y / 2 && pointToCheck.y < middlePosition.y + size.y / 2)
		return true;
	else
		return false;
}


float4 MainPS(float4 pos : SV_POSITION, float4 color : COLOR0, float2 texCoord : TEXCOORD0) : SV_TARGET0
{

	
    float4 tex = SpriteTexture.Sample(TextureSampler, texCoord);


    float4 finalColor = color;
    float3 tintColor = float3(1.0f, 1.0f, 1.0f);

    //find absolute position of the shader
    float horizontalPos = floor(texCoord.x * textureSize.x);
    float verticalPos = floor(texCoord.y * textureSize.y);
    float2 currentPos = float2(horizontalPos, verticalPos);
    float2 lightMapCoords = { 0, 0 };
    bool isNearLight = false;


    float2 lightPos = float2(0, 0);
    if (isInsideRect(currentPos, lightPos_1, lightMapSize))
    {
        lightPos = lightPos_1;
        isNearLight = true;
    }
    else if (isInsideRect(currentPos, lightPos_2, lightMapSize))
    {
        lightPos = lightPos_2;
        isNearLight = true;
    }
    else if (isInsideRect(currentPos, lightPos_3, lightMapSize))
    {
        lightPos = lightPos_3;
        isNearLight = true;
    }
    else if (isInsideRect(currentPos, lightPos_4, lightMapSize))
    {
        lightPos = lightPos_4;
        isNearLight = true;
    }
    if (isNearLight)
    {
        float x = ((horizontalPos - lightPos.x) / lightMapSize.x) + (1.0f / 2.0f);
        float y = ((verticalPos - lightPos.y) / lightMapSize.y) + (1.0f / 2.0f);

        tintColor = float3(0.5f, 0.f, 0.2f);
        lightMapCoords = float2(x, y);
    }

    float lightIntensity = LightMapTexture.Sample(LightMapSampler, lightMapCoords);

    


	//// Compute lighting.
 //   float lightAmount = saturate(dot(normal.xyz, LightDirection));
 //   color.rgb *= AmbientColor + (lightAmount * LightColor);
    //float3 tintColor = float3(1.0f, 0.0f, 0.3f);
    //finalColor.rgb *= tintColor;
    finalColor.rgb = tex.rgb + (tintColor * lightIntensity);
    return finalColor;
}



technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};
