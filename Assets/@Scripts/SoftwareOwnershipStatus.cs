/// <summary>
/// There will be somewhere around 200-250 pieces of Software to collect. Each 
/// piece of Software is either (0) unowned or (1) owned. When a piece of 
/// Software is obtained (through dialog or an Item Crate), it goes from 
/// unowned to owned.
/// <para />
/// Owned Software displays in the Botlink App. Unowned Software does not.
/// <para />
/// Player's/teammates' Bots' forms can only have Software that the player 
/// owns installed, but each piece of owned Software can be installed on any 
/// number of Bots or forms.
/// </summary>
public enum SoftwareOwnershipStatus
{
    NotOwned,
    Owned
}