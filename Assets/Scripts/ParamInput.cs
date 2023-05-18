using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Granden.gwh;
using Granden;

public class ParamInput : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider _Slider;
    public TMP_InputField _InputText;

    void Awake()
    {
        if(_InputText == null || _Slider == null)
        {
            Debug.LogError($"{gameObject.name} ªì©l¤Æ¥¢±Ñ");
        }

        _InputText.onValueChanged.AddListener(OnInputFieldChange);
        _Slider.onValueChanged.AddListener(OnSliderChange);

        _InputText.text = _Slider.value.ToString();
    }

    private void OnInputFieldChange(string InputStr)
    {
        _Slider.value = float.Parse(InputStr);
    }
    private void OnSliderChange(float InputValue)
    {
        _InputText.text = InputValue.ToString();
    }

    public float GetParam(bool bIsVert = false)
    {
        if(bIsVert)
        {
            return _Slider.maxValue - _Slider.value + _Slider.minValue;
        }
        else
        {
            return _Slider.value;
        }        
    }
    public void SetParam(float Value)
    {
        _Slider.value   = Value;
        _InputText.text = Value.ToString();
    }

}
