// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Dissolve"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        [NoScaleOffset] _FlowMap ("Flow (RG)", 2D) = "black" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
        _Dissolve ("Dissolve", Range(0,1)) = 0
        _UVScale ("UV Scale", Float) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
            sampler2D _NoiseTex;
            sampler2D _FlowMap;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;
            float _Dissolve;
            float _UVScale;

			fixed4 frag(v2f IN) : SV_Target
			{
                float d = tex2D(_NoiseTex, IN.texcoord * _UVScale).r;
                if (d <= _Dissolve)
                    discard;
                float2 flowVector = (tex2D(_FlowMap, IN.texcoord).rg - 0.5)*2;
                float4 a = tex2D(_MainTex, IN.texcoord - flowVector * _Dissolve/2);
				fixed4 c = IN.color * a;
                //c.rgb *= IN.color * a;
				return c;
			}
		ENDCG
		}
	}
}