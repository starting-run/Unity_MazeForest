Shader "Hygn/Glitch"
{
    Properties
    {   
        [Header(Main Settings)] [Space(10)]
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        [HDR] _Color ("Color", Color) = (1,1,1,1)
        _AlphaThreshold ("Alpha Clip Threshold", Range(0,1)) = 0.5
        _BorderWidth ("Border Width", Range(0,0.1)) = 0.1

        [Space(5)] [Header(Noise LUT)] [Space(10)]
        [NoScaleOffset] _NoiseLUT ("Noise Texture", 2D) = "grey" {}

        [Space(5)] [Header(Glitch)] [Space(10)]
        _XGlitchAmountX ("X axis Glitch Amount (X-Axis)", Range(0,3)) = 0.7
        _XGlitchAmountY ("X axis Glitch Amount (Y-Axis)", Range(0,1)) = 0.1

        _YGlitchAmountX ("Y axis Glitch Amount (X-Axis)", Range(0,1)) = 0.1
        _YGlitchAmountY ("Y axis Glitch Amount (Y-Axis)", Range(0,1)) = 0.2

        _GlitchStep ("Glitch Step", Range(1,10)) = 1

        _GlitchSpeed ("Glitch Speed", Range(1,64)) = 24
        _GlitchFreq ("Glitch Frequency", Range(0,1)) = 0.5

        _GlitchDutyCycle ("Glitch Duty Cycle", Range(0,1)) = 0.2
        
        [Space(5)] [Header(Chromatic Aberration)] [Space(10)]
        [Toggle] _CHROABRA ("Chromatic Aberration", float) = 1
        _ChromaticAbr ("Chromatic Aberration", Vector) = (0.8,1,1.2)
        
        [Space(5)] [Header(Scanline)] [Space(10)]
        [Toggle] _SCANLINE ("Scanline", float) = 1
        _ScanlineNumber ("Scanline Number", Float) = 64
        [NoScaleOffset] _ScanlineTex ("ScanLine Texture", 2D) = "White" {}
        _ScanlineBase ("Scanline Base", Range(0,1)) = 0
        [Space(5)] [Header(Scanning)] [Space(10)]
        _ScanSpeed ("Scanning Speed", Range(-5,5)) = 1
        _ScanBase ("Scannning Base", Range(0,1)) = 0.8
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #pragma shader_feature __ _SCANLINE_ON
            #pragma shader_feature __ _CHROABRA_ON

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex, _NoiseLUT, _ScanlineTex;
            float4 _Color, _ChromaticAbr;
            float _AlphaThreshold, _XGlitchAmountX, _XGlitchAmountY, _GlitchSpeed, _BorderWidth, _GlitchFreq, _GlitchDutyCycle;
            float _ScanlineNumber, _ScanlineBase, _GlitchStep, _ScanSpeed, _ScanBase, _YGlitchAmountX, _YGlitchAmountY;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uv *= 1 + _BorderWidth * 2;
                o.uv -= _BorderWidth;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float4 frag (v2f i) : SV_Target
            {   
                float Time = (_Time.y * _GlitchSpeed) - frac(_Time.y * _GlitchSpeed);
                Time /= _GlitchSpeed;

                float2 UVOffset = 0;
            #ifdef _SCANLINE_ON
                UVOffset.x = tex2D(_NoiseLUT, float2((i.uv.y * _XGlitchAmountY * _ScanlineNumber - frac(i.uv.y * _XGlitchAmountY * _ScanlineNumber)) /  _ScanlineNumber, frac(Time))).r;
                UVOffset.y = tex2D(_NoiseLUT, float2((i.uv.x * _YGlitchAmountX), frac(Time))).g;
            #endif

            #ifndef _SCANLINE_ON
                UVOffset.x = tex2D(_NoiseLUT, float2((i.uv.y * _XGlitchAmountY), frac(Time))).r;
                UVOffset.y = tex2D(_NoiseLUT, float2((i.uv.x * _YGlitchAmountX), frac(Time))).g;
            #endif
                
                float GlitchStep = floor(_GlitchStep) * 2 + 1;
                UVOffset = UVOffset * GlitchStep - frac(UVOffset * GlitchStep) - floor(_GlitchStep);
                UVOffset /= floor(_GlitchStep);
                UVOffset.x *= _BorderWidth * _XGlitchAmountX * 0.5;
                UVOffset.y *= _BorderWidth * _YGlitchAmountY* 0.5;

                float GlitchEnable = frac(_Time.y * _GlitchFreq) > (1 - _GlitchDutyCycle);
                UVOffset *= GlitchEnable;

                float4 col;

            #ifdef _CHROABRA_ON
                float2 GlitchUV_R = clamp(0, 1, i.uv + UVOffset * _ChromaticAbr.r);
                float2 GlitchUV_G = clamp(0, 1, i.uv + UVOffset * _ChromaticAbr.g);
                float2 GlitchUV_B = clamp(0, 1, i.uv + UVOffset * _ChromaticAbr.b);

                float2 col_R = tex2D(_MainTex, GlitchUV_R).ra;
                float2 col_G = tex2D(_MainTex, GlitchUV_G).ga;
                float2 col_B = tex2D(_MainTex, GlitchUV_B).ba;
                
                float3 alpha = float3((col_R.y > _AlphaThreshold),
                                      (col_G.y > _AlphaThreshold),
                                      (col_B.y > _AlphaThreshold));

                col.rgb = float3(col_R.x * alpha.r, 
                                 col_G.x * alpha.g, 
                                 col_B.x * alpha.b);

                col.a =  col_R.y * alpha.r;
                col.a += col_G.y * alpha.g;
                col.a += col_B.y * alpha.b;
                col.a /= alpha.r + alpha.g + alpha.b;
                col.a = clamp(col.a, 0, 1);
            #endif

            #ifndef _CHROABRA_ON
                float2 GlitchUV = clamp(0, 1, i.uv + UVOffset);
                col = tex2D(_MainTex, GlitchUV);
                col.a = col.a * (col.a > _AlphaThreshold);
            #endif
                
            #ifdef _SCANLINE_ON
                float scanline = tex2D(_ScanlineTex, float2(i.uv.x, i.uv.y * _ScanlineNumber)).r;
                scanline = lerp(_ScanlineBase, scanline, scanline > _ScanlineBase);
                float scan = frac(i.uv.y - _Time.y * _ScanSpeed);
                scanline *= lerp(_ScanBase, scan, scan > _ScanBase);
                col.a *= scanline;
            #endif

                col *= _Color;

                UNITY_APPLY_FOG(i.fogCoord, col);

                return col;
            }
            ENDCG
        }
    }
}
