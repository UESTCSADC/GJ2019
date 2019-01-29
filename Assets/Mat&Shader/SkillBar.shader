Shader "UI/SkillBar"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_ChangedTex("Texture1",2D) = "white" {}

		_FillColor("FillColor",Color) = (1,1,1,1)
		_SkillRate("Skill Rate",Float) = 0

		_MaskTop("Mask Top",Float) = 0
		_MaskBottom("Mask Buttom",Float) = 0
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

			sampler2D _MainTex;
			sampler2D _ChangedTex;
			float4 _MainTex_ST;

			float4 _FillColor;
			float _SkillRate;

			float _MaskTop;
			float _MaskBottom;
			
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
				bool isD = i.uv.x > _SkillRate;
				bool isVisibal =  i.vertex.y > _MaskTop && i.vertex.y < _MaskBottom;
				fixed4 col = isD ? tex2D(_MainTex, i.uv) : tex2D(_ChangedTex,i.uv);
				col = fixed4(_FillColor.x, _FillColor.y, _FillColor.z, col.w);
				col = isVisibal ? col : fixed4(i.vertex.y, 0, 0, 0);
				// apply fog
				return col;
			}
			ENDCG
		}
	}
}
