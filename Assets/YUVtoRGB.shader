Shader "Custom/YUVtoRGB"
{
    Properties
    {
        _YTex ("Y Texture", 2D) = "white" {}
        _UTex ("U Texture", 2D) = "white" {}
        _VTex ("V Texture", 2D) = "white" {}
    }
    SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _YTex;
            sampler2D _UTex;
            sampler2D _VTex;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float y = tex2D(_YTex, i.texcoord).r;
                float u = tex2D(_UTex, i.texcoord).r - 0.5;
                float v = tex2D(_VTex, i.texcoord).r - 0.5;

                float r = y + 1.402 * v;
                float g = y - 0.344136 * u - 0.714136 * v;
                float b = y + 1.772 * u;

                return fixed4(r, g, b, 1.0);
            }
            ENDCG
        }
    }
}
