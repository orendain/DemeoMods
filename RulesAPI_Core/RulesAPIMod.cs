namespace RulesAPI
{
    using System;
    using System.Linq;
    using HarmonyLib;
    using MelonLoader;

    internal class RulesAPIMod : MelonMod
    {
        private static readonly Harmony RulesPatcher = new Harmony("com.orendain.demeomods.rulesapi.patcher");

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.rulesapi");
            ModPatcher.Patch(harmony);
        }

        public override void OnApplicationLateStart()
        {
            PatchRegisteredRules();
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
