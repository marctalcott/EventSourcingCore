using System.Threading.Tasks;

namespace Ezley.SnapshotStore
{
    public interface ISnapshotStore
    {
        Task<Snapshot> LoadSnapshotAsync(string streamId);
  
        Task SaveSnapshotAsync(string streamId, int version, object snapshot);
    }
}