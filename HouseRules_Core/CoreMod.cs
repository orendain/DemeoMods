namespace HouseRules
{
    using System;
    using System.Linq;
    using HarmonyLib;
    using HouseRules.Types;
    using MelonLoader;

    internal class CoreMod : MelonMod
    {
        private static readonly Harmony RulesPatcher = new Harmony("com.orendain.demeomods.houserules.core.patcher");

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.houserules.core");
            LifecycleDirector.Patch(harmony);
            BoardSyncer.Patch(harmony);
            HangoutsGameRacer.Patch(harmony);

            HR.Rulebook.Register(Ruleset.None);
        }

        public override void OnApplicationLateStart()
        {
            PatchRegisteredRules();
        }

        private static void PatchRegisteredRules()
        {
            var patchableRules = HR.Rulebook.RuleTypes.Where(typ => typeof(IPatchable).IsAssignableFrom(typ)).ToList();
            HR.Logger.Msg($"Found [{patchableRules.Count}] registered rules that require game patching.");

            foreach (var ruleType in patchableRules)
            {
                HR.Logger.Msg($"Patching game with rule type: {ruleType}");

                var traverse = Traverse.Create(ruleType)
                    .Method("Patch", paramTypes: new[] { typeof(Harmony) }, arguments: new object[] { RulesPatcher });
                if (!traverse.MethodExists())
                {
                    HR.Logger.Warning($"Could not find expected Patch method for rule [{ruleType}]. Skipping patching for that rule.");
                    continue;
                }

                try
                {
                    traverse.GetValue();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    HR.Logger.Error($"Failed to patch game with rule type [{ruleType}]: {e}");
                }
            }
        }
    }
}
