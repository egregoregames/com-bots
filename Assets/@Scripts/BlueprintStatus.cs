/// <summary>
/// There is a total of 78 Blueprints to see/collect. Each Blueprint is either 
/// (0) unseen, (1) seen but not owned, or (2) owned. When a form of a Bot is 
/// seen for the first time in a battle, it goes from unseen to seen. When a 
/// Blueprint is obtained (through dialog or an Item Crate), it goes from 
/// unseen/seen to owned.
/// <para />
/// Seen Blueprints display in the Botlink App with limited data. Owned 
/// Blueprints display in the Botlink App with full data. Unseen Blueprints 
/// do not display in the Botlink App.
/// <para />
/// Player's/teammates' Bots can only be formatted into a Blueprint that the 
/// player owns, and no two may use the same Blueprint.
/// </summary>
public enum BlueprintStatus
{
    Unseen,

    /// <summary>
    /// Player has seen this Blueprint, but does not own it
    /// </summary>
    Seen,

    Owned
}