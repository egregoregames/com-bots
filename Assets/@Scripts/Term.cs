/// <summary>
/// The game’s story is divided into six terms
/// </summary>
public enum Term
{
    /// <summary>
    /// Early Fall to Late Fall. Ends when the player wins their first Promotion Battle
    /// </summary>
    FirstTerm,

    /// <summary>
    /// Late Fall to Winter Vacation. Ends when the player wins their second Promotion Battle
    /// </summary>
    SecondTerm,

    /// <summary>
    /// Winter Vacation to Early Spring. Ends when the player wins their third Promotion Battle
    /// </summary>
    ThirdTerm,

    /// <summary>
    /// Early Spring to Late Spring. Ends when the player wins their fourth Promotion Battle
    /// </summary>
    FourthTerm,

    /// <summary>
    /// Late Spring to Summer Vacation. Finishing the story
    /// </summary>
    FifthTerm,

    /// <summary>
    /// Summer vacation (Post-game)
    /// </summary>
    SixthTerm
}