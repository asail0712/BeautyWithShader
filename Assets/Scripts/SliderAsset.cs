using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Granden.Common;
using Granden.Common.Click;

namespace Granden.BeautyWithShader
{
    public class SliderAsset : MonoBehaviour
    {
        public List<ParamInput> ParamList = new List<ParamInput>();
        public Button BackToDefaultBtn;

        // Start is called before the first frame update
        private void Awake()
        {
            for (int i = 0; i < ParamList.Count; ++i)
            {
                if (PlayerPrefs.HasKey($"ParamList {i}"))
                {
                    ParamList[i].SetParam(PlayerPrefs.GetFloat($"ParamList {i}"));
                }
            }

            new ButtonToCommandAdapter(
                    new SingleClickFilter(
                        BackToDefaultBtn,
                        GlobalState.IsClicked),
                    new Command2ActionAdapter(BackToDefault));
        }

        private void OnDestroy()
        {
            for (int i = 0; i < ParamList.Count; ++i)
            {
                PlayerPrefs.SetFloat($"ParamList {i}", ParamList[i].GetParam());
            }
        }

        void BackToDefault()
        {
            ParamList[0].SetParam(0f);
            ParamList[1].SetParam(.2f);
            ParamList[2].SetParam(2f);
            ParamList[3].SetParam(2f);
            ParamList[4].SetParam(0.2f);
            ParamList[5].SetParam(2f);
            ParamList[6].SetParam(3);
        }

    }
}