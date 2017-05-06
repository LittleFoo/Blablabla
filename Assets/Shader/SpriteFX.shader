// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "ProjZombie/SpriteFX"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_GrayTex ("GrayTexture", 2D) = "white" {}
		_ColorTex ("ColorTexture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_standard ("Standard", Color) = (0,1,0,1)
		_newBlack ("NewBlack", range (0,1)) = 0.1
		_newWhite ("NewWhite", range (0,1)) = 0.9
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
		// No culling or depth
		ZWrite Off
  		Cull Off
  		Blend One OneMinusSrcAlpha
  		

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
				float4 color    : COLOR;
			};
			
			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				fixed4 color    : COLOR;
			};
			
			fixed4 _Color;
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color * _Color;
				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (OUT.vertex);
				#endif
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _GrayTex;
			sampler2D _ColorTex;
			fixed4 _standard;
			fixed _newBlack;
			fixed _newWhite;
			//It is executed for each pixel and the output is the color info of the pixel.
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 grayCol = tex2D(_GrayTex, i.uv);
				fixed grayscale = grayCol.r*_standard.r + grayCol.g*_standard.g;
				grayscale = clamp(grayscale, _newBlack, _newWhite);
				grayscale = grayscale - _newBlack/(_newWhite-_newBlack);
				i.uv = float2(grayscale, _standard.b);
				fixed4 col = tex2D(_ColorTex, i.uv)*i.color;
				col.a = step(_standard.b, grayscale);
				col.rgb *= col.a;
				return col;
			}
			ENDCG
		}
	}
}
