Shader "Custom/ScreenPanelShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PanelTex ("Panel Texture", 2D) = "white" {}
        _PanelPosition ("Panel Position", float) = 0
        _PanelSize ("Panel Size", float) = 0
        _PanelSmoothSize ("Panel Smooth Size", float) = 0

        _InitValue ("Initial FBM Value", float) = 0
        _MultValue ("Multiplier FBM Value", float) = 0
        _InitLan ("Initial FBM Lancularity", float) = 0
        _MultLan ("Multiplier FBM Lancularity", float) = 0
        _MultTime ("Multiplier Time", float) = 0
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            sampler2D _PanelTex;
            
            float _PanelPosition;
            float _PanelSize;
            float _PanelSmoothSize;

            float _InitValue;
            float _MultValue;
            float _InitLan;
            float _MultLan;
            float _MultTime;

            float hash(float t)
            {
                return sin((sin(t * 284.17) * 38.5811 % 1) * 92.713);
            }

            float noise(float t)
            {
                float i = floor(t);
                return lerp(hash(i), hash(i + 1.), t%1);
            }

            float fbm(float t)
            {
                //float a = .1, l = .6, m = 1.3, n = 2., tm = .3, v = .5;
                float a = _InitValue, l = _MultValue, m = _InitLan, n = _MultLan, tm = _MultTime, v = _PanelPosition+.5;
                int i = 0;
                for (; i++ < 8;)
                {
                    v += noise(t * m + _Time.y * float(i) * tm + m) * a;
                    a *= l;
                    m *= n;
                }

                return v;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Normalized pixel coordinates (from 0 to 1)
                float2 shift = i.uv + _Time.y * .02 + float2(cos(i.uv.y + _Time.y*.2) * .2, cos(i.uv.y * 8. + _Time.y * .4) * .8);

                float f = fbm(i.uv.y); // p = sin(iTime) * .15 + .2, d = .15;

                // Time varying pixel color
                fixed4 col;
                float pct = smoothstep(i.uv.x + _PanelSize, i.uv.x + _PanelSize + _PanelSmoothSize, f) + smoothstep(f + _PanelSize, f + _PanelSize + _PanelSmoothSize, i.uv.x);
                col = pct * tex2D(_PanelTex, shift) + (1. - pct) * tex2D(_MainTex, i.uv);

                return col;
                // Output to screen
                //fragColor = vec4(col, 1.0);
            }

            /*fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                col.rgb = 1 - col.rgb;
                return col;
            }*/
            ENDCG
        }
    }
}
