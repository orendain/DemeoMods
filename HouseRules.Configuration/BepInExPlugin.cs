#if BEPINEX
namespace HouseRules.Configuration
{
    using System;
    using System.IO;
    using BepInEx;
    using BepInEx.Logging;
    using HouseRules.Core;
    using UnityEngine.SceneManagement;

    [BepInPlugin(HouseRulesConfigurationBase.ModId, HouseRulesConfigurationBase.ModName, BuildVersion.Version)]
    [BepInDependency("com.orendain.demeomods.houserules.core")]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        private static readonly string RulesetDirectory = Path.Combine(Paths.GameRootPath, "HouseRules");

        private void Awake()
        {
            Log = Logger;
            HouseRulesConfigurationBase.Init(this);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Start()
        {
            LoadConfiguration();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            HouseRulesConfigurationBase.OnSceneLoaded(scene.buildIndex, scene.name);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            HouseRulesConfigurationBase.OnSceneUnloaded(scene.buildIndex, scene.name);
        }

        private void LoadConfiguration()
        {
            var loadRulesetsFromConfigEntry = Config.Bind(
                "General",
                "LoadRulesetsFromConfig",
                true,
                "Whether or not to load rulesets from config files.");
            var shouldLoadRulesetsFromConfig = loadRulesetsFromConfigEntry.Value;
            if (shouldLoadRulesetsFromConfig)
            {
                HouseRulesConfigurationBase.LoadRulesetsFromDirectory(RulesetDirectory);
            }

            var defaultRulesetEntry = Config.Bind(
                "General",
                "DefaultRuleset",
                string.Empty,
                "Default ruleset to have selected.");
            var defaultRuleset = defaultRulesetEntry.Value;
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
                HouseRulesConfigurationBase.LogWarning(
                    $"Failed to select default ruleset [{defaultRuleset}] specified in config: {e}");
            }
        }
    }
}
#endif
