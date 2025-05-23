using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using WDPS.Core.Models;

namespace WDPS.Core.Services
{
    public class FeatureFlagService
    {
        public FeatureFlagConfig Config { get; private set; }

        public FeatureFlagService(string configPath)
        {
            LoadConfig(configPath);
        }

        public void LoadConfig(string path)
        {
            if (File.Exists(path))
            {
                var json = File.ReadAllText(path);
                Config = JsonSerializer.Deserialize<FeatureFlagConfig>(json) ?? new FeatureFlagConfig();
            }
            else
            {
                Config = new FeatureFlagConfig();
            }
        }

        public bool IsFeatureEnabled(string feature)
        {
            return Config.FeatureFlags.TryGetValue(feature, out var enabled) && enabled;
        }

        public string GetABTestGroup(string testName)
        {
            return Config.ABTestGroups.TryGetValue(testName, out var group) ? group : "A";
        }
    }
} 