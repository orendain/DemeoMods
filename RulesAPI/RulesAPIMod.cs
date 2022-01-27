namespace RulesAPI
{
    using System;
    using HarmonyLib;
    using MelonLoader;

    internal class RulesAPIMod : MelonMod
    {
        private static readonly Harmony RulesPatcher = new Harmony("com.orendain.demeomods.rulesapi.patcher");

        private MelonPreferences_Category _configCategory;
        private MelonPreferences_Entry<string> _rulesetConfig;

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.rulesapi");
            ModPatcher.Patch(harmony);
        }

        public override void OnApplicationLateStart()
        {
            LoadRegisteredRules();
            InitializeConfig();
        }

        public override void OnApplicationQuit()
        {
            _rulesetConfig.Value = RulesAPI.SelectedRuleset != null ? RulesAPI.SelectedRuleset.Name : string.Empty;
            _configCategory.SaveToFile();
        }

        private void InitializeConfig()
        {
            _configCategory = MelonPreferences.CreateCategory("RulesAPI");
            _configCategory.SetFilePath("RulesAPI.cfg");

            _rulesetConfig = _configCategory.CreateEntry("ruleset", string.Empty);
            if (string.IsNullOrEmpty(_rulesetConfig.Value))
            {
                return;
            }

            try
            {
                RulesAPI.SelectRuleset(_rulesetConfig.Value);
            }
            catch (ArgumentException e)
            {
                RulesAPI.Logger.Warning($"Failed to select ruleset [{_rulesetConfig.Value}] from config: {e}");
            }
        }

        private static void LoadRegisteredRules()
        {
            RulesAPI.Logger.Msg($"Loading [{Registrar.Instance().RuleTypes.Count}] registered rule types.");

            foreach (var ruleType in Registrar.Instance().RuleTypes)
            {
                RulesAPI.Logger.Msg($"Loading rule type: {ruleType}");
                var patchMethod = AccessTools.Method(ruleType, "OnPatch");
                if (patchMethod == null)
                {
                    RulesAPI.Logger.Warning($"Did not find suitable 'OnPatch' method for rule [{ruleType}]. No patching is done for that rule.");
                    continue;
                }

                try
                {
                    patchMethod.Invoke(null, new object[] { RulesPatcher });
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    RulesAPI.Logger.Error($"Failed to apply patch for rule [{ruleType}]: {e}");
                }
            }
        }
    }
}
