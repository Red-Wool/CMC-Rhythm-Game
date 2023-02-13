Shader "Custom/OutlineShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Step ("Step", float) = 0
        _Difference ("Difference", float) = 0

        [MaterialToggle] _Horizontal ("Horizontal Outline", int) = 0
        [MaterialToggle] _Vertical ("Vertical Outline", int) = 0
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

            float _Step;
            float _Difference;

            int _Horizontal;
            int _Vertical;

            /*
            void mainImage( out vec4 fragColor, in vec2 fragCoord )
{
    // Normalized pixel coordinates (from 0 to 1)
    vec2 uv = fragCoord/iResolution.xy;

    vec2 pos = uv + vec2(0,sin(iTime+uv.y*10.))*.3;

    // Time varying pixel color
    vec3 col = texture(iChannel0,pos).xxx;
    vec3 d = vec3(0,1,-1)*.03;
    vec3 r = texture(iChannel0, pos + d.yx).xxx;
    
    float dif = length(col - r);
    
    col = vec3(step(dif, .03));
    

    // Output to screen
    fragColor = vec4(col,1.0);
}
            */

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 dCol = tex2D(_MainTex, i.uv + float2(_Step,0));
                fixed4 uCol = tex2D(_MainTex, i.uv + float2(0,_Step));

                float dif = (_Horizontal ? length(col - dCol) : 0) + (_Vertical ? length(col - uCol) : 0), d = step(_Difference, dif);
                col.rgb = float3(d,d,d);
                // just invert the colors
                //col.rgb = 1 - col.rgb;
                return col;


            }
            ENDCG
        }
    }
}
