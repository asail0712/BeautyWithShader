using System;

namespace Granden
{
    public interface IMessageBox
    {
        void Execute(Action ok, Action cancel);
    }
}