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

        public static IDisposable OnSocialyteProfileUpdated(Action<NpcConnectionDatum> x)
            => _onSocialyteProfileUpdated.Subscribe(x);

        public static IDisposable OnTeammateBondUpdated(Action<TeammateBondDatum> x)
            => _onTeammateBondUpdated.Subscribe(x);

        public static IDisposable OnTeamMembersChanged(Action x)
            => _onTeamMembersUpdated.Subscribe(x);

        public static IDisposable OnMedalAdded(Action<int> x)
            => _onMedalAdded.Subscribe(x);
    }
}