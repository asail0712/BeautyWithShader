Shader "Custom/MirrorShader" {
    Properties{
        _MainTex("Texture", 2D) = "white" {}
        _MirrorX("Mirror X", Range(0, 1)) = 0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                sampler2D _MainTex;
                float _MirrorX;

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target {
                    float2 mirrorUV = i.uv;
                    if (_MirrorX > 0)
                        mirrorUV.x = 1.0 - mirrorUV.x;

                    fixed4 col = tex2D(_MainTex, mirrorUV);
                    return col;
                }
                ENDCG
            }
        }
}