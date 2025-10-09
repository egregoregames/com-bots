using System;
using System.Collections.Generic;

[Serializable]
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

    /* HARDWARE NOTES. TODO: Move this to a design doc and hardware class xml
    //    Each form(up to 5 total), of each of the player’s Bot and all the potential teammates' Bots, can have between 0 and 8 (6 for Basic forms) pieces of Hardware equipped at a time.

    //This is further broken down to:

    //-Up to 3 pieces of Internal Hardware

    //-Up to 2 pieces of Arm Hardware

    //-Up to 1 piece of Headgear Hardware

    //-Up to 1 piece of Armor Hardware(for Combat and Awakened forms)

    //-Up to 1 piece of Boot Hardware(for Combat and Awakened forms)

    //A Bot form’s Hardware loadout can be changed by speaking to a unique NPC in the Engineering Bldg(for a teammate’s Bot, only if the teammate’s Bond is sufficient).

    //Each Bot form can only have Hardware installed where the total of all the installed Hardware’s weight is less than or equal to the Bot form’s Load.

    //If the Blueprint of a team member’s Bot is changed, and the Load of a Bot form of the new Blueprint is less than the weight of the form’s Hardware loadout, Hardware is automatically uninstalled in reverse chronological order(from Boot, to Armor, to Headgear, to Arm, to Internal) until the form’s Load is greater than or equal to the Hardware weight.

    //A Bot form’s equipped Hardware is displayed in the Botlink App.

    //Hardware mainly has passive, in-battle effects.*/

    /// <summary>
    /// Max 3
    /// </summary>
    public List<string> InternalHardwareIds { get; set; } = new();

    /// <summary>
    /// Max 2
    /// </summary>
    public List<string> ArmHardwareIds { get; set; } = new();

    public string HeadgearHardwareId { get; set; } = null;

    /// <summary>
    /// Requires combat or awakened form
    /// </summary>
    public string ArmorHardwareId { get; set; } = null;

    /// <summary>
    /// Requires combat or awakened form
    /// </summary>
    public string BootHardwareId { get; set; } = null;

    /// <summary>
    /// Each form (up to 5 total), of each of the player’s Bot and all the 
    /// potential teammates' Bots, can have between 3 (2 for teammates) and 
    /// 8 pieces of Software installed at a time. 
    /// <para />
    /// The “Attack,” “Focus,” and “Flee” Software are always installed on the
    /// player’s Bot.The “Attack” and “Focus” Software are always installed on 
    /// each teammate’s Bot.
    /// <para />
    /// A Bot form’s Software loadout can be changed in the Botlink App (for a 
    /// teammate’s Bot, only if the teammate’s Bond is sufficient).
    /// <para />
    /// Each Blueprint and its upgradeable forms is compatible with only certain 
    /// pieces of Software.If the Blueprint of a team member’s Bot is changed, 
    /// any incompatible Software is automatically uninstalled.
    /// <para />
    /// Each Bot form can only have Software installed where the total of all 
    /// the installed Software’s RAM costs is less than or equal to the Bot 
    /// form’s RAM. 
    /// <para />
    /// If the Blueprint of a team member’s Bot is changed, and the RAM of a 
    /// Bot form of the new Blueprint is less than the RAM cost of the form’s 
    /// Software loadout, Software is automatically uninstalled in reverse 
    /// chronological order until the form’s RAM is greater than or equal to 
    /// the RAM cost.
    /// </summary>
    public List<string> InstalledSoftwareIds { get; set; } = new();

    /// <summary>
    /// For each member of the player’s party (always includes the player, and 
    /// may include 1 or 2 teammates), tracks the Ability that is currently 
    /// assigned to the member’s Bot (either the Blueprint’s first, second, 
    /// or third Ability).
    /// <para />
    /// Abilities mainly have passive, in-battle effects.
    /// </summary>
    public string ActiveAbilityId { get; set; } = null;

    /// <summary>
    /// For each member of the player’s party (always includes the player, 
    /// and may include 1 or 2 teammates), tracks the Blueprint that the 
    /// member’s Bot is currently formatted into. 
    /// <para />
    /// Blueprint determines the Bot’s name, appearance, next forms, 
    /// base stats, potential abilities, and compatible (potentially 
    /// installable) Software.
    /// </summary>
    public string BlueprintId { get; set; } = null;

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