sampler TextureSampler : register(s0);

struct VertexShaderOutput
{
    float4 col : COLOR0;
    float2 tex : TEXCOORD0;
};

float gameTime = 0;

float pixelSizeX = 0.001f;
float pixelSizeY = 0.001f;

static const int NumTiles = 100;
static const half3 EdgeColor = { 0.0f, 0.0f, 0.0f };
static const half Threshhold = 0.15f;


float4 Tiles( VertexShaderOutput input ) : COLOR0
{    
    half size = 1.0/NumTiles;
    
    half2 Pbase = input.tex - fmod( input.tex, size.xx );
    half2 PCenter = Pbase + ( size / 2.0 ).xx;
    
    half2 st = (input.tex - Pbase) / size;
    half4 c1 = (half4)0;
    half4 c2 = (half4)0;
    
    half4 invOff = half4( ( 1 - EdgeColor ), 1 );
    
    if (st.x > st.y) { c1 = invOff; }
    
    half threshholdB =  1.0 - Threshhold;
    if (st.x > threshholdB) { c2 = c1; }
    if (st.y > threshholdB) { c2 = c1; }
    
    half4 cBottom = c2;
    
    c1 = (half4)0;
    c2 = (half4)0;
    
    if (st.x > st.y) { c1 = invOff; }
    if (st.x < Threshhold) { c2 = c1; }
    if (st.y < Threshhold) { c2 = c1; }
    
    half4 cTop = c2;
    half4 tileColor = tex2D( TextureSampler, PCenter );
    input.col = tileColor + cTop - cBottom;
    
    return input.col;
}

technique Technique1
{
    pass Pass0
    {
        PixelShader = compile ps_3_0 Tiles();      
    }
}