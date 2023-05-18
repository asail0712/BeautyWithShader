using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Granden.gwh
{
    public class WebCamHandler
    {
        private TMP_Dropdown _Dropdown;
        private RawImage _CamImg;

        private Quaternion _ImgBaseRotation;
        private WebCamDevice[] _Devices;
        private WebCamTexture _WebcamTexture;
        private AspectRatioFitter _RatioFitter;

        private int _DeviceIdx = 0;

        public WebCamHandler()
            : base()
        {

        }

        public bool Initial(RawImage CamImg, TMP_Dropdown Dropdown)
        {
            if (CamImg == null || Dropdown == null)
            {
                Debug.LogError("WebCamHandler ªì©l¤Æ¥¢±Ñ");
                return false;
            }

            _CamImg             = CamImg;
            _Dropdown           = Dropdown;

            _RatioFitter        = _CamImg.GetComponent<AspectRatioFitter>();
            _ImgBaseRotation    = _CamImg.transform.rotation;
            _Devices            = WebCamTexture.devices;

            List<string> DeviceList = new List<string>();
            for (int i = 0; i < _Devices.Length; i++)
            {
                Debug.Log(_Devices[i].name);
                DeviceList.Add(_Devices[i].name);
            }

            if (_Dropdown != null)
            {
                _Dropdown.ClearOptions();
                _Dropdown.AddOptions(DeviceList);
                _Dropdown.onValueChanged.AddListener(OnDropDownChanged);
            }

            UpdateWebCam(_DeviceIdx);

            return true;
        }

        public WebCamTexture GetWebCamTexture()
        {
            return _WebcamTexture;
        }

        private void OnDropDownChanged(int value)
        {
            UpdateWebCam(value);
        }
        private void UpdateWebCam(int idx)
        {
            _DeviceIdx = Mathf.Clamp(idx, 0, _Devices.Length - 1);
            if (_WebcamTexture != null)
            {
                _CamImg.enabled = false;
                _WebcamTexture.Stop();
                GameObject.Destroy(_WebcamTexture);
            }

            _WebcamTexture = new WebCamTexture(_Devices[_DeviceIdx].name);

            Debug.Log($"Update camera to {_Devices[_DeviceIdx].name}");

            _CamImg.material.mainTexture    = _WebcamTexture;
            _CamImg.enabled                 = true;

            _WebcamTexture.Play();

            float angle = AdjustRatation(_CamImg.transform, _WebcamTexture);
            FitImageSizeToCamSize(
                (Mathf.Abs(angle) == 90) ? AspectRatioFitter.AspectMode.HeightControlsWidth : AspectRatioFitter.AspectMode.WidthControlsHeight,
                _WebcamTexture);
        }

        private void FitImageSizeToCamSize(AspectRatioFitter.AspectMode mode, WebCamTexture camTexture)
        {
            float ratio = (float)camTexture.width / (float)camTexture.height;

            _RatioFitter.aspectMode     = mode;
            _RatioFitter.aspectRatio    = ratio;
            Debug.Log($"camTexture:{camTexture.width} {camTexture.height}");
        }

        private float AdjustRatation(Transform obj, WebCamTexture camTexture)
        {
            float angle     = camTexture.videoRotationAngle;
            obj.rotation    = _ImgBaseRotation * Quaternion.AngleAxis(angle, Vector3.back);
            Debug.Log($"camTexture.videoRotationAngle:{angle}");

            return angle;
        }

    }

}