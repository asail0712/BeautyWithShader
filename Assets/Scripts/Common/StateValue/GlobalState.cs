namespace Granden
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