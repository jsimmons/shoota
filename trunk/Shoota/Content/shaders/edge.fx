sampler TextureSampler : register(s0);

struct VertexShaderOutput
{
    float4 col : COLOR0;
    float2 tex : TEXCOORD0;
};

float gameTime = 0;

float pixelSizeX = 0.001f;
float pixelSizeY = 0.001f;

static const float pixels = 1.0f;
static const float threshold = 0.001f;

float getGray(float4 c)
{
    return(dot(c.rgb,((0.33333).xxx)));
}

float4 EdgeDetect( VertexShaderOutput input ) : COLOR0
{   
	float2 offsetX = ( pixels * pixelSizeX, 0.0f );
	float2 offsetY = ( 0.0f, pixels * pixelSizeY );

	float base = getGray( tex2D( TextureSampler, input.tex ) );

	float s = getGray( tex2D( TextureSampler, input.tex + offsetX ) ) * 0.25f;
	s += getGray( tex2D( TextureSampler, input.tex - offsetX ) ) * 0.25f;
	s += getGray( tex2D( TextureSampler, input.tex + offsetY ) ) * 0.25f;
	s += getGray( tex2D( TextureSampler, input.tex - offsetY ) ) * 0.25f;
	
	input.col = 0;
	
	if( abs( base - s ) * 2 > threshold )
	{
		input.col = 1;
	}

	input.col.a = 1.0f;

	return input.col;
}

technique Technique1
{
    pass Pass1
    {
        PixelShader = compile ps_2_0 EdgeDetect();      
    }
}