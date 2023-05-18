using System.Threading.Tasks;

namespace Granden
{
    public interface IDataProvider<T>
    {
        Task<T> GetAsync();
    }
}