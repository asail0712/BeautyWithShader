using System;

namespace Granden
{
    public class StateValueLazyAdapter<T> : Lazy<IStateValue<T>>, IStateValue<T>
    {
        public StateValueLazyAdapter(Func<IStateValue<T>> valueFactory) : base(valueFactory)
        {
        }

        public T State { get => Value.State; set => Value.State = value; }
    }
}