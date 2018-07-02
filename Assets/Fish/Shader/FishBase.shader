﻿Shader "Custom/FishBase"
{
	Properties
	{
		_MainTex ("Body", 2D) = "white" {}

		_time("Time",Range(0,1)) = 0
		//_time("Center",Vector) = (0,0,0,0)
		//_amplitude("Amplitude",Range(0.1,2)) = 0

		_RotateCenter("Center",Vector) = (0,0,0,0)
        _Tail("Tail",Range(0,1)) = 0
		_Agg("Agg",Range(0,1)) = 0
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
			//float _amplitude = 1;
			float2 _RotateCenter;
			#define PI 3.1415926535897932384626433832795
			float3 rotate(float3 input,float2 center) {
				input.z += center.y;
				
				float t = (input.x + _time*2);//* _amplitude;
				float z = t > 0.5 && t < 1.5 ? sin(t * PI) : t <= 0.5 ? 1 : -1;
				input.x += center.x;
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
            float3 aim(float3 input,float angle){
                input.z = cos(angle*PI / 180)*input.z;
                input.x = input.x + sin(angle*PI / 180)*input.z;
                return input;
            }
			float _Tail;
			float _Agg;
			float3 bend(float3 input,float t) {
				//_Tail = _Time * 20;
				float2 xz = CalculatePosition(
				float2(-0.5 + (sin(_Tail * PI * 4)) * _Agg/10, cos(_Tail * PI * 2) * _Agg / 5),
				float2(0, -cos(_Tail * PI * 2 - PI/2) * _Agg / 10),
				float2(0.5, 0),
                t
				);
				input.x = xz.x;
				input.z = xz.y+ input.z ;
                input = aim(input, cos(_Tail * PI * 2  )*(1-t)*40 * _Agg);
				return input;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(bend(rotate(v.vertex,_RotateCenter.xy),v.uv));
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
