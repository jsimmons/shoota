sampler TextureSampler : register(s0);

float gameTime = 0;

float pixelSizeX = 0.001f;
float pixelSizeY = 0.001f;

struct VertexShaderOutput
{
    float4 color : COLOR0;
    float2 texCoord : TEXCOORD0;
};

float4 PixelShaderFunction( VertexShaderOutput input ) : COLOR0
{    
	input.texCoord.y += sin( gameTime + input.texCoord.x * 5 ) * 0.05;
	input.texCoord.x += cos( gameTime + input.texCoord.y ) * 0.01;
	
	float4 color = tex2D( TextureSampler, input.texCoord );
	
    return color;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 PixelShaderFunction();      
    }
}