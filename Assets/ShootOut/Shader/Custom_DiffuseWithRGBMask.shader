Shader "Custom/DiffuseWithRGBMask" {
	Properties {
		_Texture ("Texture", 2D) = "white" {}
		_RGBMask ("RGB Mask", 2D) = "white" {}
		_ColorR ("Color R", Vector) = (0,0,0,0)
		_ColorG ("Color G", Vector) = (0,0,0,0)
		_ColorB ("Color B", Vector) = (0,0,0,0)
		_Smoothness ("Smoothness", Range(0, 1)) = 0
		[Toggle] _ChangeUV ("ChangeUV", Float) = 0
		[HideInInspector] _texcoord2 ("", 2D) = "white" {}
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