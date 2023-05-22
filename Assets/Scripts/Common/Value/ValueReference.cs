namespace Granden.Common.Value
{
    public class ValueReference<T> : IValueReference<T>
    {
        public ValueReference(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }
}