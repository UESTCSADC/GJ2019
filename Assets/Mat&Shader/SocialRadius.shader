Shader "Sprites/SocialRadius"
{
	Properties
	{
		_MainTex("Sprite Texture", 2D) = "white" { }

		_Center("Center", Vector) = (0,0,0,0)
		_BaseRadius("Base Radius",Float) = 0
		_CoreRadius("Core Radius",Float) = 0

		_BaseColor("Base Color",Color) = (1.0,1.0,1.0,1.0)
		_BaseLineColor("Base Line Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_BaseLineSize("Base Line", Float) = 1.0

		_CoreColor("Core Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_CoreLineColor("Core Line Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_CoreLineSize("Core Line", Float) = 1.0
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
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
			};

			float4 _Center;

			float _BaseRadius;
			float _CoreRadius;

			float4 _BaseColor;
			float4 _BaseLineColor;
			float _BaseLineSize;

			float4 _CoreColor;
			float4 _CoreLineColor;
			float _CoreLineSize;

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float radius = length(i.vertex.xy - _Center.xy);
				bool core = radius < _CoreRadius;
				bool coreLine = radius < (_CoreRadius + _CoreLineSize);
				bool base = radius < _BaseRadius;
				bool baseLine = radius < (_BaseLineSize + _BaseRadius);

				float4 col = baseLine ? _BaseLineColor : float4(0, 0, 0, 0);
				col = base ? _BaseColor : col;
				col = coreLine ? _CoreLineColor : col;
				col = core ? _CoreColor : col;
				
				return col;
			}
			ENDCG
		}
	}
}
