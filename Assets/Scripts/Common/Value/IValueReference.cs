namespace Granden.Common.Value
{
    public interface IValueReference<T>
    {
        T Value { get; set; }
    }
}