public enum ItemType
{
    /// <summary>
    /// Key Items are “quest items.” Their unique features are that they 
    /// cannot be bought or sold, and (with only two exceptions) you cannot 
    /// have more than one copy of each. Thus, besides those two exceptions, 
    /// they do not display a quantity number in the Backpack menu.
    /// <para />
    /// The two Key Items with quantities are Fixit Tickets and Meteor Shards.
    /// </summary>
    KeyItem,

    /// <summary>
    /// Have quantities and can be bought and sold, can be used in battle. 
    /// Some can be used in the overworld
    /// </summary>
    BattleItem,

    /// <summary>
    /// Have quantities and can be bought and (mostly) sold, cannot be used in battle. 
    /// Some can be used in the overworld
    /// <para />
    /// The 3 General Items that cannot be sold are Litter, Broken Part, 
    /// and Corrupt Disc (these are the 3 items that can be recycled)
    /// </summary>
    GeneralItem,

    /// <summary>
    /// Hardware is equipment. They can be attached (“bonded”) to Bots. 
    /// Hardware also displays two unique attributes in the menu: weight 
    /// and attachment point. Cannot be sold, but can be bought sometimes
    /// <para />
    /// Hardware attachment points include Internal, Headgear, Arm, Armor, 
    /// and Boot
    /// </summary>
    Hardware,

    /// <summary>
    /// Parts are like General Items (quantity, bought and sold, etc) but 
    /// they can also be used to craft Items/Hardware with a certain NPC 
    /// (known as “fabrication”)
    /// </summary>
    Part
}