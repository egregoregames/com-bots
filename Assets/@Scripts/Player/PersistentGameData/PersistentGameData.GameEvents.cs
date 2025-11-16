using System;

public partial class PersistentGameData
{
    public static class GameEvents
    {
        public static IDisposable OnTermUpdated(Action x)
            => _onTermUpdated.Subscribe(x);

        public static IDisposable OnSaveDataLoaded(Action x)
            => _onSaveDataLoaded.Subscribe(x);

        public static IDisposable OnRankXpUpdated(Action x)
            => _onRankXpUpdated.Subscribe(x);

        public static IDisposable OnCreditsUpdated(Action x)
            => _onCreditsUpdated.Subscribe(x);

        public static IDisposable OnMoneyUpdated(Action x)
            => _onMoneyUpdated.Subscribe(x);

        public static IDisposable OnLocationUpdated(Action x)
            => _onLocationUpdated.Subscribe(x);

        public static IDisposable OnQuestUpdated(Action<QuestTrackingDatum> x)
            => _onQuestUpdated.Subscribe(x);

        public static IDisposable OnInventoryItemUpdate(Action<InventoryItemDatum> x)
            => _onInventoryItemUpdated.Subscribe(x);

        /// <summary>
        /// Is invoked when a new connection is made in the Socialyte App. 
        /// Passes the integer ID of the NPC
        /// </summary>
        /// 
        /// <param name="x">
        /// An action with the integer ID of the added NPC as a parameter
        /// </param>
        /// 
        /// <returns></returns>
        public static IDisposable OnSocialyteProfileAdded(Action<int> x)
            => _onSocialyteProfileAdded.Subscribe(x);
    }
}