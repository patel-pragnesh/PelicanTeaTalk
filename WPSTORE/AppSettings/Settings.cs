using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System.Collections.Generic;
using WPSTORE.Model;

namespace WPSTORE.AppSettings
{
    public static class Settings
    {
        private static ISettings AppSettings => CrossSettings.Current;

        public static uint CoverAnimationDuration = 10000;

        public static List<LanguageSelectItem> Languages = new List<LanguageSelectItem>
        {
            new LanguageSelectItem { Code ="en", Name ="English"},
            new LanguageSelectItem { Code ="es", Name ="Spanish"},
            new LanguageSelectItem { Code ="fr", Name ="French"},
            new LanguageSelectItem { Code ="zh", Name ="Chinese"},
            new LanguageSelectItem { Code ="de", Name ="German"}
        };
        public static string DefaultLanguage = "en";

        public static string LanguageSelected
        {
            get => AppSettings.GetValueOrDefault(nameof(LanguageSelected), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(LanguageSelected), value);
        }

        public static List<ThemeSelectItem> Themes = new List<ThemeSelectItem>
        {
            new ThemeSelectItem { ColorCode ="#f54e5e", Name ="Green"},
            new ThemeSelectItem { ColorCode ="#2f72e4", Name ="Red"},
            new ThemeSelectItem { ColorCode ="#5d4cf7", Name ="Purple"},
            new ThemeSelectItem { ColorCode ="#06846a", Name ="Yellow"},
            new ThemeSelectItem { ColorCode ="#d54008", Name ="Orange"}
        };
        public static string DefaultTheme = "Yellow";

        public static string ThemeSelected
        {
            get => AppSettings.GetValueOrDefault(nameof(ThemeSelected), string.Empty);
            set => AppSettings.AddOrUpdateValue(nameof(ThemeSelected), value);
        }

        public static bool IsNightModeEnabled
        {
            get => AppSettings.GetValueOrDefault(nameof(IsNightModeEnabled), false);
            set => AppSettings.AddOrUpdateValue(nameof(IsNightModeEnabled), value);
        }
    }
}
