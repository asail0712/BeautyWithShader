using System;
using UnityEngine;

namespace Granden.gwh
{
    public class BeautyEffectHandler : MonoBehaviour, IDisposable
    {
        public Material bilateralFilterMat;
        public Material gaussBlurMat;
        public Material skinCheckMat;
        public Material beautyMat;

        private Texture _OriTex;
        private Texture _Tex;
        private RenderTexture destRT;
        private RenderTexture srcRT;

        public bool Initial(int Width, int Height)
        {
            destRT  = new RenderTexture(Width, Height, 16, RenderTextureFormat.ARGB32);
            destRT.Create();

            srcRT   = new RenderTexture(Width, Height, 16, RenderTextureFormat.ARGB32);
            srcRT.Create();

            _Tex    = new Texture2D(Width, Height);

            return true;
        }

        public Texture ExecuteBeauty(Texture OriTex)
        {
            Graphics.Blit(OriTex, srcRT);
            ExecuteBeauty_Internal(srcRT, destRT);

            Graphics.ConvertTexture(destRT, _Tex);

            if (destRT != null)
            { 
                destRT.Release();
            }
            if (srcRT != null)
            { 
                srcRT.Release();
            }

            return _Tex;
        }

        public void Dispose()
        {
            if (_Tex != null)
            { 
                Destroy(_Tex);
            }
            DestroyImmediate(gameObject);
        }

        private void ExecuteBeauty_Internal(RenderTexture src, RenderTexture dest)
        {
            RenderTexture tex0 = RenderTexture.GetTemporary(src.width, src.height);
            RenderTexture tex1 = RenderTexture.GetTemporary(src.width, src.height);
            RenderTexture tex2 = RenderTexture.GetTemporary(src.width, src.height);
            RenderTexture tex3 = RenderTexture.GetTemporary(src.width, src.height);

            //双边过滤
            Graphics.Blit(src, tex0, bilateralFilterMat);
            //高斯模糊
            //对双边过滤结果进行x方向高斯模糊
            Graphics.Blit(tex0, tex1, gaussBlurMat, 0);
            //对双边过滤结果进行y方向高斯模糊
            Graphics.Blit(tex1, tex2, gaussBlurMat, 1);

            //对原图进行肤色识别，得到肤色mask
            Graphics.Blit(src, tex1, skinCheckMat, 0);
            //对肤色mask进行处理，减少噪点
            Graphics.Blit(tex1, tex3, skinCheckMat, 1);
            RenderTexture.ReleaseTemporary(tex1);

            //将几张纹理图传入beauty.shader
            //我们将在beauty.shader中进行处理
            beautyMat.SetTexture("_BlurTex", tex0);//双边过滤得到的纹理
            beautyMat.SetTexture("_GaussTex", tex2);//对双边过滤结果进行高斯模糊得到的纹理
            beautyMat.SetTexture("_SkinTex", tex3);//原图进行肤色识别
            Graphics.Blit(src, dest, beautyMat);

            RenderTexture.ReleaseTemporary(tex0);
            RenderTexture.ReleaseTemporary(tex2);
            RenderTexture.ReleaseTemporary(tex3);
        }

        public void AdjuctSkinWhite(float FloatValue)
        {
            beautyMat.SetFloat("_SkinWhite", FloatValue);
        }
        public void AdjuctBilateralWeight(float FloatValue)
        {
            beautyMat.SetFloat("_Weight", FloatValue);
        }public void AdjuctBilateralBlurSize(float FloatValue)
        {
            bilateralFilterMat.SetFloat("_BlurSize", FloatValue);
        }
        public void AdjuctBilateralBlurSpace(float FloatValue)
        {
            bilateralFilterMat.SetFloat("_SigmaS", FloatValue);
        }
        public void AdjuctBilateralBlurColor(float FloatValue)
        {
            bilateralFilterMat.SetFloat("_SigmaR", FloatValue);
        }
        public void AdjuctBilateralBlurRange(float FloatValue)
        {
            bilateralFilterMat.SetFloat("_Range", FloatValue);
        }
        public void AdjuctGuassBlurSize(float FloatValue)
        {
            gaussBlurMat.SetFloat("_BlurSize", FloatValue);
        }
    }

  
}