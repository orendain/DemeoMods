namespace HouseRules.Configuration
{
    using System;
    using MelonLoader;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        private static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private const int LobbySceneIndex = 1;

        private GameObject _rulesetSelectionUI;

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
            if (buildIndex == LobbySceneIndex)
            {
                _rulesetSelectionUI = new GameObject("RulesetSelectionUI", typeof(UI.RulesetSelectionUI));
            }
        }

        private static void LoadRulesetsFromConfig()
        {
            var rulesets = ConfigManager.ImportRulesets();
            foreach (var ruleset in rulesets)
            {
                try
                {
                    HR.Rulebook.Register(ruleset);
                    Logger.Msg($"Loaded and registered ruleset from config: {ruleset.Name}");
                }
                catch (Exception e)
                {
                    Logger.Warning($"Failed to load and register ruleset [{ruleset.Name}] from config: {e}");
                }
            }
        }
    }
}
