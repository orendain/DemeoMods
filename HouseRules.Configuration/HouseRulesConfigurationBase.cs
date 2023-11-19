namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Common.UI;
    using HouseRules.Configuration.UI;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using UnityEngine;

    internal static class HouseRulesConfigurationBase
    {
        internal const string ModId = "com.orendain.demeomods.houserules.configuration";
        internal const string ModName = "HouseRules.Configuration";
        internal const string ModAuthor = "DemeoMods Team";

        private const int NonVrSteamLobbySceneIndex = 1;
        private const int NonVrCombinedSteamLobbySceneIndex = 3;

        private static Action<object>? _logInfo;
        private static Action<object>? _logDebug;
        private static Action<object>? _logWarning;
        private static Action<object>? _logError;

        internal static void LogInfo(object data) => _logInfo?.Invoke(data);

        internal static void LogDebug(object data) => _logDebug?.Invoke(data);

        internal static void LogWarning(object data) => _logWarning?.Invoke(data);

        internal static void LogError(object data) => _logError?.Invoke(data);

        private static readonly List<string> FailedRulesetFiles = new();

        internal static bool IsUpdateAvailable { get; private set; }

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Log == null)
                {
                    LogError("Logger instance is invalid. Cannot initialize.");
                    return;
                }

                _logInfo = plugin.Log.LogInfo;
                _logDebug = plugin.Log.LogDebug;
                _logWarning = plugin.Log.LogWarning;
                _logError = plugin.Log.LogError;
            }
            #endif

            #if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                _logInfo = mod.LoggerInstance.Msg;
                _logDebug = mod.LoggerInstance.Msg;
                _logWarning = mod.LoggerInstance.Warning;
                _logError = mod.LoggerInstance.Error;
            }
            #endif

            DetermineIfUpdateAvailable();
        }

        private static async void DetermineIfUpdateAvailable()
        {
            IsUpdateAvailable = await RevolutionsChecker.IsUpdateAvailable();
            LogInfo($"{(IsUpdateAvailable ? "New" : "No new")} Revolutions update found.");
        }

        internal static void OnSceneUnloaded(int buildIndex, string sceneName)
        {
            // LogDebug("OnSceneUnloaded");
            GameObject canvasObject = GameObject.Find("~LeanTween");
            if (canvasObject == null)
            {
                return;
            }

            // LogDebug($"Scene unloaded {buildIndex} - {sceneName}");
            // Prevent Game VR object from trying to appear at main menu and hangouts
            Transform transformScreenToRemove = canvasObject.transform.Find("HouseRulesUiGameVr");
            if (transformScreenToRemove == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(transformScreenToRemove.gameObject);

            Transform transformScreenToRemove2 = canvasObject.transform.Find("HouseRulesUiGameVr2");
            if (transformScreenToRemove2 == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(transformScreenToRemove2.gameObject);

            Transform transformScreenToRemove3 = canvasObject.transform.Find("HouseRulesUiGameVr3");
            if (transformScreenToRemove3 == null)
            {
                return;
            }

            UnityEngine.Object.Destroy(transformScreenToRemove3.gameObject);
        }

        internal static void OnSceneLoaded(int buildIndex, string sceneName)
        {
            // LogDebug("OnSceneLoaded");
            if (buildIndex == 0 || sceneName.Contains("Startup"))
            {
                return;
            }

            if (Environments.IsPcEdition())
            {
                if (buildIndex == NonVrSteamLobbySceneIndex || buildIndex == NonVrCombinedSteamLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in PC. Loading UI.");
                    _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
                }
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
                // LogDebug($"VR buildIndex: {buildIndex}");
                bool revolutions = false;
                bool progressive = false;
                if (HR.SelectedRuleset != Ruleset.None)
                {
                    LogDebug("Recognized modded game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr", typeof(HouseRulesUiGameVr));
                }

                foreach (var rule in HR.SelectedRuleset.Rules)
                {
                    if (rule.ToString().Contains("RevolutionsRule"))
                    {
                        revolutions = true;
                    }

                    if (rule.ToString().Contains("PieceProgressRule"))
                    {
                        progressive = true;
                    }
                }

                if (revolutions && !HR.SelectedRuleset.Name.Equals("Demeo Reloaded"))
                {
                    LogDebug("Recognized Revolutions game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr2", typeof(HouseRulesUiGameVr2));
                }

                if (progressive)
                {
                    LogDebug("Recognized Progressive game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr3", typeof(HouseRulesUiGameVr3));
                }
            }
        }

        /// <summary>
        /// Load and register all rulesets found in the given directory.
        /// </summary>
        /// <param name="directory">Path of the directory from which to load rulesets.</param>
        internal static void LoadRulesetsFromDirectory(string directory)
        {
            // Uncomment next line to export registered rulesets.
            // ExampleRulesetExporter.ExportRegisteredRulesets("HouseRules\\ExampleRulesets");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            HouseRulesConfigurationBase.LogDebug("LoadRulesetsFromDirectory");
            var rulesetFiles = RulesetImporter.ListRulesets(directory);
            LogInfo($"Found [{rulesetFiles.Count}] custom ruleset files to load.");

            foreach (var file in rulesetFiles)
            {
                try
                {
                    var ruleset = RulesetImporter.Read(file, tolerateFailures: false);
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
