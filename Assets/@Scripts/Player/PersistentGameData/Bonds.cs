using System.Linq;
using System.Threading.Tasks;

public partial class PersistentGameData
{
    public static class Bonds
    {
        public static async Task<TeammateBond> GetAsync(int npcId)
        {
            var instance = await GetInstanceAsync();
            return Get(npcId);
        }

        public static TeammateBond Get(int npcId)
        {
            var existing = Instance.PlayerTeammateBonds
                .FirstOrDefault(x => x.NpcId == npcId);

            if (existing == null) 
                return TeammateBond.None;

            return existing.TeammateBond;
        }

        public static async Task SetAsync(int npcId, TeammateBond teammateBond)
        {
            var instance = await GetInstanceAsync();
            var existing = instance.PlayerTeammateBonds
                .FirstOrDefault(x => x.NpcId == npcId);

            if (existing == null)
            {
                existing = new()
                {
                    NpcId = npcId
                };
                instance.PlayerTeammateBonds.Add(existing);
            }

            existing.TeammateBond = teammateBond;
            _onTeammateBondUpdated?.Invoke(existing);
        }
    }
}