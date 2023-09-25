namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using Common.UI;
    using HouseRules.Configuration.UI;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using UnityEngine;

    internal static class HouseRulesConfigurationCore
    {
        internal const string ModId = "com.orendain.demeomods.houserules.configuration";
        internal const string ModName = "HouseRules.Configuration";
        internal const string ModVersion = "1.8.0";
        internal const string ModAuthor = "DemeoMods Team";

        internal static Action<object> LogMessage;
        internal static Action<object> LogInfo;
        internal static Action<object> LogDebug;
        internal static Action<object> LogWarning;
        internal static Action<object> LogError;

        private const int PC1LobbySceneIndex = 1;
        private const int PC2LobbySceneIndex = 3;

        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        internal static bool IsUpdateAvailable { get; private set; }

        internal static void Init(object loader)
        {
#if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                LogMessage = plugin.Log.LogMessage;
                LogInfo = plugin.Log.LogInfo;
                LogDebug = plugin.Log.LogDebug;
                LogWarning = plugin.Log.LogWarning;
                LogError = plugin.Log.LogError;
            }
#endif

#if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                LogMessage = mod.LoggerInstance.Msg;
                LogInfo = mod.LoggerInstance.Msg;
                LogDebug = mod.LoggerInstance.Msg;
                LogWarning = mod.LoggerInstance.Warning;
                LogError = mod.LoggerInstance.Error;
            }
#endif

            DetermineIfUpdateAvailable();
        }

        private static async void DetermineIfUpdateAvailable()
        {
            IsUpdateAvailable = await VersionChecker.IsUpdateAvailable();
            LogInfo($"{(IsUpdateAvailable ? "New" : "No new")} HouseRules update found.");
        }

        public static void LoadConfiguration()
        {
            ExampleRulesetExporter.ExportExampleRulesetsIfNeeded();

            var loadRulesetsFromConfig = ConfigManager.GetLoadRulesetsFromConfig();
            if (loadRulesetsFromConfig)
            {
                LoadRulesetsFromConfig();
            }

            var rulesetName = ConfigManager.GetDefaultRuleset();
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
                LogWarning($"Failed to select default ruleset [{rulesetName}] specified in config: {e}");
            }
        }

        internal static void OnSceneUnloaded(int buildIndex, string sceneName)
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

        internal static void OnSceneLoaded(int buildIndex, string sceneName)
        {
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

                LogDebug("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
            }
            else if (Environments.IsInHangouts() || sceneName.Contains("HobbyShop"))
            {
                LogDebug("Recognized lobby in Hangouts. Loading UI.");
                _ = new GameObject("HouseRulesUiHangouts", typeof(HouseRulesUiHangouts));
            }
            else if (sceneName.Contains("Lobby"))
            {
                LogDebug("Recognized lobby in VR. Loading UI.");
                _ = new GameObject("HouseRulesUiVr", typeof(HouseRulesUiVr));
            }
            else
            {
                if (HR.SelectedRuleset != Ruleset.None)
                {
                    LogDebug("Recognized modded game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr", typeof(HouseRulesUiGameVr));
                }
            }
        }

        private static void LoadRulesetsFromConfig()
        {
            var rulesetFiles = ConfigManager.RulesetFiles;
            LogInfo($"Found [{rulesetFiles.Count}] ruleset files in configuration.");

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
                    LogWarning(
                        $"Failed to import and register ruleset from file [{file}]. Skipping that ruleset: {e}");
                }
            }
        }
    }
}
