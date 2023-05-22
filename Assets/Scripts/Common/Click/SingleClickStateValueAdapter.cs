using UnityEngine;
using Granden.Common.Value;

namespace Granden.Common.Click
{
    public class SingleClickStateValueAdapter : MonoBehaviour, IStateValue<bool>
    {
        private IStateValue<bool> stateValue;

        public bool State { get => stateValue.State; set => stateValue.State = value; }

        private void Awake()
        {
            stateValue = GlobalState.IsClicked;
        }
    }
}