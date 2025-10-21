using System.ComponentModel;

/// <summary>
/// The game’s story is divided into six terms
/// </summary>
public enum Term
{
    /// <summary>
    /// Early Fall to Late Fall. Ends when the player wins their first Promotion Battle
    /// </summary>
    [Description("Early Fall")]
    FirstTerm,

    /// <summary>
    /// Late Fall to Winter Vacation. Ends when the player wins their second Promotion Battle
    /// </summary>
    [Description("Late Fall")]
    SecondTerm,

    /// <summary>
    /// Winter Vacation to Early Spring. Ends when the player wins their third Promotion Battle
    /// </summary>
    [Description("Winter Vacation")]
    ThirdTerm,

    /// <summary>
    /// Early Spring to Late Spring. Ends when the player wins their fourth Promotion Battle
    /// </summary>
    [Description("Early Spring")]
    FourthTerm,

    /// <summary>
    /// Late Spring to Summer Vacation. Finishing the story
    /// </summary>
    [Description("Late Spring")]
    FifthTerm,

    /// <summary>
    /// Summer vacation (Post-game)
    /// </summary>
    [Description("Summer Vacation")]
    SixthTerm
}