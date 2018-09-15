Shader "Sprites/OutlineMeter"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha("Enable External Alpha", Float) = 0

		[Header(OutlineMeterProperties)]
		_MeterValue("Meter Value", Range(0, 1)) = 1
			_Speed("Color Speed", Float) = 10
		_OutlineColor("Outline Color", Color) = (1,1,0,1)
	}

		SubShader
	{
		Tags
	{
		"Queue" = "Transparent"
		"IgnoreProjector" = "True"
		"RenderType" = "Transparent"
		"PreviewType" = "Plane"
		"CanUseSpriteAtlas" = "True"
	}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
	{
		CGPROGRAM
#pragma vertex SpriteVert
#pragma fragment frag
#pragma target 2.0
#pragma multi_compile_instancing
#pragma multi_compile _ PIXELSNAP_ON
#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
#include "UnitySprites.cginc"


		struct VertexInput {
			float4 pos:	POSITION;
			float4 color:	COLOR;
			float2 uv:	TEXCOORD0;
		};

		struct VertexOutput {
			float4 sv_pos:	SV_POSITION;
			float4	color:	COLOR;
			float2 uv:	TEXCOORD0;
		};

		VertexOutput vert(VertexInput input) {
			VertexOutput output;
			output.sv_pos = UnityObjectToClipPos(input.pos);
			output.uv = input.uv;

			return output;
		}

		float4 _MainTex_TexelSize;
	float _MeterValue;
	float _Speed;
	fixed4 _OutlineColor;

	float4 frag(VertexOutput output) : SV_Target
	{

		float4 c = tex2D(_MainTex, output.uv) * output.color;
		c.rgb *= c.a;

		if (c.a <= _MeterValue) {
			return _OutlineColor + cos(_Time * _Speed - float4(0, 1, 0, 1));
		}
		return c;
	}


		ENDCG
	}
	}
}