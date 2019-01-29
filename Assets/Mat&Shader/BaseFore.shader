Shader "UI/BaseFore"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" { }

		_Soc("社交", Float) = 0
		_Int("智力", Float) = 0
		_Acq("才艺", Float) = 0
		_Sta("体能", Float) = 0

		_DotRadius("DotRadius", Float) = 0
		_DotColor("DotColor", Color) = (0,0,0,1)

		_LineSize("LineSize",Float) = 0
		_LineColor("LineColor", Color) = (0,0,0,1)
	}
	SubShader
	{
		Tags{ "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float4 _MainTex_ST;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				

				return col;
			}
			ENDCG
		}
	}
}
