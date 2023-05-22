using System;

namespace Granden.Common
{
    public interface ICondition
    {
        void Evaluate(Action<bool> resultHandler);
    }
}