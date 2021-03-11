// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "CustomShader/NDeffius" 
{
	Properties
	{
		_DeffiuseColor("deffiuse color", color) = (1,1,1,1)
		_Gloss("gloss", Range(0.0, 255.0)) = 8.0
		_Ramp("ramp", 2D) = "white"{}
	}


	SubShader
	{
		Pass
		{
			Tags{"LightMode"="ForwardBase"}
			CGPROGRAM
			#include "Lighting.cginc"
			#pragma vertex vert
			#pragma fragment frag
			float4 _DeffiuseColor;
			float _Gloss;
			sampler2D _Ramp;
			sampler2D _Ramp_ST;

			struct a2v
			{
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float2 uv : TEXCOORD0;

			};
			struct v2f
			{
				float4 pos : SV_POSITION;
				float2 uv : TEXCOORD0;
				float4 worldNormal : TEXCOORD1;
				float4 worldLightDir : TEXCOORD2;
				float4 worldViewDir : TEXCOORD3;
			};

			v2f vert(a2v vinput)
			{
				v2f voutput; 
				voutput.worldNormal = float4(normalize(mul(vinput.normal.xyz, (float3x3)unity_ObjectToWorld)), 1);
				voutput.worldLightDir = normalize(_WorldSpaceLightPos0);
				voutput.worldViewDir = float4(normalize(_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, vinput.vertex).xyz), 1);
				voutput.uv = vinput.uv;
				voutput.pos = UnityObjectToClipPos(vinput.vertex);
				return voutput;
			}
			float4 frag(v2f finput) : SV_Target
			{
				float4 albedo = _DeffiuseColor;
				float4 ambient = UNITY_LIGHTMODEL_AMBIENT * albedo;
				float value = (dot(finput.worldNormal, finput.worldLightDir) + 1) / 2 ;
				float4 deffiuse = _LightColor0 * albedo * tex2D(_Ramp, float2(value, value)).r;

				float4 halfDir = normalize(finput.worldLightDir + finput.worldViewDir);

				value = 0;
				if ((pow(max(0, dot(finput.worldNormal.xyz, halfDir.xyz)), 8)) > 0.2)
				{
					value = 0.2;
				}
				float4 spacular = _LightColor0 * value;
				return ambient + deffiuse + spacular;
			}
			ENDCG
		}
	}
}