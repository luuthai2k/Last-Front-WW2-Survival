Shader"Custom/LeftRightMaskOverlayShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MaskTex ("Mask (RGBA)", 2D) = "white" {} // Mask texture (sử dụng RGBA)
        _LeftRegionColor ("Left Region Color", Color) = (0, 1, 0, 1) // Màu cho vùng bên trái
        _MaskOverlayColor ("Mask Overlay Color", Color) = (1, 0, 0, 1) // Màu đổ cho Mask Texture ở vùng giữa
        _RightRegionColor ("Right Region Color", Color) = (1, 0, 0, 1) // Màu cho vùng bên phải
        _LeftRegionEnd ("Left Region End", Range(0, 1)) = 0.33 // Điểm kết thúc của vùng bên trái (0-1)
        _RightRegionStart ("Right Region Start", Range(0, 1)) = 0.66 // Điểm bắt đầu của vùng bên phải (0-1)
        _MaskOpacity ("Mask Opacity", Range(0, 1)) = 1.0 // Độ trong suốt của Mask Texture
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
LOD 100
        Blend
SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
sampler2D _MaskTex;
float4 _MaskTex_ST;

float4 _LeftRegionColor;
float4 _MaskOverlayColor;
float4 _RightRegionColor;
float _LeftRegionEnd;
float _RightRegionStart;
float _MaskOpacity;

struct appdata
{
    float4 vertex : POSITION;
    float2 uv : TEXCOORD0;
};

struct v2f
{
    float4 vertex : SV_POSITION;
    float2 uv_main : TEXCOORD0;
    float2 uv_mask : TEXCOORD1;
};

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv_main = TRANSFORM_TEX(v.uv, _MainTex);
    o.uv_mask = TRANSFORM_TEX(v.uv, _MaskTex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float x = i.uv_main.x;
    fixed4 finalColor = fixed4(0, 0, 0, 0);
    fixed4 texColor = tex2D(_MainTex, i.uv_main);

    if (x < _LeftRegionEnd)
    {
        finalColor = _LeftRegionColor;
    }
    else if (x >= _RightRegionStart)
    {
        finalColor = _RightRegionColor;
    }
    else // Vùng ở giữa
    {
                    // Sample mask texture
        fixed4 maskColor = tex2D(_MaskTex, i.uv_mask);
        maskColor.a *= _MaskOpacity; // Áp dụng độ trong suốt

                    // Đổ màu cho Mask Texture và sử dụng alpha của mask để trộn với màu nền
        finalColor = lerp(_RightRegionColor, _MaskOverlayColor * maskColor, maskColor.a);
    }

                // Áp dụng texture chính lên trên màu
    finalColor = texColor * finalColor;

    return finalColor;
}
            ENDCG
        }
    }
}