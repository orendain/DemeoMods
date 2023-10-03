#if MELONLOADER
using HouseRules.Configuration;
using MelonLoader;

[assembly: MelonInfo(
    typeof(MelonLoaderMod),
    HouseRulesConfigurationBase.ModName,
    HouseRulesConfigurationBase.ModVersion,
    HouseRulesConfigurationBase.ModAuthor,
    "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("574512")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace HouseRules.Configuration
{
    using System;
    using System.IO;
    using HouseRules.Core;

    internal class MelonLoaderMod : MelonMod
    {
        internal static readonly string RulesetDirectory = Path.Combine(MelonUtils.BaseDirectory, "HouseRules");

        public override void OnInitializeMelon()
        {
            HouseRulesConfigurationBase.Init(this);
        }

        public override void OnLateInitializeMelon()
        {
            LoadConfiguration();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            HouseRulesConfigurationBase.OnSceneLoaded(buildIndex, sceneName);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            HouseRulesConfigurationBase.OnSceneUnloaded(buildIndex, sceneName);
        }

        private void LoadConfiguration()
        {
            var configCategory = MelonPreferences.CreateCategory("HouseRules");
            var shouldLoadRulesetsFromConfig = configCategory.CreateEntry("loadRulesetsFromConfig", true).Value;
            if (shouldLoadRulesetsFromConfig)
            {
                HouseRulesConfigurationBase.LoadRulesetsFromDirectory(RulesetDirectory);
            }

            var defaultRuleset = configCategory.CreateEntry("defaultRuleset", string.Empty).Value;
            if (string.IsNullOrEmpty(defaultRuleset))
            {
                return;
            }

            try
            {
                HR.SelectRuleset(defaultRuleset);
            }
            catch (ArgumentException e)
            {
                HouseRulesConfigurationBase.LogWarning($"Failed to select default ruleset [{defaultRuleset}] specified in config: {e}");
            }
        }
    }
}
#endif
