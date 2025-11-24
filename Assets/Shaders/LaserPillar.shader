Shader "Custom/LaserPillar"
{
    Properties
    {
        _Color ("Laser Color", Color) = (0, 1, 1, 1)
        _EmissionStrength ("Emission Strength", Range(1, 10)) = 3.0
        _FresnelPower ("Fresnel Power", Range(0.1, 5)) = 2.0
        _AlphaFalloff ("Alpha Falloff", Range(0, 1)) = 0.8
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _CoreIntensity ("Core Intensity", Range(0, 1)) = 0.7
        _CoreSize ("Core Size", Range(0, 1)) = 0.4
    }
    
    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            "IgnoreProjector"="True" 
        }
        
        LOD 100
        
        ZWrite Off
        ZTest LEqual
        Blend SrcAlpha One
        Cull Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
                float3 normal : TEXCOORD2;
                float radialDist : TEXCOORD3;
            };
            
            float4 _Color;
            float _EmissionStrength;
            float _FresnelPower;
            float _AlphaFalloff;
            float _ScrollSpeed;
            float _CoreIntensity;
            float _CoreSize;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.normal = UnityObjectToWorldNormal(v.normal);
                
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                
                float2 centered = v.uv * 2.0 - 1.0;
                o.radialDist = length(centered);
                
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                float fresnel = 1.0 - saturate(dot(normalize(i.viewDir), normalize(i.normal)));
                fresnel = pow(fresnel, _FresnelPower);
                
                float scrollOffset = frac(_Time.y * _ScrollSpeed);
                float scanline = frac(i.uv.y * 5.0 + scrollOffset);
                scanline = smoothstep(0.3, 0.7, scanline);
                
                float verticalFade = 1.0 - pow(i.uv.y, _AlphaFalloff);
                
                float coreGradient = 1.0 - smoothstep(0.0, _CoreSize, i.radialDist);
                coreGradient = pow(coreGradient, 2.0);
                
                float edgeGlow = pow(1.0 - i.radialDist, 3.0);
                
                float3 whiteCore = float3(1, 1, 1);
                float3 baseColor = lerp(_Color.rgb, whiteCore, coreGradient * _CoreIntensity);
                float3 finalColor = lerp(baseColor, whiteCore, edgeGlow);
                
                float4 col;
                col.rgb = finalColor * _EmissionStrength * (1.0 + scanline * 0.3);
                col.a = (fresnel + coreGradient * 0.5 + edgeGlow * 0.3) * verticalFade * _Color.a;
                
                return col;
            }
            ENDCG
        }
    }
    
    Fallback Off
}

