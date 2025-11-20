using UnityEngine;

public partial class PersistentGameData
{
    public static class TeamMembers
    {
        public static async void Add(int npcId)
        {
            var instance = await GetInstanceAsync();

            if (instance.PlayerNpcTeamMembers.Contains(npcId))
                return;

            instance.PlayerNpcTeamMembers.Add(npcId);
            _onTeamMembersUpdated?.Invoke();
        }

        public static async void Remove(int npcId)
        {
            var instance = await GetInstanceAsync();

            if (!instance.PlayerNpcTeamMembers.Contains(npcId))
                return;

            instance.PlayerNpcTeamMembers.Remove(npcId);
            _onTeamMembersUpdated?.Invoke();
        }

        public static bool IsInParty(int npcId)
        {
            return Instance.PlayerNpcTeamMembers.Contains(npcId);
        }
    }
}