using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public partial class PersistentGameData
{
    /// <summary>
    /// Contains API endpoints for methods related to adding and getting quests
    /// </summary>
    public static class Quests
    {
        /// <summary>
        /// Will update or add a quest to the player's tracked quests.
        /// <para />
        /// Set the currentStep to update the quest description in the Planner App. See
        /// StaticGameData and the individual Quest data scriptable objects for reference.
        /// <para />
        /// Setting currentStep to 100 will complete the quest, which will automatically
        /// update in the Planner App, and a new active quest will be automatically 
        /// chosen in the list, going through Requirements first, in order of quest 
        /// ID, then Electives.
        /// </summary>
        /// 
        /// <param name="questId">
        /// 
        /// </param>
        /// 
        /// <param name="currentStep">
        /// Should be between 0 and 100
        /// </param>
        public static async void Update(int questId, int currentStep)
        {
            using var block = InputBlocker.GetBlock("Updating quests");

            var quest = await GetOrAdd(questId);

            quest.CurrentStep = currentStep;

            if (quest.IsCompleted)
            {
                quest.Complete();
            }

            EnsureAtLeastOneActive();

            _onQuestUpdated?.Invoke(quest);
        }

        /// <summary>
        /// Sets this quest as the active quest in the Planner App. This should be called
        /// specifically if you need to make a quest active without the player's input.
        /// <para />
        /// This should NOT be used when the player is manually setting a quest active
        /// in the Planner App (there is already separate code for that).
        /// <para />
        /// This should NOT be used when adding the very first quest to the player's
        /// tracked quests (that will happen automatically).
        /// <para />
        /// This should NOT be used immediately after a quest has been completed and you
        /// want to set a new active quest (this happens automatically).
        /// </summary>
        /// <param name="questId"></param>
        public static async void ForceActive(int questId)
        {
            var quest = await GetOrAdd(questId);

            if (quest.IsCompleted) return;

            // Make all other quests inactive
            var instance = await GetInstanceAsync();

            foreach (var item in instance.PlayerQuestTrackingData)
            {
                item.IsActive = false;
            }

            quest.IsActive = true;
            _onQuestUpdated.Invoke(quest);
        }

        /// <summary>
        /// Returns all tracked quest data as a <see cref="IReadOnlyList{T}"/>.
        /// <para />
        /// Keep in mind that any modifications made directly to objects in the
        /// collection will NOT automatically fire any events. Only <see cref="PlannerPanel"/>
        /// should directly set any properties of the objects in this list.
        /// </summary>
        /// <returns></returns>
        public static IReadOnlyList<QuestTrackingDatum> GetAll()
        {
            return Instance.PlayerQuestTrackingData.AsReadOnly();
        }

        private static async Task<QuestTrackingDatum> GetOrAdd(int questId)
        {
            var instance = await GetInstanceAsync();

            var quest = instance.PlayerQuestTrackingData
                .FirstOrDefault(x => x.QuestId == questId);

            if (quest == null)
            {
                quest = new QuestTrackingDatum()
                {
                    QuestId = questId
                };

                instance.PlayerQuestTrackingData.Add(quest);
            }

            return quest;
        }

        private static async void EnsureAtLeastOneActive()
        {
            var instance = await GetInstanceAsync();

            var active = instance.PlayerQuestTrackingData
                .Where(x => !x.IsCompleted && x.IsActive)
                .FirstOrDefault();

            if (active == null)
            {
                var inactive = instance.PlayerQuestTrackingData
                    .OrderBy(x => x.QuestId)
                    .FirstOrDefault(x => !x.IsCompleted);

                if (inactive != null)
                {
                    inactive.IsActive = true;
                }
            }
        }
    }
}