using System.Linq;

public partial class PersistentGameData
{
    public static class Socialyte
    {
        /// <summary>
        /// Add a new NPC connection in the Socialyte app. Will log a warning if
        /// the NPC already exists as a connection. Check if the connection already exists
        /// by calling <see cref="ConnectionExists(int)"/>
        /// </summary>
        /// <param name="npcId">The Profile ID of the NPC. 
        /// Lives at <see cref="SocialyteProfileStaticDatum.ProfileId"/></param>
        public static async void AddConnection(int npcId)
        {
            var instance = await GetInstanceAsync();

            var existing = instance.PlayerNpcConnections
                .FirstOrDefault(x => x.NpcId == npcId);

            if (existing != null)
            {
                instance.Log($"NPC id {npcId} has already been added as a socialyte connection");
                return;
            }

            instance.PlayerNpcConnections.Add(new() 
            {
                NpcId = npcId
            });
        }

        /// <returns>True if the player is already conencted to this NPC via Socailyte</returns>
        public static bool ConnectionExists(int npcId)
        {
            var existing = Instance.PlayerNpcConnections
                .FirstOrDefault(x => x.NpcId == npcId);

            return existing != null;
        }
    }
}