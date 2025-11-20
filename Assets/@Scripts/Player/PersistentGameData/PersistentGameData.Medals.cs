using System.Threading.Tasks;

public partial class PersistentGameData
{
    public static class Medals
    {
        public static async void AddMedal(int medalId)
        {
            var instance = await GetInstanceAsync();
            if (instance.PlayerMedals.Contains(medalId))
            {
                return;
            }

            instance.PlayerMedals.Add(medalId);
        }

        public static async Task<bool> HasMedal(int medalId)
        {
            var instance = await GetInstanceAsync();
            return instance.PlayerMedals.Contains(medalId);
        }

        /// <returns>The total number of medals the player has</returns>
        public static async Task<int> GetCountAsync()
        {
            var instance = await GetInstanceAsync();
            return instance.PlayerMedals.Count;
        }
    }
}