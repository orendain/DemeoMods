namespace HouseRules.Core
{
    using System;
    using System.Linq;
    using HarmonyLib;
    using HouseRules.Core.Types;
    using MelonLoader;

    internal class CoreMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Core");

        public override void OnInitializeMelon()
        {
            LifecycleDirector.Patch(HarmonyInstance);
            BoardSyncer.Patch(HarmonyInstance);

            HR.Rulebook.Register(Ruleset.None);
        }

        public override void OnLateInitializeMelon()
        {
            PatchRegisteredRules();
        }

        private void PatchRegisteredRules()
        {
            var patchableRules = HR.Rulebook.RuleTypes.Where(typ => typeof(IPatchable).IsAssignableFrom(typ)).ToList();
            Logger.Msg($"Found [{patchableRules.Count}] registered rules that require game patching.");

            foreach (var ruleType in patchableRules)
            {
                Logger.Msg($"Patching game with rule type: {ruleType}");

                var traverse = Traverse.Create(ruleType)
                    .Method("Patch", paramTypes: new[] { typeof(Harmony) }, arguments: new object[] { HarmonyInstance });
                if (!traverse.MethodExists())
                {
                    Logger.Warning($"Could not find expected Patch method for rule [{ruleType}]. Skipping patching for that rule.");
                    continue;
                }

                try
                {
                    traverse.GetValue();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    Logger.Error($"Failed to patch game with rule type [{ruleType}]: {e}");
                }
            }
        }
    }
}
