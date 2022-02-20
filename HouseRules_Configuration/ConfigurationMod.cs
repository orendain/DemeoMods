namespace HouseRules.Configuration
{
    using System;
    using System.Text;
    using Boardgame;
    using MelonLoader;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private const int LobbySceneIndex = 1;

        private static bool _hadNoLoadingIssues = true;

        public override void OnApplicationLateStart()
        {
            ExampleRulesetExporter.ExportExampleRulesetIfNeeded();

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
            if (buildIndex != LobbySceneIndex)
            {
                return;
            }

            _ = new GameObject("RulesetSelectionUI", typeof(UI.RulesetSelectionUI));

            if (_hadNoLoadingIssues)
            {
                return;
            }

            ShowStartupIssuesMessage();
        }

        private static void LoadRulesetsFromConfig()
        {
            _hadNoLoadingIssues = ConfigManager.TryImportRulesets(out var rulesets);
            foreach (var ruleset in rulesets)
            {
                HR.Rulebook.Register(ruleset);
                Logger.Msg($"Loaded and registered ruleset from config: {ruleset.Name}");
            }
        }

        private static void ShowStartupIssuesMessage()
        {
            var message = new StringBuilder()
                .AppendLine("Attention:")
                .AppendLine().AppendLine("HouseRules encountered issues loading at least one of your custom rulesets.")
                .AppendLine().AppendLine("Those rulesets have been left out. You may proceed without them.")
                .AppendLine().AppendLine("See logs for more information.")
                .ToString();
            GameUI.ShowCameraMessage(message, 15f);
        }
    }
}
