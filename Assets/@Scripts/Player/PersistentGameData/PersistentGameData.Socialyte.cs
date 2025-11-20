using System.Collections.Generic;
using System.Linq;

public partial class PersistentGameData
{
    /// <summary>
    /// API for 
    /// </summary>
    public static class Socialyte
    {
        /// <summary>
        /// Set the visibility of an NPC connection in the Socialyte App
        /// </summary>
        /// 
        /// <param name="npcId">
        /// The Profile ID of the NPC. 
        /// Lives at <see cref="SocialyteProfileStaticDatum.ProfileId"/>
        /// </param>
        public static async void SetConnectionVisible(int npcId, bool isVisible)
        {
            await GetInstanceAsync();
            var connection = GetConnectionDatum(npcId);

            if (connection.IsVisible == isVisible) 
                return;

            connection.IsVisible = isVisible;
            _onSocialyteProfileUpdated?.Invoke(connection);
        }

        private static NpcConnectionDatum GetConnectionDatum(int npcId)
        {
            var existing = Instance.PlayerNpcConnections
                .FirstOrDefault(x => x.NpcId == npcId);

            if (existing == null)
            {
                existing = new NpcConnectionDatum
                {
                    NpcId = npcId
                };

                Instance.PlayerNpcConnections.Add(existing);
            }

            return existing;
        }

        /// <returns>True if the player is already conencted to this NPC via Socailyte</returns>
        public static bool ConnectionExists(int npcId)
        {
            var existing = Instance.PlayerNpcConnections
                .FirstOrDefault(x => x.NpcId == npcId);

            return existing != null;
        }

        public static IReadOnlyList<NpcConnectionDatum> GetAll()
        { 
            return Instance.PlayerNpcConnections.AsReadOnly();
        }
    }
}