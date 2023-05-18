Shader "Custom/Face/SkinCheck"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
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

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 check(fixed4 col)
            {
                //使用的是ycbcr颜色模型,一般肤色会在这个区间内
                //也可以使用RGB颜色模型，我试了下，感觉上面更准确
                half u = (-0.169 * col.r - 0.331 * col.g + 0.5 * col.b + 0.5) * 255;
                half v = (0.5 * col.r - 0.419 * col.g - 0.081 * col.b + 0.5) * 255;

                fixed t1 = saturate(sign(u - 80));
                fixed t2 = saturate(sign(121 - u));
                fixed t3 = saturate(sign(v - 124));
                fixed t4 = saturate(sign(175 - v));

                //肤色区域 t=1
                fixed t = sign(t1 * t2 * t3 * t4);

                //只显示肤色区域
                //return col * t;

                //记录下肤色区域 t = 1
                return fixed4(col.rgb, t);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return check(col);
            }
            ENDCG
        }

        Pass
        {
                //降噪;
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
                    float2 uv[9] : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                fixed4 _MainTex_TexelSize;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    half size = 1;
                    for (int m = 0; m < 2; m++)
                    {
                        for (int n = 0; n < 2; n++)
                        {
                            float x = _MainTex_TexelSize.x * (n - 1);
                            float y = _MainTex_TexelSize.y * (1 - m);
                            o.uv[m * 3 + n] = v.uv + float2(x, y) * size;
                        }
                    }

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    fixed4 color = tex2D(_MainTex, i.uv[4]);

                    half alpha = 0;

                    for (int m = 0; m < 2; m++)
                    {
                        for (int n = 0; n < 2; n++)
                        {
                            fixed4 col = tex2D(_MainTex, i.uv[m * 3 + n]);
                            alpha += col.a;
                        }
                    }

                    half a0 = saturate((alpha - color.a - 0.5) * 10);//周围全黑;
                    half a1 = 1 - saturate((alpha - color.a - 7.5) * 10);//周围全白;

                    return color * a0 * a1;
                    //return fixed4(color.rgb, color.a * a0 * a1);
                }
                ENDCG
            }

            Pass
            {
                    //降噪---除去肤色小块;
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
                    fixed4 _MainTex_TexelSize;

                    v2f vert(appdata v)
                    {
                        v2f o;
                        o.vertex = UnityObjectToClipPos(v.vertex);
                        o.uv = v.uv;

                        return o;
                    }

                    fixed isskin(v2f i)
                    {
                        float r = min(_ScreenParams.x, _ScreenParams.y);
                        r = round(r * 0.2);
                        int step = max(1, round(r * 0.1));

                        half rate = 1;
                        //向四个方向射出射线;
                        for (int m = 0; m < 5; m++)
                        {
                            half alpha = 0;
                            half count = 0.01;

                            for (int n = 0; n < r; n += step)
                            {
                                float x = n * ((m + 1) % 2) * sign(1 - m);
                                float y = n * (m % 2) * sign(2 - m);

                                count += 1;
                                alpha += tex2D(_MainTex, i.uv + float2(x * _MainTex_TexelSize.x, y * _MainTex_TexelSize.y)).a;
                            }

                            //采样75%都是肤色,说明这个区域是脸部;
                            rate = rate * saturate((0.9 - alpha / count) * 1000);

                        }

                        return 1 - rate;
                    }


                    fixed4 frag(v2f i) : SV_Target
                    {
                        fixed4 color = tex2D(_MainTex, i.uv);

                        return color * color.a * isskin(i);
                        //return fixed4(color.rgb, color.a * rate);
                    }
                    ENDCG
                }
    }
}