Shader "Unlit/Ripple"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _AspectRatio ("AspectRatio", Float) = 0.5625
        _Size ("Size", Float) = 0.05
        _Strength ("Strength", Float) = -0.1
        _WaveSpawnPosition ("Wave Spawn Position", Vector) = (0,0,0,0)
        _WaveDistanceFromCenter ("Wave Distance From Center", Float) = -0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

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

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _AspectRatio;
            float _Size;
            float _Strength;
            float2 _WaveSpawnPosition;
            float _WaveDistanceFromCenter;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float a = _WaveDistanceFromCenter + _Size;
                float b = _WaveDistanceFromCenter - _Size;
                float2 uv = i.uv - _WaveSpawnPosition;
                uv.x *= _AspectRatio;
                float smoothstep_value = smoothstep(b, a, length(uv));
                float minus_sv = 1 - smoothstep_value;

                float c = smoothstep_value * minus_sv;
                float d = normalize(uv) * _Strength;
                float e = c * d;
                float2 target_uv = i.uv + e;

                fixed4 col = tex2D(_MainTex, target_uv);
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
