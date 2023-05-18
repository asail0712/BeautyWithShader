using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Granden.Core;

    namespace Granden.gwh
{ 
    public class BeautyInstaller : MonoBehaviour
    {
        public TMP_Dropdown _Dropdown;
        public RawImage _CamImg;
        public BeautyEffectHandler _BeautyEffectHandler;
        public Toggle _ToggleSwitch;
        public SliderAsset _SliderAsset;

        private OperatorHandler _OperatorHandler;
        private WebCamHandler _WebCamHandler;

        void Awake()
        {
            InitalWebCam();
            InitialOpeator();
        }

        void OnDestroy()
        {
            Heartbeat.UnregisterHaert(_OperatorHandler);
        }

        /**********************************************
        *   初始化操作面板
        *********************************************/
        private void InitialOpeator()
        {
            _OperatorHandler = new OperatorHandler();
            _OperatorHandler.Initial(_BeautyEffectHandler, _CamImg, _ToggleSwitch, _SliderAsset, _WebCamHandler.GetWebCamTexture);

            Heartbeat.RegisterHaert(_OperatorHandler);
        }

        /**********************************************
        *   初始化WebCam
        *********************************************/
        private void InitalWebCam()
        {
            _WebCamHandler = new WebCamHandler();
            _WebCamHandler.Initial(_CamImg, _Dropdown);
        }
    }
}