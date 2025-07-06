using Newtonsoft.Json;
using System;
using System.IO;
using System.Xml;

namespace CurrencyConvertor
{
    public class SettingsManager
    {
        private readonly string _settingsPath;

        public SettingsManager()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var appFolder = Path.Combine(appDataPath, "CurrencyConvertor");
            Directory.CreateDirectory(appFolder);
            _settingsPath = Path.Combine(appFolder, "settings.json");
        }

        public AppSettings LoadSettings()
        {
            try
            {
                if (File.Exists(_settingsPath))
                {
                    var json = File.ReadAllText(_settingsPath);
                    return JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                }
            }
            catch (Exception)
            {
                // If loading fails, return default settings
            }

            return new AppSettings();
        }

        public void SaveSettings(AppSettings settings)
        {
            try
            {
                var json = JsonConvert.SerializeObject(settings, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented);
                File.WriteAllText(_settingsPath, json);
            }
            catch (Exception)
            {
                // Silently fail if saving settings fails
            }
        }
    }

    public class AppSettings
    {
        public decimal LastAmount { get; set; } = 100;
        public string LastFromCurrency { get; set; } = "USD";
        public string LastToCurrency { get; set; } = "EUR";
        public bool IsDarkTheme { get; set; } = true;
    }
}