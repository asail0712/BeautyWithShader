using System;

namespace Granden
{
    public interface ICondition
    {
        void Evaluate(Action<bool> resultHandler);
    }
}