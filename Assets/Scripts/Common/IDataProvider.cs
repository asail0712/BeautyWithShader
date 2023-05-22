using System.Threading.Tasks;

namespace Granden.Common
{
    public interface IDataProvider<T>
    {
        Task<T> GetAsync();
    }
}