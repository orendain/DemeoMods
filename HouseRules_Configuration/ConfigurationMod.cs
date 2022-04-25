namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using Common;
    using MelonLoader;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        private const string DemeoPCEditionString = "Demeo PC Edition";

        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();

        private const int LobbySceneIndex = 1;
        private const int HangoutsSceneIndex = 43;
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        internal static bool IsUpdateAvailable { get; private set; }

        public override void OnApplicationStart()
        {
            CommonModule.Initialize();
            DetermineIfUpdateAvailable();
        }

        public override void OnApplicationLateStart()
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
            if (MelonUtils.CurrentGameAttribute.Name == DemeoPCEditionString)
            {
                Logger.Msg("PC Edition detected. Skipping VR UI loading.");
                return;
            }

            if (buildIndex == LobbySceneIndex || buildIndex == HangoutsSceneIndex)
            {
                _ = new GameObject("HouseRules_RulesetSelection", typeof(UI.RulesetSelectionUI));
            }
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
                    Logger.Warning($"Failed to import and register ruleset from file [{file}]. Skipping that ruleset: {e}");
                }
            }
        }
    }
}
