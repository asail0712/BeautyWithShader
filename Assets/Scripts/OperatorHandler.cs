using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Granden.Core;

namespace Granden.BeautyWithShader
{
    public enum InputParamType
    {
        SkinWhite           = 0,
        BilateralWeight,
        BilateralBlurSize,
        BilateralBlurSpace,
        BilateralBlurColor,
        BilateralBlurRange,
        GuassBlurSize,
        NumOfType
    }

    public class OperatorHandler: IHeart
    {
        private BeautyEffectHandler _BeautyEffect;
        private Texture _CameraTex;
        private Toggle _ToggleSwitch;
        private RawImage _CamImg;
        private SliderAsset _SliderAsset;
        private Func<WebCamTexture> _GetWebCamTexFunc;

        public OperatorHandler()
            : base()
        {

        }

        public bool Initial(BeautyEffectHandler BeautyEffect, RawImage CamImg, Toggle ToggleSwitch, SliderAsset Slider, Func<WebCamTexture> GetWebCamTexFunc)
        {
            if (BeautyEffect == null 
                || CamImg == null 
                || ToggleSwitch == null 
                || Slider == null 
                || Slider.ParamList.Count != (int)InputParamType.NumOfType
                || GetWebCamTexFunc == null)
            {
                Debug.LogError("OperatorHandler 初始化缺少元件");

                return false;
            }

            _BeautyEffect           = BeautyEffect;
            _CamImg                 = CamImg;
            _ToggleSwitch           = ToggleSwitch;
            _SliderAsset            = Slider;
            _GetWebCamTexFunc       = GetWebCamTexFunc;

            int width   = _GetWebCamTexFunc().width;
            int height  = _GetWebCamTexFunc().height;

            _BeautyEffect.Initial(width, height);
            
            return true;
        }

        public void UpdateHeart(float DeltaTime)
        {
            AdjustParam();

            if (_ToggleSwitch.isOn)
            {
                _CamImg.texture = _BeautyEffect.ExecuteBeauty(_GetWebCamTexFunc());
            }
            else
            {
                _CamImg.texture = _GetWebCamTexFunc();
            }            
        }

        private void AdjustParam()
        {
            for(int i = 0;i < _SliderAsset.ParamList.Count; ++i)
            {
                ParamInput ParamUnit = _SliderAsset.ParamList[i];

                InputParamType Type = (InputParamType)i;

                switch (Type)
                {
                    case InputParamType.SkinWhite:
                        _BeautyEffect.AdjuctSkinWhite(ParamUnit.GetParam());
                        break;
                    case InputParamType.BilateralWeight:
                        _BeautyEffect.AdjuctBilateralWeight(ParamUnit.GetParam());
                        break;
                    case InputParamType.BilateralBlurSize:
                        _BeautyEffect.AdjuctBilateralBlurSize(ParamUnit.GetParam());
                        break;
                    case InputParamType.BilateralBlurSpace:
                        _BeautyEffect.AdjuctBilateralBlurSpace(ParamUnit.GetParam());
                        break;
                    case InputParamType.BilateralBlurColor:
                        _BeautyEffect.AdjuctBilateralBlurColor(ParamUnit.GetParam());
                        break;
                    case InputParamType.BilateralBlurRange:
                        _BeautyEffect.AdjuctBilateralBlurRange(ParamUnit.GetParam());
                        break;
                    case InputParamType.GuassBlurSize:
                        _BeautyEffect.AdjuctGuassBlurSize(ParamUnit.GetParam());
                        break;
                }
            }
        }
    }
}