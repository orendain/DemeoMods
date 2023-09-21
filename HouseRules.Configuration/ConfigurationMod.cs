namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using BepInEx;
    using BepInEx.Configuration;
    using BepInEx.Logging;
    using Common.UI;
    using HouseRules.Configuration.UI;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [BepInPlugin("com.orendain.demeomods.houserules.configuration", "HouseRules.Configuration", "2.0.0")]
    [BepInDependency("com.orendain.demeomods.houserules.core", "2.0.0")]
    public class ConfigurationMod : BaseUnityPlugin
    {
        private const int PC1LobbySceneIndex = 1;
        private const int PC2LobbySceneIndex = 3;

        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        internal static bool IsUpdateAvailable { get; private set; }

        internal static ManualLogSource Log { get; private set; }

        private ConfigEntry<string> DefaultRulesetEntry { get; set; }

        private ConfigEntry<bool> LoadRulesetsFromConfigEntry { get; set; }

        private void Awake()
        {
            Log = Logger;
            DetermineIfUpdateAvailable();

            SceneManager.sceneUnloaded += OnSceneUnloaded;
            SceneManager.sceneLoaded += OnSceneLoaded;

            ExampleRulesetExporter.ExportExampleRulesetsIfNeeded();
        }

        private void Start()
        {
            LoadConfig();
            if (LoadRulesetsFromConfigEntry.Value)
            {
                LoadRulesetsFromConfig();
            }

            var rulesetName = DefaultRulesetEntry.Value;
            if (string.IsNullOrEmpty(rulesetName))
            {
                return;
            }

            try
            {
                HR.SelectRuleset(rulesetName);
            }
            catch (ArgumentException e)
            {
                Log.LogWarning($"Failed to select default ruleset [{rulesetName}] specified in config: {e}");
            }
        }

        private void LoadConfig()
        {
            DefaultRulesetEntry = Config.Bind(
                "General",
                "DefaultRuleset",
                string.Empty,
                "Default ruleset to have selected.");
            LoadRulesetsFromConfigEntry = Config.Bind(
                "General",
                "LoadRulesetsFromConfig",
                true,
                "Whether or not to load rulesets from config files.");
        }

        public void OnSceneUnloaded(Scene scene)
        {
            // Logger.Msg($"Scene unloaded {buildIndex} - {sceneName}");
            GameObject canvasObject = GameObject.Find("~LeanTween");
            if (canvasObject == null)
            {
                return;
            }

            // Prevent Game VR object from trying to appear at main menu and hangouts
            Transform transformScreenToRemove = canvasObject.transform.Find("HouseRulesUiGameVr");
            if (transformScreenToRemove == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(transformScreenToRemove.gameObject);
        }

        public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var buildIndex = scene.buildIndex;
            var sceneName = scene.name;

            // Logger.Msg($"buildIndex {buildIndex} - sceneName {sceneName}");
            if (buildIndex == 0 || sceneName.Contains("Startup"))
            {
                return;
            }

            if (Environments.IsPcEdition())
            {
                if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
                {
                    return;
                }

                Log.LogDebug("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
            }
            else if (Environments.IsInHangouts() || sceneName.Contains("HobbyShop"))
            {
                Log.LogDebug("Recognized lobby in Hangouts. Loading UI.");
                _ = new GameObject("HouseRulesUiHangouts", typeof(HouseRulesUiHangouts));
            }
            else if (sceneName.Contains("Lobby"))
            {
                Log.LogDebug("Recognized lobby in VR. Loading UI.");
                _ = new GameObject("HouseRulesUiVr", typeof(HouseRulesUiVr));
            }
            else
            {
                if (HR.SelectedRuleset != Ruleset.None)
                {
                    Logger.LogDebug("Recognized modded game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr", typeof(HouseRulesUiGameVr));
                }
            }
        }

        private static async void DetermineIfUpdateAvailable()
        {
            IsUpdateAvailable = await VersionChecker.IsUpdateAvailable();
            Log.LogDebug($"{(IsUpdateAvailable ? "New" : "No new")} HouseRules update found.");
        }

        private static void LoadRulesetsFromConfig()
        {
            var rulesetFiles = ConfigManager.RulesetFiles;
            Log.LogInfo($"Found [{rulesetFiles.Count}] ruleset files in configuration.");

            foreach (var file in rulesetFiles)
            {
                try
                {
                    var ruleset = ConfigManager.ImportRuleset(file, tolerateFailures: false);
                    HR.Rulebook.Register(ruleset);
                }
                catch (Exception e)
                {
                    FailedRulesetFiles.Add(file);
                    Log.LogWarning(
                        $"Failed to import and register ruleset from file [{file}]. Skipping that ruleset: {e}");
                }
            }
        }
    }
}
