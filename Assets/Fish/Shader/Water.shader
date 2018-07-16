Shader "Custom/PerlinNoise" {
    Properties {
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
		_LightColor("LightColor",COLOR) = (1,1,1,1)
		_DeepColor("DeepColor",COLOR) = (1,1,1,1)
    }
    SubShader {
//        Tags { "RenderType"="Opaque" }
        LOD 200
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
		sampler2D _MainTex;
          uniform float4 _MainTex_ST;
	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = float2(v.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw);
		return o;
	}
//noise
	float2 random2(float2 st) {
		st = float2(dot(st, float2(127.1, 311.7)),
			dot(st, float2(269.5, 183.3)));
		return -1.0 + 2.0*frac(sin(st)*43758.5453123);
	}

	float noise(float2 st) {
		float2 i = floor(st);
		float2 f = frac(st);

		float2 u = f * f*(3.0 - 2.0*f);

		return lerp(lerp(dot(random2(i + float2(0.0, 0.0)), f - float2(0.0, 0.0)),
			dot(random2(i + float2(1.0, 0.0)), f - float2(1.0, 0.0)), u.x),
			lerp(dot(random2(i + float2(0.0, 1.0)), f - float2(0.0, 1.0)),
				dot(random2(i + float2(1.0, 1.0)), f - float2(1.0, 1.0)), u.x), u.y);
	}
	float3 mod289(float3 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
	float2 mod289(float2 x) { return x - floor(x * (1.0 / 289.0)) * 289.0; }
	float3 permute(float3 x) { return mod289(((x*34.0) + 1.0)*x); }


	float snoise(float2 v) {
		const float4 C = float4(0.211324865405187,  // (3.0-sqrt(3.0))/6.0
			0.366025403784439,  // 0.5*(sqrt(3.0)-1.0)
			-0.577350269189626,  // -1.0 + 2.0 * C.x
			0.024390243902439); // 1.0 / 41.0
		float2 i = floor(v + dot(v, C.yy));
		float2 x0 = v - i + dot(i, C.xx);
		float2 i1;
		i1 = (x0.x > x0.y) ? float2(1.0, 0.0) : float2(0.0, 1.0);
		float4 x12 = x0.xyxy + C.xxzz;
		x12.xy -= i1;
		i = mod289(i); // Avoid truncation effects in permutation
		float3 p = permute(permute(i.y + float3(0.0, i1.y, 1.0))
			+ i.x + float3(0.0, i1.x, 1.0));

		float3 m = max(0.5 - float3(dot(x0, x0), dot(x12.xy, x12.xy), dot(x12.zw, x12.zw)), 0.0);
		m = m * m;
		m = m * m;
		float3 x = 2.0 * frac(p * C.www) - 1.0;
		float3 h = abs(x) - 0.5;
		float3 ox = floor(x + 0.5);
		float3 a0 = x - ox;
		m *= 1.79284291400159 - 0.85373472095314 * (a0*a0 + h * h);
		float3 g;
		g.x = a0.x  * x0.x + h.x  * x0.y;
		g.yz = a0.yz * x12.xz + h.yz * x12.yw;
		return 130.0 * dot(m, g);
	}
    float water(){
    return 0;
    }
	float4 _LightColor;
	float4 _DeepColor;
		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = tex2D(_MainTex, i.uv);
		float2 pos = i.uv;
		pos.x = (pos.x -0.5)* (3 + pos.y * 20);
		pos.y = pos.y * (10+ pos.y*50) + 10;
		float2 vel = _Time;
		float o = snoise(pos + vel);
		float2 value = float2(0, 0);
		value.y = sin(_Time*0.02);
		float a = snoise(pos*value*3.1415);
		vel = float2(cos(a*0.1), sin(a));
		o += snoise(pos + vel);
		float m = (1 - pos.y/25) < 0 ? 0 : (1 - pos.y/25);
		float f = noise(o  * m);
		f = f < 0 ? 0 : f;
            col.w = 1;
			col.xyz =  _DeepColor.xyz +  _LightColor.xyz *(f + m) *col.w * col.xyz ;
			return col;
		}
		ENDCG
		}
    }
    FallBack "Diffuse"
}
/*
fixed4 frag(v2f i) : SV_Target
        {
            fixed4 col = tex2D(_MainTex, i.uv);
        float2 pos = i.uv;
        pos.x = (pos.x -0.5)* (3 + pos.y * 20);
        pos.y = pos.y * (10+ pos.y*50) + 10;
        float2 vel = _Time;
        float o = snoise(pos + vel);
        float2 value = float2(0, 0);
        value.y = sin(_Time*0.02);
        float a = snoise(pos*value*3.1415);
        vel = float2(cos(a*0.1), sin(a));
        o += snoise(pos + vel);
        float m = (1 - pos.y/25) < 0 ? 0 : (1 - pos.y/25);
        float f = noise(o  * m);
        f = f < 0 ? 0 : f;
            col.xyz =  _DeepColor.xyz +  _LightColor.xyz *(f+m) ;
            return col;
        }
*/