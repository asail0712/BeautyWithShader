
Shader "Custom/Effect/BilateralFilters"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurSize("BlurSize", Range(1,12)) = 1
        _SigmaS("SigmaS", Range(1,10)) = 5
        _SigmaR("SigmaR", Range(0.01,1)) = 0.09
        _Range("Range", Range(1,10)) = 2
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
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                float4 _MainTex_TexelSize;
                float _BlurSize;
                float _SigmaS;
                float _SigmaR;
                float _Range;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    o.uv = v.uv;

                    return o;
                }

                fixed4 bilater(v2f i)
                {
                    half sigmas2 = 2 * _SigmaS * _SigmaS;//函数中常量系数,取5
                    half sigmar2 = 2 * _SigmaR * _SigmaR;//函数中常量系数,取0.09
                    half fenzi_r = 0,fenzi_g = 0,fenzi_b = 0;
                    half fenmu_r = 0, fenmu_g = 0, fenmu_b = 0;
                    fixed4 col = tex2D(_MainTex,i.uv);

                    for (int m = 0; m < 2 * _Range + 1; m++)
                    {
                        half mpingfang = pow(m - _Range, 2);
                        for (int n = 0; n < 2 * _Range + 1; n++)
                        {
                            //_BlurSize为模糊级别，数值越大，模糊程度越高，图像失真也越大
                            fixed4 tcol = tex2D(_MainTex,i.uv + float2(_MainTex_TexelSize.x * (m - _Range), _MainTex_TexelSize.y * (n - _Range)) * _BlurSize);

                            fixed4 ncol = col - tcol;
                            half npingfang = pow((n - _Range),2);
                            half w_s = (mpingfang + npingfang) / sigmas2;
                            half wr = pow(2.718, -(w_s + ncol.r * ncol.r / sigmar2));//e常量=2.718...
                            half wg = pow(2.718, -(w_s + ncol.g * ncol.g / sigmar2));
                            half wb = pow(2.718, -(w_s + ncol.b * ncol.b / sigmar2));
                            fenmu_r += wr;
                            fenmu_g += wg;
                            fenmu_b += wb;
                            fenzi_r += wr * tcol.r;
                            fenzi_g += wg * tcol.g;
                            fenzi_b += wb * tcol.b;
                        }
                    }
                    return fixed4(fenzi_r / fenmu_r, fenzi_g / fenmu_g, fenzi_b / fenmu_b, col.a);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    return bilater(i);
                }
                ENDCG
            }
        }

            FallBack Off
}