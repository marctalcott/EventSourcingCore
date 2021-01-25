using System.Threading.Tasks;

namespace Ezley.ProjectionStore
{
    public interface IViewRepository
    {
        Task<View> LoadViewAsync(string name);
        
        Task<T> LoadTypedViewAsync<T>(string name);

        Task<bool> SaveViewAsync(string name, View view);
    }
}