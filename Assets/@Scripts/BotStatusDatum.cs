using System.Collections.Generic;

public class BotStatusDatum
{
    /// <summary>
    /// Value for the Energy (i.e., "mana", "stamina", etc) 
    /// of the Bot.
    /// <para />
    /// If a Bot's Max Energy changes (rank up; effect of newly active Ability; 
    /// effect of newly equipped Hardware; Blueprint change), its current 
    /// Energy becomes whatever integer value (must be at least 1) yields the 
    /// same ratio of Current Energy/Max Energy that existed before the Max 
    /// Energy changed.
    /// </summary>
    public int Energy { get; set; }

    /// <summary>
    /// Value for the Endurance (i.e., hitpoints) of the Bot.
    /// <para />
    /// If a Bot's Max Endurance changes (rank up; effect of newly active 
    /// Ability; effect of newly equipped Hardware; Blueprint change), its 
    /// current Endurance becomes whatever integer value (must be at least 1) 
    /// yields the same ratio of Current Endurance/Max Endurance that existed 
    /// before the Max Endurance changed.
    /// </summary>
    public int Endurance { get; set; }

    public bool IsOverloaded { get; set; }

    public bool IsRusted { get; set; }

    public List<BotStatusDisplay> GetBotStatusDisplay()
    {
        if (Endurance <= 0)
        {
            return new() { BotStatusDisplay.Offline };
        }
        else if (IsRusted && IsOverloaded)
        {
            return new() { BotStatusDisplay.Rusted, BotStatusDisplay.Overloaded };
        }
        else if (IsRusted)
        {
            return new() { BotStatusDisplay.Rusted };
        }
        else if (IsOverloaded)
        {
            return new() { BotStatusDisplay.Overloaded };
        }
        else
        {
            return new() { BotStatusDisplay.Normal };
        }
    }
}