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

		_AreaColor("AreaColor", Color) = (0,0,0,1)
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

			float _Soc;
			float _Int;
			float _Acq;
			float _Sta;
			
			float _DotRadius;
			float4 _DotColor;

			float _LineSize;
			float4 _LineColor;

			float4 _AreaColor;

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
				float4 col = float4(0,0,0,0);
				//Sta 上 Int 左 Soc 下 Acq 右
				float3 AcqDot = float3(0.5 + _Acq * 0.5f,0.5,0);
				float3 SocDot = float3(0.5,0.5 - _Soc * 0.5f,0);
				float3 IntDot = float3(0.5 - _Int * 0.5f,0.5,0);
				float3 StaDot = float3(0.5,0.5 + _Sta * 0.5f,0);

				float3 uv3 = float3(i.uv.x, i.uv.y, 0);
				//Area
				bool InArea = cross((StaDot - uv3), (AcqDot - StaDot)).z < 0 &&
					cross((AcqDot - uv3), (SocDot - AcqDot)).z < 0 &&
					cross((SocDot - uv3), (IntDot - SocDot)).z < 0 &&
					cross((IntDot - uv3), (StaDot - IntDot)).z < 0;
				col = InArea ? _AreaColor : col;

				//Dot
				bool isDot = length(i.uv - StaDot) < _DotRadius ||
					length(i.uv - IntDot) < _DotRadius ||
					length(i.uv - SocDot) < _DotRadius ||
					length(i.uv - AcqDot) < _DotRadius;
				col = isDot ? _DotColor : col;

				return col;
			}
			ENDCG
		}
	}
}
