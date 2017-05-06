// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ProjZombie/SpritesExt"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
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
				half2 texcoord  : TEXCOORD0;
				float4 ref : TEXCOORD1;
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

				OUT.ref = ComputeScreenPos(OUT.vertex);

				return OUT;
			}

			sampler2D _MainTex;
//			sampler2D _AlphaTex;
//			float _AlphaSplitEnabled;
//			uniform sampler2D _DistortionTex;
//			uniform float _DistortionAmp;

			uniform sampler2D _DecalTex;

			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
//				if (_AlphaSplitEnabled)
//					color.a = tex2D (_AlphaTex, uv).r;

				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
//				fixed4 dis = tex2Dproj(_DistortionTex, UNITY_PROJ_COORD(IN.ref));
//				fixed3 uvOffset = UnpackNormal(dis);
//				dis *= _DistortionAmp;

//				float2 uv = float2(IN.texcoord.x + dis.x, IN.texcoord.y + dis.y);
				float2 uv = float2(IN.texcoord.x, IN.texcoord.y);

				fixed4 c = SampleSpriteTexture (uv) * IN.color;

				fixed4 decalColor = tex2Dproj(_DecalTex, UNITY_PROJ_COORD(IN.ref));

				c.rgb += decalColor.rgb ;
				c.rgb *= c.a;
								
				return c;
			}
		ENDCG
		}
	}
}
