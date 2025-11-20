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
            _onTeamMembersChanged?.Invoke();
        }

        public static async void Remove(int npcId)
        {
            var instance = await GetInstanceAsync();

            if (!instance.PlayerNpcTeamMembers.Contains(npcId))
                return;

            instance.PlayerNpcTeamMembers.Remove(npcId);
            _onTeamMembersChanged?.Invoke();
        }
    }
}