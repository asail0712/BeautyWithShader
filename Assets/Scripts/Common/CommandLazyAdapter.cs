using System;

namespace Granden.Common
{
    public class CommandLazyAdapter : Lazy<ICommand>, ICommand
    {
        public CommandLazyAdapter(Func<ICommand> valueFactory) : base(valueFactory)
        {
        }

        public void Execute()
        {
            Value.Execute();
        }
    }
}