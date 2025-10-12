/// <summary>
/// Add to fields or properties to mark them for saving/loading with ComBotsSaveSystem
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
public class ComBotsSaveAttribute : System.Attribute
{
    public string Key { get; private set; }
    public object DefaultValue { get; set; }

    public ComBotsSaveAttribute(string saveKey, object defaultValue)
    {
        Key = saveKey;
        DefaultValue = defaultValue;
    }
}