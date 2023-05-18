﻿using System;
using UnityEngine.UI;

namespace Granden
{
    public class SingleClickFilter : IButton
    {
        public event Action OnClick;

        private readonly IStateValue<bool> isClicked;

        public SingleClickFilter(Button button, IStateValue<bool> isClicked)
        {
            this.isClicked = isClicked;

            button.onClick.AddListener(OnClickButtonHandler);
        }

        private void OnClickButtonHandler()
        {
            if (isClicked.State == true)
            {
                return;
            }

            isClicked.State = true;

            OnClick?.Invoke();
        }
    }
}