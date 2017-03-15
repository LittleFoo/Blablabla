
Shader "Custom/PixelProjection" {
    Properties {
        _MainColor ("Main Color", Color) = (0,0.4627451,1,1)
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        _ReflectionTex ("Reflection Tex", 2D) = "white" {}
        [HideInInspector] _saturation ("Saturation", range (0,1)) = 0.5
         _speed ("speed", range (0,50)) = 1//表示水波的扭曲强度   
         _Indentity ("Indentity", range (0,0.01)) = 0.1//表示水波的扭曲强度   
          _pixelGroup("pixelGroup", range (0,0.1)) =0.05//表示水波的扭曲强度  
          _fadeBegin("fadeBegin", range (0,1)) =0.4//表示水波的扭曲强度  
           _fadeEnd("fadeEnd", range (0,1)) =0.5//表示水波的扭曲强度  
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
         Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Lighting Off
//        GrabPass{ "Refraction" }
        Pass {
//            Name "FORWARD"
//            Tags {
//                "LightMode"="ForwardBase"
//            }
           
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
           // #pragma exclude_renderers d3d9 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
           // #pragma target 3.0
            uniform sampler2D Refraction;
            uniform sampler2D _CameraDepthTexture;
            uniform float4 _TimeEditor;
            uniform float4 _MainColor;
            uniform fixed _EnableReflections;
            uniform sampler2D _ReflectionTex; uniform float4 _ReflectionTex_ST;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 screenPos : TEXCOORD5;
                UNITY_FOG_COORDS(7)
            };
            
			float _saturation;
			float _WaveTile;
			sampler2D _WaveTex;
			half _speed;  
             half _Indentity;
            half _pixelGroup;
            float _fadeBegin;
            float _fadeEnd;
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                o.screenPos = o.pos;
                return o;
            }
            
	            float4 frag(VertexOutput i) : COLOR {
				float2 ruv = ((i.screenPos.rg)*0.5+0.5);
	             float ss = floor(ruv.y/_pixelGroup);//[0-50]
	       			ruv.x += (cos((_Time.x*_speed+ss*10)))*_Indentity;
	              float4 _ReflectionTex_var = tex2D(_ReflectionTex,ruv);
				_ReflectionTex_var = (_ReflectionTex_var * _MainColor);
				if(ruv.y > _fadeEnd || ruv.y < _fadeBegin)
					_ReflectionTex_var.a = 0;
				else 
					_ReflectionTex_var.a = _MainColor.a*(ruv.y - _fadeBegin)/(_fadeEnd - _fadeBegin);
			

// 					_ReflectionTex_var.r = 0;
// 					_ReflectionTex_var.g = 0;
// 					_ReflectionTex_var.b = frac(_Time.x);
                return _ReflectionTex_var;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
