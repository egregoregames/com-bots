/// <summary>
/// For each of the 12 potential teammates, ranges from 0 to 3. 
/// <para />
/// This value displays on the teammate's Socialyte profile and on the 
/// Teammate Select screen in the Botlink app (if the teammate is in the 
/// player's party).
/// <para />
/// This value determines how much the player can customize or control a given 
/// teammate's Bot.
/// </summary>
public enum TeammateBond
{
    /// <summary>
    /// The player cannot add the teammate to their party.
    /// </summary>
    None,

    /// <summary>
    /// The player can add the teammate to their party, but they cannot 
    /// customize or control the teammate's Bot. The teammate's actions in 
    /// battle are determined by an independent AI, not by the player’s choice.
    /// </summary>
    Level1,

    /// <summary>
    /// The player can customize the Software equipped to the teammate's Bot 
    /// as well as its Active Ability. The player also decides what the teammate 
    /// does in battle.
    /// </summary>
    Level2,

    /// <summary>
    /// In addition to the benefits of Bond 2, the player can also customize 
    /// the Hardware equipped to the teammate's Bot as well as the Blueprint 
    /// that the teammate's Bot uses.
    /// </summary>
    Level3
}