using System.Linq;
using System.Threading.Tasks;

public partial class PersistentGameData
{
    public static class Software
    {
        public static async Task<SoftwareOwnershipStatus> GetStatusAsync(int softwareId)
        {
            var instance = await GetInstanceAsync();
            return instance.PlayerOwnedSoftware
                .FirstOrDefault(d => d.SoftwareId == softwareId)?.Status 
                ?? SoftwareOwnershipStatus.NotOwned;
        }

        public static async void SetStatus(int softwareId, SoftwareOwnershipStatus status)
        {
            var instance = await GetInstanceAsync();

            var datum = instance.PlayerOwnedSoftware
                .FirstOrDefault(d => d.SoftwareId == softwareId);

            if (datum == null)
            {
                datum = new PlayerSoftwareOwnershipDatum
                {
                    SoftwareId = softwareId,
                };
                instance.PlayerOwnedSoftware.Add(datum);
            }

            datum.Status = status;
        }

        public static async Task<int> GetCountAsync()
        {
            var instance = await GetInstanceAsync();

            return instance.PlayerOwnedSoftware
                .Where(x => x.Status == SoftwareOwnershipStatus.Owned)
                .Count();
        }
    }
}