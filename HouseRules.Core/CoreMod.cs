namespace HouseRules.Core
{
    using System;
    using System.Linq;
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;
    using HouseRules.Core.Types;

    [BepInPlugin("com.orendain.demeomods.houserules.core", "HouseRules.Core", "2.0.0")]
    public class CoreMod : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }

        private static Harmony _harmony;


        private void Awake()
        {
            Log = Logger;
            _harmony = new Harmony("com.orendain.demeomods.houserules.core");
            LifecycleDirector.Patch(_harmony);
            BoardSyncer.Patch(_harmony);

            HR.Rulebook.Register(Ruleset.None);
        }

        private void Start()
        {
            Log.LogDebug("About to start coremod.start.");
            PatchRegisteredRules();
        }

        private void PatchRegisteredRules()
        {
            var patchableRules = HR.Rulebook.RuleTypes.Where(typ => typeof(IPatchable).IsAssignableFrom(typ)).ToList();
            Log.LogDebug($"Found [{patchableRules.Count}] registered rules that require game patching.");

            foreach (var ruleType in patchableRules)
            {
                Log.LogDebug($"Patching game with rule type: {ruleType}");

                var traverse = Traverse.Create(ruleType)
                    .Method("Patch", paramTypes: new[] { typeof(Harmony) }, arguments: new object[] { _harmony });
                if (!traverse.MethodExists())
                {
                    Log.LogWarning($"Could not find expected Patch method for rule [{ruleType}]. Skipping patching for that rule.");
                    continue;
                }

                try
                {
                    traverse.GetValue();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    Log.LogError($"Failed to patch game with rule type [{ruleType}]: {e}");
                }
            }
        }
    }
}
