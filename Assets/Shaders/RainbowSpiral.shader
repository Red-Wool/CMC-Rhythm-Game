Shader "Custom/RainbowSpiral"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlobSize("Blob Size", float) = 0
        _BlobStep("Blob Step", float) = 0
        _HueShiftIntensity("Hue Shift Intensity", float) = 0
        _LightIntensity("Light Intensity", float) = 0
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

            float _BlobSize;
            float _BlobStep;
            float _HueShiftIntensity;
            float _LightIntensity;

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

            float3 rgb2hcv(in float3 rgb)
            {
                // Credit: https://www.shadertoy.com/view/4dKcWK
                // RGB [0..1] to Hue-Chroma-Value [0..1]
                // Based on work by Sam Hocevar and Emil Persson
                float4 p = (rgb.g < rgb.b) ? float4(rgb.bg, -1., 2. / 3.) : float4(rgb.gb, 0., -1. / 3.);
                float4 q = (rgb.r < p.x) ? float4(p.xyw, rgb.r) : float4(rgb.r, p.yzx);
                float c = q.x - min(q.w, q.y);
                float h = abs((q.w - q.y) / (6. * c + 1e-10) + q.z);
                return float3(h, c, q.x);
            }

            float3 rgb2hsv(in float3 rgb)
            {
                // Credit: https://www.shadertoy.com/view/4dKcWK
                // RGB [0..1] to Hue-Saturation-Value [0..1]
                float3 hcv = rgb2hcv(rgb);
                float s = hcv.y / (hcv.z + 1e-10);
                return float3(hcv.x, s, hcv.z);
            }

            float3 hsv2rgb(in float3 c)
            {
                float3 rgb = clamp(abs(((c.x * 6.0 + float3(0, 4, 2)) % 6.) - 3.) - 1., 0., 1.);

                return c.z * lerp(float3(1,1,1), rgb, c.y);
            }

            float rand(float r)
            {
                return frac(sin(r * 728.731) * 847.315) * 38.92;
            }

            float metaCircle(float2 uv)
            {
                float minDist = 10.;
                float2 minPos = float2(0,0), f = frac(uv), i = floor(uv);

                for (int x = -2; x < 3; x++)
                {
                    for (int y = -2; y < 3; y++)
                    {
                        float2 c = i + float2(x, y),
                            d = sin(float2(rand(c.x * 38.4 + c.y) + _Time.y * .07, rand(c.y * 92.1 + c.x) + _Time.y * .06) + _Time.y * .2) * 1.,
                            p = c + d;
                        float dif = length(uv - p), v = clamp((minDist - dif) / .5 * .5 + .5, 0., 1.);
                        dif = lerp(minDist, dif, v) - .5 * v * (1. - v);

                        if (minDist > dif)
                        {
                            minPos = p;
                            minDist = dif;

                        }


                    }
                }

                return minDist;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                i.uv -= .5;
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                float3 hsv = rgb2hsv(col);

                float d = metaCircle(i.uv * 5.) + sin(length(i.uv) * 30. + atan2(i.uv.y, i.uv.x) * 10. + _Time.y * .3) * .05;
                d *= step(d, _BlobSize);
                hsv.z += d * _LightIntensity;
                hsv.x += (d * _HueShiftIntensity + step(1e-5, d) * step(d, _BlobSize - _BlobStep) * length(i.uv)) + (1. - frac(step(_BlobSize - _BlobStep, d) * _Time.y * .1));
                col = fixed4(hsv2rgb(hsv),0);

                return col;
            }
            ENDCG
        }
    }
}
