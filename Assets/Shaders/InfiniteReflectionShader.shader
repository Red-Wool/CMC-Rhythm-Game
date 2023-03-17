Shader "Custom/InfiniteReflectionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Background ("Background Texture", 2D) = "white" {}
        _ReflectionDistance("Reflection Distance", float) = 0
        _ReflectionAmount("Reflection Amount", float) = 0
        _ReflectionSetback("Reflection Setback", float) = 0
        _ReflectionMoveSpeed("Reflection Move Speed", float) = 0
        _ReflectionMoveInten("Reflection Move Intensity", float) = 0
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
            sampler2D _Background;

            float _ReflectionDistance;
            float _ReflectionAmount;
            float _ReflectionSetback;
            float _ReflectionMoveSpeed;
            float _ReflectionMoveInten;
            

            bool compVec3(float3 i, float3 j)
            {
                bool x = (i.x >= j.x),
                    y = (i.y >= j.y),
                    z = (i.z >= j.z);
                return x && y && z;
            }

            bool checkRange(float i, float low, float size, float lim)
            {
                return (i >= low && i <= low + size) ||
                    (low + size >= lim && i <= low + size % lim);
            }

            bool check(float i, float low, float size, float lim)
            {
                //If multiple removes
                //for (float x = 0.; x < 1.; x++)
                //{ //low + x * 10
                    if (checkRange(i, (low) % lim, size, lim))
                        return true;
                //}
                return false;
            }

            fixed4 clampGetTex(float2 uv, float size)
            {
                if (uv.x > 1 || uv.x < 0 || uv.y > 1 || uv.y < 0)
                    return fixed4(1, 1, 1, 1);
                return tex2Dgrad(_MainTex, size * (uv - (1. - (1. / size)) * .5), 0, 0);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = fixed4(1,1,1,1);//tex2D(_MainTex, i.uv);
                // just invert the colors
                //float3 col = vec3(1),//
                float2 result = i.uv + _Time.y * .1 + float2(cos(i.uv.x * 20.) * .5, 0);
                float4 endCol = tex2D(_Background, result);//cos(uv.xyx*.2+iTime*5. + vec3(0,2,4))*.5+.5;
                //texture(iChannel0, uv+.5 + d).xyz;

                float// t = mod(iTime,2.)-1.,
                    bg = _ReflectionAmount,
                    move = (_Time.y * _ReflectionMoveSpeed % _ReflectionDistance) * _ReflectionMoveInten,
                //low = ((_Time.y * 10.) % (bg + 1.)) - .5,//bg*(3. * pow(t,2.) - 2. * pow(t,3.)) - .1,
                //size = 15., //bg*(3. * pow(t,4.) - 2. * pow(t,5.)) + .1,
                key = bg;

                bool checkFlag = false;
                //float size;

                for (float x = -_ReflectionSetback; x < bg-_ReflectionSetback; x++)
                {
                    //if (check(x, low, size, bg + 1.))
                    //{
                    //    continue;
                    //}

                    if (compVec3(col, float3(.9,.9,.9)))
                    {
                        float size = (1. + x * _ReflectionDistance - move);
                        //(sin(iTime)*.2+.3) //(1.-(1./size))
                        col = clampGetTex(i.uv, size);//tex2Dgrad(_MainTex, clamp(size * (i.uv - (1. - (1. / size))*.5), 0., 1.),0,0);
                        key = x;
                    }
                    else
                    {
                        
                        break;
                    }
                }
                if (compVec3(col, float3(.9,.9,.9)))
                {
                    col = endCol;
                }
                //else if (key <= .1) {
                //    col = lerp(col, endCol, move / _ReflectionDistance);
                //}
                else
                {
                    col = lerp(col, endCol, (key-move/_ReflectionDistance) / bg);
                    
                }

                // Output to screen
                //fragColor = vec4(col, 1.0);
                return col;
            }
            ENDCG
        }
    }
}
