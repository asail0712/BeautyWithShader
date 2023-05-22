namespace Granden.Common.Value
{
    public interface IStateValue<T>
    {
        T State { get; set; }
    }
}