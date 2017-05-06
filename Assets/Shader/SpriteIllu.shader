// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/SpriteIllu"
{
	Properties
	{
		[PerRendererData] _MainTex ("Texture", 2D) = "white" {}
		_IlluTex ("Texture", 2D) = "white" {}
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
			

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				o.color = v.color;
				#ifdef PIXELSNAP_ON
				o.vertex = UnityPixelSnap (OUT.vertex);
				#endif
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _IlluTex;
			fixed4 _Color;
			//It is executed for each pixel and the output is the color info of the pixel.
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv)*i.color;
				col.rgb *= col.a;
				fixed4 col2 = tex2D(_IlluTex, i.uv)*_Color*i.color;
				col2.rgb*=col2.a;
				if(col.a == 0)
				return col2;
				else
				return col;
			}
			ENDCG
		}
	}
}
