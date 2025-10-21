using System;
using System.ComponentModel;
using System.Reflection;

public static class EnumExtensions
{
    /// <summary>
    /// Cache this value if accessing frequently
    /// </summary>
    /// <param name="value"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());

        if (field != null)
        {
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(
                field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute == null ? value.ToString() : attribute.Description;
        }

        return value.ToString();
    }
}