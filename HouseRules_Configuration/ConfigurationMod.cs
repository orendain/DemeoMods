namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using Common;
    using MelonLoader;
    using Newtonsoft.Json.Linq;
    using UnityEngine;

    internal class ConfigurationMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        internal static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();

        internal static string LatestHouseRulesVersion { get; private set; }

        private const int LobbySceneIndex = 1;
        private const int HangoutsSceneIndex = 43;
        private static readonly List<string> FailedRulesetFiles = new List<string>();

        public override void OnApplicationStart()
        {
            CommonModule.Initialize();
            FindLatestReleaseVersion();
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

        /// <summary>
        /// Finds the latest HouseRules release version, returning the empty string if one can not be found.
        /// </summary>
        private static async void FindLatestReleaseVersion()
        {
            Logger.Msg("Checking for latest HouseRules release.");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "HouseRules");

            var responseString = await client.GetStringAsync("https://api.github.com/repos/orendain/DemeoMods/releases");
            var responseJson = JArray.Parse(responseString);
            foreach (var obj in responseJson.Children<JObject>())
            {
                if (obj["tag_name"] == null)
                {
                    continue;
                }

                if (!TryParseVersion(obj["tag_name"].ToString(), out var version))
                {
                    continue;
                }

                Logger.Msg($"Found latest HouseRules release: {version}");
                LatestHouseRulesVersion = version;
                return;
            }

            LatestHouseRulesVersion = string.Empty;
        }

        /// <summary>
        /// Extracts a version from a standard HouseRules tag name.
        /// </summary>
        private static bool TryParseVersion(string tag, out string version)
        {
            if (!tag.EndsWith("-houserules"))
            {
                version = string.Empty;
                return false;
            }

            version = tag.Substring(1).Replace("-houserules", string.Empty);
            return true;
        }
    }
}
