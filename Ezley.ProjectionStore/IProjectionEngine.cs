using System.Threading.Tasks;

namespace Ezley.ProjectionStore
{
    public interface IProjectionEngine
    {
        void RegisterProjection(IProjection projection);

        Task StartAsync(string instanceName);

        Task StopAsync();
    }
}