namespace Dart.Helpers
{
    public static class ColorHelper
    {
        // Cache the theme to avoid repeated lookups
        private static bool IsDarkTheme => Application.Current?.RequestedTheme == AppTheme.Dark;

        // Train type border colors
        private static readonly Color DartTrainBorderColor = Color.FromArgb("#14A085"); // Green for DART trains
        private static readonly Color OtherTrainBorderColor = Color.FromArgb("#FF8C00"); // Orange for other trains

        // Card border colors
        private static readonly Color CardBorderColorDark = Color.FromArgb("#3a3a3a");
        private static readonly Color CardBorderColorLight = Color.FromArgb("#e0e0e0");

        // Card background colors
        private static readonly Color CardBackgroundColorDark = Color.FromArgb("#2b2b2b");
        private static readonly Color CardBackgroundColorLight = Color.FromArgb("#ffffff");

        // Due time label colors
        private static readonly Color DueTimeUrgentColor = Color.FromArgb("#ff6b6b"); // Red for urgent trains
        private static readonly Color DueTimeNormalDarkColor = Color.FromArgb("#4ecdc4"); // Cyan for dark mode
        private static readonly Color DueTimeNormalLightColor = Color.FromArgb("#2a9d8f"); // Teal for light mode

        // Text colors
        private static readonly Color SecondaryTextDarkColor = Color.FromArgb("#b0b0b0");
        private static readonly Color SecondaryTextLightColor = Color.FromArgb("#666666");
        private static readonly Color TertiaryTextDarkColor = Color.FromArgb("#888888");
        private static readonly Color TertiaryTextLightColor = Color.FromArgb("#999999");

        // Button colors
        private static readonly Color SelectedButtonColor = Color.FromArgb("#512BD4");
        private static readonly Color UnselectedButtonColor = Colors.Transparent;

        // Train type border colors
        public static Color GetTrainBorderColor(string trainType)
        {
            if (!string.IsNullOrWhiteSpace(trainType) &&
                trainType.Equals("DART", StringComparison.OrdinalIgnoreCase))
            {
                return DartTrainBorderColor;
            }
            return OtherTrainBorderColor;
        }

        // Card border colors (for general cards, not train-specific)
        public static Color GetCardBorderColor()
        {
            return IsDarkTheme
                ? CardBorderColorDark
                : CardBorderColorLight;
        }

        // Card background colors
        public static Color GetCardBackgroundColor()
        {
            return IsDarkTheme
                ? CardBackgroundColorDark
                : CardBackgroundColorLight;
        }

        // Due time label colors
        public static Color GetDueTimeLabelColor(int dueInMinutes)
        {
            if (dueInMinutes <= 5)
            {
                return DueTimeUrgentColor;
            }

            return IsDarkTheme
                ? DueTimeNormalDarkColor
                : DueTimeNormalLightColor;
        }

        // Secondary text colors
        public static Color GetSecondaryTextColor()
        {
            return IsDarkTheme
                ? SecondaryTextDarkColor
                : SecondaryTextLightColor;
        }

        // Tertiary text colors (for last location info)
        public static Color GetTertiaryTextColor()
        {
            return IsDarkTheme
                ? TertiaryTextDarkColor
                : TertiaryTextLightColor;
        }

        // Direction button colors
        public static Color GetSelectedButtonColor()
        {
            return SelectedButtonColor;
        }

        public static Color GetUnselectedButtonColor()
        {
            return UnselectedButtonColor;
        }
    }
}
