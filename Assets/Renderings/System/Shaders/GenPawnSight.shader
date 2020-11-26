Shader "Unlit/GenPawnSight"
{
	Properties
	{

	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "RenderQueue" = "Transparent" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite Off
		ZTest Off

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 local_position : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.local_position = v.vertex;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 pos = i.local_position.xyz;
				float distance = length(pos);
				float x = smoothstep(0.5f, 2.5f, distance) * 0.25f;

				fixed4 col = fixed4(0.86f, 0.65f, 0.34f, x);
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
		ENDCG
	}
	}
}
