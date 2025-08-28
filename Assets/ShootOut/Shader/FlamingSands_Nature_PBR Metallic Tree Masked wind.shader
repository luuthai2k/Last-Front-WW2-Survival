Shader "FlamingSands/Nature/PBR Metallic Tree Masked wind" {
	Properties {
		[Header(Wind Shader Foliage)] [HideInInspector] _windmap ("wind map", 2D) = "white" {}
		_WindStrength ("Wind Strength", Range(0, 0.4)) = 0.1
		_WindRadius ("Wind Radius", Range(0, 30)) = 10
		_WindSpeed ("Wind Speed", Range(0, 0.4)) = 0.1
		_VertexAO ("Vertex AO", Range(0, 1)) = 1
		_Cutoff ("Mask Clip Value", Float) = 0.25
		[NoScaleOffset] _AlbedoA ("Albedo (A)", 2D) = "white" {}
		_MetallnessValue ("Metallness Value", Float) = 1
		_Glossiness ("Glossiness", Float) = 1
		_NormalIntensity ("Normal Intensity", Float) = 1
		[NoScaleOffset] [Normal] _Normal ("Normal", 2D) = "bump" {}
		[HideInInspector] _texcoord ("", 2D) = "white" {}
		[HideInInspector] __dirty ("", Float) = 1
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200

		Pass
		{
			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			float4x4 unity_ObjectToWorld;
			float4x4 unity_MatrixVP;

			struct Vertex_Stage_Input
			{
				float4 pos : POSITION;
			};

			struct Vertex_Stage_Output
			{
				float4 pos : SV_POSITION;
			};

			Vertex_Stage_Output vert(Vertex_Stage_Input input)
			{
				Vertex_Stage_Output output;
				output.pos = mul(unity_MatrixVP, mul(unity_ObjectToWorld, input.pos));
				return output;
			}

			float4 frag(Vertex_Stage_Output input) : SV_TARGET
			{
				return float4(1.0, 1.0, 1.0, 1.0); // RGBA
			}

			ENDHLSL
		}
	}
	Fallback "Diffuse"
	//CustomEditor "ASEMaterialInspector"
}