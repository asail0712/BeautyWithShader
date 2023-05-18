using UnityEngine;

namespace Granden
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