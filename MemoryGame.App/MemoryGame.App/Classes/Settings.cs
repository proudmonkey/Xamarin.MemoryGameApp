// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using System;

namespace MemoryGame.App.Classes
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
        private static ISettings AppSettings => CrossSettings.Current;

        public static string PlayerFirstName
        {
            get { return AppSettings.GetValueOrDefault(nameof(PlayerFirstName), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(PlayerFirstName), value); }
        }

        public static string PlayerLastName
        {
            get { return AppSettings.GetValueOrDefault(nameof(PlayerLastName), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(PlayerLastName), value); }
        }

        public static string PlayerEmail
        {
            get { return AppSettings.GetValueOrDefault(nameof(PlayerEmail), string.Empty); }
            set { AppSettings.AddOrUpdateValue(nameof(PlayerEmail), value); }
        }

        public static int TopScore
        {
            get { return AppSettings.GetValueOrDefault(nameof(TopScore), 1); }
            set { AppSettings.AddOrUpdateValue(nameof(TopScore), value); }
        }

        public static DateTime DateAchieved
        {
            get { return AppSettings.GetValueOrDefault(nameof(DateAchieved), DateTime.UtcNow); }
            set { AppSettings.AddOrUpdateValue(nameof(DateAchieved), value); }
        }

        public static bool IsProfileSync
        {
            get { return AppSettings.GetValueOrDefault(nameof(IsProfileSync), false); }
            set { AppSettings.AddOrUpdateValue(nameof(IsProfileSync), value); }
        }

        public static int PlayerID
        {
            get { return AppSettings.GetValueOrDefault(nameof(PlayerID), 0); }
            set { AppSettings.AddOrUpdateValue(nameof(PlayerID), value); }
        }

    }
}