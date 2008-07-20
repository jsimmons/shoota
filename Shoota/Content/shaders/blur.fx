sampler TextureSampler : register(s0);

struct VertexShaderOutput
{
    float4 col : COLOR0;
    float2 tex : TEXCOORD0;
};

float gameTime = 0;

float pixelSizeX = 0.001f;
float pixelSizeY = 0.001f;

static const float blurStrength = 1.5f;

static const int g_cKernelSize = 13;

static const float BlurWeights[g_cKernelSize] = 
{
    0.002216,
    0.008764,
    0.026995,
    0.064759,
    0.120985,
    0.176033,
    0.199471,
    0.176033,
    0.120985,
    0.064759,
    0.026995,
    0.008764,
    0.002216,
};

float4 BlurHorizontal( VertexShaderOutput input ) : COLOR0
{   
	input.col = 0;
 
	for( int i = -6; i <= 6; i++ )
	{
		input.col += tex2D( TextureSampler, input.tex + ( float2( ((float)i) * blurStrength * pixelSizeX, 0.0f ) ) * BlurWeights[i + 6] );
	}
	
	return input.col;
}

float4 BlurVertical( VertexShaderOutput input ) : COLOR0
{    
	input.col = 0;
	
	for( int i = -6; i <= 6; i++ )
	{
		input.col += tex2D( TextureSampler, input.tex + ( float2( 0.0f, ((float)i) * blurStrength * pixelSizeY ) ) ) * BlurWeights[i + 6];
	}

	return input.col;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 BlurHorizontal();      
    }
    
	pass Pass1
    {
        PixelShader = compile ps_2_0 BlurVertical();      
    }
}