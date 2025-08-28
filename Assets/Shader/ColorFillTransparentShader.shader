Shader"Shader Graphs/ColorFillTransparentShader"
{
    Properties
    {
        _FillRate ("Fill Rate", Float) = 0.7
        _LockColor ("LockColor", Color) = (0,0,0,1)
        _BaseColor ("BaseColor", Color) = (1,1,1,1)
        [NoScaleOffset] _MainTex ("Base Map", 2D) = "white" {}
        _BorderWidth ("Border Width", Float) = 0.05
        _Texture2D ("Texture2D", 2D) = "white" {}
        [HideInInspector] _QueueOffset ("_QueueOffset", Float) = 0
        [HideInInspector] _QueueControl ("_QueueControl", Float) = -1
        [HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
LOD 200

        CGPROGRAM
       #pragma surface surf Lambert alpha:fade
        #pragma target 3.0

sampler2D _MainTex;
sampler2D _Texture2D;
float4 _Texture2D_ST;
float _FillRate;
float _BorderWidth;
float4 _LockColor;
float4 _BaseColor;

struct Input
{
    float2 uv_MainTex;
    float3 worldPos;
    float4 screenPos; 
};

void vert(inout appdata_full v, out Input o)
{
    UNITY_INITIALIZE_OUTPUT(Input, o);
    float4 worldPos = mul(unity_ObjectToWorld, v.vertex);
    o.worldPos = worldPos.xyz;
    o.screenPos = ComputeScreenPos(UnityObjectToClipPos(v.vertex)); 
    o.uv_MainTex = v.texcoord;
}
void surf(Input IN, inout SurfaceOutput o)
{
    float posY = IN.worldPos.y;

    float borderMin = _FillRate;
    float borderMax = _FillRate + _BorderWidth;
    if (posY < _FillRate)
    {
                // Vùng đã fill
        fixed4 tex = tex2D(_MainTex, IN.uv_MainTex) * _BaseColor;
        o.Albedo = tex.rgb;
        o.Alpha = tex.a;
    }
    else if (posY >= borderMin && posY <= borderMax)
    {
                // Border
       
        float2 uv_Texture2D;

        uv_Texture2D.x = frac(IN.screenPos.x / IN.screenPos.w) * _Texture2D_ST.x;
        uv_Texture2D.y =(posY - borderMin) * _Texture2D_ST.y;

        fixed4 borderTex = tex2D(_Texture2D, uv_Texture2D);
        if (borderTex.r < 0.01 && borderTex.g < 0.01 && borderTex.b < 0.01)
        {
            o.Alpha = 0;
        }
        else
        {
            o.Albedo = borderTex.rgb;
            o.Alpha = borderTex.a;
        }
    }
    else
    {
                // Vùng Lock
        o.Albedo = _LockColor.rgb;
        o.Alpha = _LockColor.a;
    }
}
        ENDCG
    }

Fallback"Legacy Shaders/Diffuse"
}
