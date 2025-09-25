Shader "Unlit/AudioReactive"
{
    Properties
    {
        _Scale ("Scale", Float) = 1.0
        _AudioSpectrum ("Audio Spectrum", Float) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            
            // Audio Reactive Properties
            float _Scale;
            float _AudioSpectrum;

            v2f vert (appdata v)
            {
                v2f o;
                
                float audioScale = _Scale + _AudioSpectrum;
                
                // scale.y
                float3 scaledVertex = v.vertex;
                scaledVertex.y = v.vertex.y * audioScale;
                
                o.vertex = UnityObjectToClipPos(scaledVertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                return fixed4(1, 1, 1, 1);
            }
            ENDCG
        }
    }
}
