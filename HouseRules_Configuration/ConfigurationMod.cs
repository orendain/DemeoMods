namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using Common;
    using Common.UI;
    using HouseRules.Configuration.UI;
    using MelonLoader;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        private const int LobbySceneIndex = 1;

        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        internal static bool IsUpdateAvailable { get; private set; }

        public override void OnInitializeMelon()
        {
            CommonModule.Initialize(HarmonyInstance);
            DetermineIfUpdateAvailable();
        }

        public override void OnLateInitializeMelon()
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
                Logger.Warning($"Failed to select default ruleset [{rulesetName}] specified in config: {e}");
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (Environments.IsPcEdition())
            {
                if (buildIndex != LobbySceneIndex)
                {
                    return;
                }

                Logger.Msg("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
                return;
            }

            if (Environments.IsInHangouts())
            {
                Logger.Msg("Recognized lobby in Hangouts. Loading UI.");
                _ = new GameObject("HouseRulesUiHangouts", typeof(HouseRulesUiHangouts));
                return;
            }

            if (buildIndex != LobbySceneIndex)
            {
                return;
            }

            Logger.Msg("Recognized lobby in VR. Loading UI.");
            _ = new GameObject("HouseRulesUiVr", typeof(HouseRulesUiVr));
        }

        private static async void DetermineIfUpdateAvailable()
        {
            IsUpdateAvailable = await VersionChecker.IsUpdateAvailable();
            Logger.Msg($"{(IsUpdateAvailable ? "New" : "No new")} HouseRules update found.");
        }

        private static void LoadRulesetsFromConfig()
        {
            var rulesetFiles = ConfigManager.RulesetFiles;
            Logger.Msg($"Found [{rulesetFiles.Count}] ruleset files in configuration.");

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
                    Logger.Warning(
                        $"Failed to import and register ruleset from file [{file}]. Skipping that ruleset: {e}");
                }
            }
        }
    }
}
