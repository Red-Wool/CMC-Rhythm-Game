Shader "Custom/VortexShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _SideTex ("Side Texture", 2D) = "white" {}
        _Strength ("Strength", float) = 0
        _StepX ("X Step", float) = 0
        _StepY ("Y Step", float) = 0
        
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
            sampler2D _SideTex;

            float _Strength;
            float _StepX;
            float _StepY;

            bool compare2(float2 uv, float v)
            {
                return uv.x > v && uv.x < 1. - v && uv.y > v && uv.y < 1. - v;
            }

            float box(float2 uv, float2 c, float2 s)
            {
                float2 d = abs(uv - c);
                return step(d.x, s.x) * step(d.y, s.y);
            }

            fixed4 smallImage(float2 uv, float2 c, float2 size)
            {
                float2 d = abs(uv - c - size) / (size * 2.);
                float pct = box(uv, c, size);
                fixed4 col = fixed4(1,1,1,1);

                col = tex2Dgrad(_MainTex, d,0,0);
                if (!compare2(abs(d), .01))
                    col = tex2Dgrad(_SideTex, d,0,0);
                //else if (tex == 1)
                //    col = texture(iChannel2, d).xyz;

                return pct * col + (1. - pct) * fixed4(0,0,0,0);
            }

            float2 rot(float2 uv, float2 c, float a)
            {//
                return mul((c - uv), float2x2(
                    sin(a), cos(a),
                    -cos(a), sin(a)));
            }

            /*void mainImage(out vec4 fragColor, in float2 fragCoord)
            {
                // Normalized pixel coordinates (from 0 to 1)
                float2 uv = (fragCoord - float2(iResolution.x - iResolution.y, 0) * .5) / iResolution.y;

                float3 col;
                float low = 100.;
                // Time varying pixel color
                for (int i = 0; i < 10; i++)
                {
                    float s = mod(-10. * acos(cos(.1 * iTime)) * .2 + float(i) * .2, 2.);
                    float2 c = float2(cos(iTime * 0.6947) * pow(2. - s, 1.2) * .15, sin(iTime * 0.6947) * (2. - s) * .15) + .5,
                        p = rot(uv, c, s * .1 - 1.5707 + pow(s * 3., 1.4) * cos(float(i) * 3.1415));


                    float pct = box(p, float2(0, 0), float2(s, s));

                    if (s < low && pct > 0.)
                    {
                        low = s;
                        col = smallImage(p, float2(0, 0), float2(s, s));
                        continue;
                    }
                }
                fragColor = vec4(col, 1.0);
            }*/

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                //float3 col;
                float low = 100.;
                // Time varying pixel color
                for (int x = 0; x < 10; x++)
                {
                    //acos(cos(.1 * _Time.y)
                    float s = ((_Strength * _Time.y) * .2 + float(x) * .2);
                    if (s < 0) {
                        s += floor(abs(s) * .5) * 2. + 2.;
                    }
                    s %= 2.;
                    float2 c = float2(cos(_Time.y * 0.6947) * pow(2. - s, 1.2) * _StepX, sin(_Time.y * 0.6947) * (2. - s) * _StepY) + .5,
                       p = rot(i.uv, c, s * .1 + 1.5707 + pow(s * 3., 1.4) * cos(float(x) * 3.1415));


                    float pct = box(p, float2(0, 0), float2(s, s));

                    if (s < low && pct > 0.)
                    {
                       low = s;
                       col = smallImage(p, float2(0, 0), float2(s, s));
                    }
                }
                //fragColor = vec4(col, 1.0);
                return col;
            }
            ENDCG
        }
    }
}
