namespace ComBots.Utils.Texts
{
    /// <summary>
    /// Utility class for applying Unity's Rich Text formatting.
    /// </summary>
    public static class RichTextFormatter
    {
        /// <summary>
        /// Formats text to be bold and colored using Unity's rich text tags.
        /// </summary>
        /// <param name="text">The text to format.</param>
        /// <param name="colorName">The name of the color (e.g., "red", "lightblue") or hex code (e.g., "#FF0000"). Defaults to white if null or empty.</param>
        /// <returns>Formatted string.</returns>
        public static string BoldColor(string text, string colorName)
        {
            if (string.IsNullOrEmpty(colorName))
            {
                colorName = "white"; // Default color
            }
            return $"<b><color={colorName}>{text}</color></b>";
        }

        /// <summary>
        /// Formats text to be bold and colored using Unity's rich text tags.
        /// </summary>
        /// <param name="text">The text to format.</param>
        /// <param name="colorName">The name of the color (e.g., "red", "lightblue") or hex code (e.g., "#FF0000"). Defaults to white if null or empty.</param>
        /// <returns>Formatted string.</returns>
        public static string Bold(string text)
        {
            return $"<b>{text}</b>";
        }

        /// <summary>
        /// Formats text to be colored using Unity's rich text tags.
        /// </summary>
        /// <param name="text">The text to format.</param>
        /// <param name="colorName">The name of the color (e.g., "red", "lightblue") or hex code (e.g., "#FF0000"). Defaults to white if null or empty.</param>
        /// <returns>Formatted string.</returns>
        public static string Color(string text, string colorName)
        {
             if (string.IsNullOrEmpty(colorName))
            {
                colorName = "white"; // Default color
            }
            return $"<color={colorName}>{text}</color>";
        }
    }
}
