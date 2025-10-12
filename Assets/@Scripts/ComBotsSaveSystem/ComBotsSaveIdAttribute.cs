/// <summary>
/// Add to a single <see cref="string"/> (probably serialized for use in the 
/// Unity Editor) field or property in a class to assign a unique identifier for 
/// saving/loading with ComBotsSaveSystem. Useful when multiple instances 
/// (i.e. NPCs) share the same classes.
/// </summary>
[System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Property)]
public class ComBotsSaveIdAttribute : System.Attribute
{
    public ComBotsSaveIdAttribute()
    {
    }
}