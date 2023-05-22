using Granden.Common.Value;

namespace Granden.Common.Click
{
    public static class GlobalState
    {
        public static readonly StateValue<bool> IsClicked;

        static GlobalState()
        {
            IsClicked = new StateValue<bool>(false);
        }
    }
}