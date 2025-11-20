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

            if (isVisible)
            {
                connection.HasNewUpdates = true;
            }

            _onSocialyteProfileUpdated?.Invoke(connection);
        }

        /// <summary>
        /// Will change the displayed bio in the Socialyte profile for the 
        /// given NPC. See <see cref="SocialyteProfileStaticDatum.BioSteps"/>
        /// </summary>
        /// <param name="npcId"></param>
        /// <param name="step"></param>
        public static async void SetBioStep(int npcId, int step)
        {
            await GetInstanceAsync();
            var connection = GetConnectionDatum(npcId);

            if (connection.CurrentBioStep == step) 
                return;

            connection.HasNewUpdates = true;
            _onSocialyteProfileUpdated?.Invoke(connection);
        }

        /// <summary>
        /// Will change the displayed check-in location in the Socialyte 
        /// profile for the NPC. See 
        /// <see cref="SocialyteProfileStaticDatum.CheckInLocationSteps"/>
        /// </summary>
        /// <param name="npcId"></param>
        /// <param name="step"></param>
        public static async void SetCheckInLocationStep(int npcId, int step)
        {
            await GetInstanceAsync();
            var connection = GetConnectionDatum(npcId);

            if (connection.CurrentCheckInLocationStep == step) 
                return;

            connection.HasNewUpdates = true;
            _onSocialyteProfileUpdated?.Invoke(connection);
        }

        /// <summary>
        /// Warning: Modifying the returned datum will not fire events. 
        /// Use the other API methods.
        /// </summary>
        /// <param name="npcId"></param>
        /// <returns></returns>
        public static NpcConnectionDatum GetConnectionDatum(int npcId)
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