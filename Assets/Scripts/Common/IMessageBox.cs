using System;

namespace Granden.Common
{
    public interface IMessageBox
    {
        void Execute(Action ok, Action cancel);
    }
}