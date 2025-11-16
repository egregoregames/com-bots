public partial class PersistentGameData
{
    public static class Socialyte
    {
        /// <summary>
        /// Add a new NPC connection in the Socialyte app. Will log a warning if
        /// the NPC already exists as a connection. Check if the connection already exists
        /// by calling <see cref="PlayerNpcConnections.Contains(npcId)"/>
        /// </summary>
        /// <param name="npcId">The Profile ID of the NPC. 
        /// Lives at <see cref="SocialyteProfileStaticDatum.ProfileId"/></param>
        public static async void AddConnection(int npcId)
        {
            var instance = await GetInstanceAsync();

            if (instance.PlayerNpcConnections.Contains(npcId))
            {
                instance.Log($"NPC id {npcId} has already been added as a socialyte connection");
                return;
            }

            instance.PlayerNpcConnections.Add(npcId);
        }
    }
}