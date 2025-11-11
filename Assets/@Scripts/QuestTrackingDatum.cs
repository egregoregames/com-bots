using Sirenix.OdinInspector;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// In GameMaker, I tracked quest status as follows:
/// <para />
/// Each quest tracks a single integer value for its current “step.”
/// <para />
/// A step of 0 means the quest has not yet been received. (Meaning it does not 
/// show up in the Planner App.)
/// <para />
/// A step of 100 means the quest has been completed. (Meaning it shows up at 
/// the end of the list in the Planner App, grayed out with a “completed” tag on it.)
/// <para />
/// Steps of 1 – n represent the steps of the quest after each successive
/// quest update, i.e., 1 means the quest has been received but nothing has 
/// been done on it, 2 means the quest has been updated once, but is not yet 
/// completed, etc. 
/// <para />
/// In other words: Every quest starts at step 0 at the start of a new game.
/// When a quest is received, it goes up by 1, i.e., goes to step 1, thus 
/// becoming visible in the Planner App. When a quest is updated, it goes up 
/// by 1. When a quest is completed, it goes to step 100.
/// <para />
/// Some quests would contain as few steps as just a step 0 and a step 100 
/// (a quest that is completed at the same time that the player receives it). 
/// But quests could theoretically contain many steps (i.e., any number below 
/// 100); the max for any single quest will probably be around 15-20.
/// <para />
/// I would check the value like so. Using the “Pharmacology” quest as an 
/// example:
/// <para />
/// if pharmacology.step = 100 { do something}
/// <para />
/// This code would activate if the “Pharmacology” quest had been completed.
/// <para />
/// For each step besides step 0, every quest has a corresponding message that 
/// will display when viewed in the Planner App.
/// </summary>
[Serializable]
public class QuestTrackingDatum
{
    [field: SerializeField, ReadOnly]
    public int QuestId { get; set; }

    [field: SerializeField, ReadOnly]
    public bool IsActive { get; set; }

    [field: SerializeField, ReadOnly]
    public int CurrentStep { get; set; } = 0;

    public bool IsCompleted => CurrentStep >= 100;

    [field: SerializeField, ReadOnly]
    public bool HasUnreadUpdates { get; set; } = true;

    public async Task<StaticQuestData> GetQuestDataAsync()
    {
        return (await StaticGameData.GetInstanceAsync()).QuestData
            .First(x => x.QuestID == QuestId);
    }

    /// <summary>
    /// Make sure <see cref="StaticGameData.Instance"/> is not null or 
    /// use <see cref="GetQuestDataAsync"/>
    /// </summary>
    /// <returns></returns>
    public StaticQuestData GetQuestData()
    {
        return StaticGameData.Instance.QuestData
            .First(x => x.QuestID == QuestId);
    }

    public void Complete()
    {
        CurrentStep = 100;
        IsActive = false;
    }
}