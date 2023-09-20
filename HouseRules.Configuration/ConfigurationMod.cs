namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using Common.UI;
    using HouseRules.Configuration.UI;
    using HouseRules.Core;
    using HouseRules.Core.Types;
    using MelonLoader;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        private const int PC1LobbySceneIndex = 1;
        private const int PC2LobbySceneIndex = 3;

        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        internal static bool IsUpdateAvailable { get; private set; }

        public override void OnInitializeMelon()
        {
            DetermineIfUpdatesAvailable();
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

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            // Logger.Msg($"Scene unloaded {buildIndex} - {sceneName}");
            GameObject canvasObject = GameObject.Find("~LeanTween");
            if (canvasObject == null)
            {
                return;
            }

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

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
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
                    // Logger.Msg($"PC buildIndex: {buildIndex}");
                    return;
                }

                Logger.Msg("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("HouseRulesUiNonVr", typeof(HouseRulesUiNonVr));
            }
            else if (Environments.IsInHangouts() || sceneName.Contains("HobbyShop"))
            {
                Logger.Msg("Recognized lobby in Hangouts. Loading UI.");
                _ = new GameObject("HouseRulesUiHangouts", typeof(HouseRulesUiHangouts));
            }
            else if (sceneName.Contains("Lobby"))
            {
                Logger.Msg("Recognized lobby in VR. Loading UI.");
                _ = new GameObject("HouseRulesUiVr", typeof(HouseRulesUiVr));
            }
            else
            {
                // Logger.Msg($"VR buildIndex: {buildIndex}");
                bool revolutions = false;
                bool progressive = false;
                if (HR.SelectedRuleset != Ruleset.None)
                {
                    Logger.Msg("Recognized modded game in VR. Loading UI.");
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
                    Logger.Msg("Recognized Revolutions game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr2", typeof(HouseRulesUiGameVr2));
                }

                if (progressive)
                {
                    Logger.Msg("Recognized Progressive game in VR. Loading UI.");
                    _ = new GameObject("HouseRulesUiGameVr3", typeof(HouseRulesUiGameVr3));
                }
            }
        }

        private static async void DetermineIfUpdatesAvailable()
        {
            IsUpdateAvailable = await RevolutionsChecker.IsUpdateAvailable();
            Logger.Msg($"{(IsUpdateAvailable ? "New" : "No new")} Revolutions update found.");
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
