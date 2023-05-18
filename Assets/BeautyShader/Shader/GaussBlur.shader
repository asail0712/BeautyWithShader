Shader "Custom/Effect/GaussBlur"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurSize("BlurSize", Range(0,20)) = 5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

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
                    float2 uv[5] : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                float _BlurSize;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    o.uv[0] = v.uv;
                    //高斯-x方向的模糊(y方向同理)
                    o.uv[1] = v.uv + float2(_MainTex_TexelSize.x * 1, 0) * _BlurSize;//_BlurSize模糊级别
                    o.uv[2] = v.uv - float2(_MainTex_TexelSize.x * 1, 0) * _BlurSize;
                    o.uv[3] = v.uv + float2(_MainTex_TexelSize.x * 2, 0) * _BlurSize;
                    o.uv[4] = v.uv - float2(_MainTex_TexelSize.x * 2, 0) * _BlurSize;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float weight[3] = {0.4026, 0.2442, 0.0545};

                    fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];

                    for (int m = 1; m < 3; m++)
                    {
                        sum += tex2D(_MainTex, i.uv[m * 2 - 1]).rgb * weight[m];
                        sum += tex2D(_MainTex, i.uv[m * 2]).rgb * weight[m];
                    }

                    return fixed4(sum, 1.0);
                }
                ENDCG
            }


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
                    float2 uv[5] : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                float _BlurSize;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    o.uv[0] = v.uv;

                    o.uv[1] = v.uv + float2(0, _MainTex_TexelSize.y * 1) * _BlurSize;
                    o.uv[2] = v.uv - float2(0, _MainTex_TexelSize.y * 1) * _BlurSize;
                    o.uv[3] = v.uv + float2(0, _MainTex_TexelSize.y * 2) * _BlurSize;
                    o.uv[4] = v.uv - float2(0, _MainTex_TexelSize.y * 2) * _BlurSize;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    float weight[3] = {0.4026, 0.2442, 0.0545};

                    fixed3 sum = tex2D(_MainTex, i.uv[0]).rgb * weight[0];

                    for (int m = 1; m < 3; m++)
                    {
                        sum += tex2D(_MainTex, i.uv[m * 2 - 1]).rgb * weight[m];
                        sum += tex2D(_MainTex, i.uv[m * 2]).rgb * weight[m];
                    }

                    return fixed4(sum, 1.0);
                }
                ENDCG
            }
        }

            FallBack Off
}