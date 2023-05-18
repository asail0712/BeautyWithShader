namespace Granden
{
    public class ButtonToCommandAdapter
    {
        private readonly ICommand command;

        public ButtonToCommandAdapter(IButton button, ICommand command)
        {
            this.command = command;
            button.OnClick += OnClickButtonHandler;
        }

        private void OnClickButtonHandler()
        {
            command.Execute();
        }
    }
}