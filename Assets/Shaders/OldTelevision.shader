Shader "Custom/OldTelevision"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _ScanlineTex ("Scanline Texture", 2D) = "white" {}
        _Str ("Strength", float) = 0
        _Size ("Size", int) = 0
        _Speed ("Speed", float) = 0
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
            sampler2D _ScanlineTex;
            float _Str;
            int _Size;
            float _Speed;


            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float2 scanlineUV = i.uv * (_ScreenParams.y / _Size);
                scanlineUV.y += _Time[1] * _Speed;

                fixed4 scan = tex2D(_ScanlineTex, scanlineUV);

                col = lerp(col, col * scan, _Str);

                // just invert the colors
                
                return col;
            }
            ENDCG
        }
    }
}
