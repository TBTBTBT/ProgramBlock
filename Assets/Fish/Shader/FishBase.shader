Shader "Custom/FishBase"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_time("Time",Range(0,1)) = 0
		//_time("Center",Vector) = (0,0,0,0)
		_amplitude("Amplitude",Range(0.1,2)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
		Tags{ "RenderType" = "Transparent" "Queue" = "Transparent" }
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
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
			float _time;
			float _amplitude;
			#define PI 3.1415926535897932384626433832795
			float3 rotate(float3 input) {
				float t = (input.x + _time*2)* _amplitude;
				float z = t > 0.5 && t < 1.5 ? sin(t * PI) : t <= 0.5 ? 1 : -1;
				input.z *= z;
				return input;
			}
			float2 CalculatePosition(float2 x1, float2 x2, float2 x3, float t)
			{
				if (t >= 0 && t <= 1)
					return pow(1 - t, 3) * x1
					+ 3 * pow(1 - t, 2) * t * x2
					+ 3 * (1 - t) * t * t * x2
					+ t * t * t * x3;
				else return float2(0, 0);
			}

			float3 bend(float3 input) {

				float2 xz = CalculatePosition(
				input.xz,
				float2(0.5, 0),
				input.xz,
				input.x+_time
				);
				input.x = xz.x;
				input.z = xz.y;
				return input;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(rotate(v.vertex));
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
