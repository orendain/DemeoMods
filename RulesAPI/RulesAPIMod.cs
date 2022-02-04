namespace RulesAPI
{
    using System;
    using System.Linq;
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
            PatchRegisteredRules();
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

        private static void PatchRegisteredRules()
        {
            var patchableRules = Registrar.Instance().RuleTypes.Where(typ => typeof(IPatchable).IsAssignableFrom(typ)).ToList();

            RulesAPI.Logger.Msg($"Found [{patchableRules.Count}] registered rules that require game patching.");

            foreach (var ruleType in patchableRules)
            {
                RulesAPI.Logger.Msg($"Patching game with rule type: {ruleType}");

                var traverse = Traverse.Create(ruleType).Method("Patch", paramTypes: new[] { typeof(Harmony) }, arguments: new object[] { RulesPatcher });
                if (!traverse.MethodExists())
                {
                    RulesAPI.Logger.Warning($"Could not find expected Patch method for rule [{ruleType}]. Skipping patching for that rule.");
                    continue;
                }

                try
                {
                    traverse.GetValue();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    RulesAPI.Logger.Error($"Failed to patch game with rule type [{ruleType}]: {e}");
                }
            }
        }
    }
}
