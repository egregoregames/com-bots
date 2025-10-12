/// <summary>
/// Used to determine how to display the bot's status visually in the Botlink App UI
/// </summary>
public enum BotStatusDisplay
{
    /// <summary>
    /// Bot has 0 endurance
    /// </summary>
    Offline,

    /// <summary>
    /// Bot has at least 1 endurance and is not rusted or overloaded
    /// </summary>
    Normal,

    Rusted,

    Overloaded
}