
Shader "Custom/Face/Beauty"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _BlurTex("BlurTex", 2D) = "white" {}
        _GaussTex("GaussTex", 2D) = "white" {}
        _SkinTex("SkinTex", 2D) = "white" {}

        _SkinWhite("SkinWhite", Range(0,1)) = 0
        _Weight("Weight", Range(0,1)) = 0.2
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
                float4 _BlurTex_TexelSize;
                sampler2D _BlurTex;
                sampler2D _GaussTex;
                sampler2D _SkinTex;
                float _SkinWhite;
                float4 _Weight;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 skin(fixed4 col)
                {
                    half u = (-0.169 * col.r - 0.331 * col.g + 0.5 * col.b + 0.5) * 255;
                    half v = (0.5 * col.r - 0.419 * col.g - 0.081 * col.b + 0.5) * 255;

                    fixed t1 = saturate(sign(u - 80));
                    fixed t2 = saturate(sign(121 - u));
                    fixed t3 = saturate(sign(v - 124));
                    fixed t4 = saturate(sign(175 - v));

                    fixed t = sign(t1 * t2 * t3 * t4);
                    return fixed4(col.r, col.g, col.b, t);
                }

                half luminance(fixed4 color) {
                    return 0.2125 * color.r + 0.7154 * color.g + 0.0721 * color.b;
                }

                fixed4 bright(fixed4 col)
                {
                    //美颜提亮算法
                    half BrightLevel = 5;
                    half3 temp = (0,0,0);
                    temp.x = log(col.r * (BrightLevel - 1) + 1) / log(BrightLevel);
                    temp.y = log(col.g * (BrightLevel - 1) + 1) / log(BrightLevel);
                    temp.z = log(col.b * (BrightLevel - 1) + 1) / log(BrightLevel);
                    return  fixed4(temp, col.a);
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 col = tex2D(_MainTex, i.uv);      //原图
                    fixed4 cskin = tex2D(_SkinTex, i.uv);    //肤色Mask
                    fixed4 bilater = tex2D(_BlurTex, i.uv);  //双边过滤
                    fixed4 gauss = tex2D(_GaussTex, i.uv);   //高斯模糊
                    //按照我们的设想，只需要对肤色区域进行双边过滤，再提亮即可完成美颜
                    //而实际上，这样做的效果不算理想，因为双边过滤算法虽然是保边算法，但它不可能做到绝对保边
                    //因此，我们需要再给模糊后的纹理，增加脸部细节 
                    //主要算法原理： 
                    //1.原图 = 模糊 + 细节  --->  细节 = 原图 - 模糊   
                    //2.增强 = 模糊 + 细节 * k
                    //这一步具有很强的主观性，是试出来的 
                    //0.2 * (col - bilater)   是取原图双边过滤剩下的细节
                    //0.8 * (bilater - gauss) 是取原图双边过滤再高斯模糊剩下的细节
                    half4 nblur = bilater + _Weight * (col - bilater) + (1 - _Weight) * (bilater - gauss);
                    nblur.r = saturate(nblur.r);//防止颜色值溢出
                    nblur.g = saturate(nblur.g);
                    nblur.b = saturate(nblur.b);
                    //使用肤色Mask，如果是肤色区域，即取模糊值，否则取原图
                    fixed4 final = lerp(col, fixed4(nblur.rgb,1) , cskin.a);
                    //提亮
                    fixed4 cbright = bright(final);
                    //根据提亮级别插值
                    final = lerp(final, cbright , _SkinWhite);

                    final.a = 1;
                    return final;
                }

                ENDCG
            }
        }

            FallBack Off
}