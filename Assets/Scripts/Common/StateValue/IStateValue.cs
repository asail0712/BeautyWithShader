namespace Granden
{
    public interface IStateValue<T>
    {
        T State { get; set; }
    }
}