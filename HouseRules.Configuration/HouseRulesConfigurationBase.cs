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

        private const int NonVrSteamWindowsLobbySceneIndex = 2;
        private const int NonVrOculusWindowsLobbySceneIndex = 2;
        private const int SteamVRLobbySceneIndex = 1;
        private const int RiftLobbySceneIndex = 1;

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
            IsUpdateAvailable = await VersionChecker.IsUpdateAvailable();
            LogInfo($"{(IsUpdateAvailable ? "New" : "No new")} HouseRules update found.");
        }

        internal static void OnSceneUnloaded(int buildIndex, string sceneName)
        {
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
            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.NonVrSteamWindows)
            {
                if (buildIndex == NonVrSteamWindowsLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in NonVrSteamWindows. Loading UI.");
                    _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
                }

                return;
            }

            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.NonVrOculusWindows)
            {
                if (buildIndex == NonVrOculusWindowsLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in NonVrOculusWindows. Loading UI.");
                    _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
                }

                return;
            }

            if (Environments.IsInHangouts())
            {
                LogDebug("Recognized lobby in Hangouts. Loading UI.");
                _ = new GameObject("HouseRulesUiHangouts", typeof(HouseRulesUiHangouts));
                return;
            }

            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.SteamVR)
            {
                if (buildIndex == SteamVRLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in SteamVR. Loading UI.");
                    _ = new GameObject("HouseRulesUiVr", typeof(HouseRulesUiVr));
                }

                return;
            }

            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.Rift)
            {
                if (buildIndex == RiftLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in Rift. Loading UI.");
                    _ = new GameObject("HouseRulesUiVr", typeof(HouseRulesUiVr));
                }

                return;
            }

            // If a scene is loaded while a ruleset is selected, we must have loaded a game level.
            if (MotherbrainGlobalVars.IsRunningOnVRPlatform && HR.SelectedRuleset != Ruleset.None)
            {
                LogDebug("Recognized modded game in VR. Loading UI.");
                _ = new GameObject("HouseRulesUiGameVr", typeof(HouseRulesUiGameVr));
            }
        }

        /// <summary>
        /// Load and register all rulesets found in the given directory.
        /// </summary>
        /// <param name="directory">Path of the directory from which to load rulesets.</param>
        internal static void LoadRulesetsFromDirectory(string directory)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

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
