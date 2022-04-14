namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Common;
    using MelonLoader;
    using Octokit;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private const int LobbySceneIndex = 1;
        private const int HangoutsSceneIndex = 43;
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        public override void OnApplicationStart()
        {
            GitHubClient client = new GitHubClient(new ProductHeaderValue("HouseRules"));
            var releases = client.Repository.Release.GetAll("orendain", "DemeoMods").Result;
            foreach (var release in releases)
            {
                if (release.Name.StartsWith("HouseRules"))
                {
                    Logger.Warning($"Latest HouseRules Release {release.Name} has the tag {release.TagName}");

                    var assemblyTitleName = Assembly.GetExecutingAssembly().GetCustomAttribute<AssemblyTitleAttribute>().Title;
                    var assemblyVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
                    Logger.Warning($"My name is {assemblyTitleName} and my version is {assemblyVersion}");
                    Logger.Warning($"My pretty name is {assemblyTitleName.Substring(0, 10)} v{assemblyVersion.Substring(0, 5)}");
                    //var assemblyConfigurationAttribute = typeof(ConfigurationMod).Assemby.GetCustomAttribute<AssemblyConfigurationAttribute>();

                    if (release.Name != assemblyVersion)
                    {
                        // Do the thing that needs to be done.
                    }

                    break; // Releases are listed in reverse chronological order, so the first HouseRules we find will be the latest.
                }
            }

            CommonModule.Initialize();
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
            if (buildIndex == LobbySceneIndex || buildIndex == HangoutsSceneIndex)
            {
                _ = new GameObject("HouseRules_RulesetSelection", typeof(UI.RulesetSelectionUI));
            }
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
