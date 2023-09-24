namespace HouseRules.Core
{
    using System;
    using System.Linq;
    using HarmonyLib;
    using HouseRules.Core.Types;

    internal static class HouseRulesCoreCore
    {
        internal const string ModId = "com.orendain.demeomods.houserules.core";
        internal const string ModName = "HouseRules.Core";
        internal const string ModVersion = "1.8.0";
        internal const string ModAuthor = "DemeoMods Team";

        internal static Action<object> LogMessage;
        internal static Action<object> LogInfo;
        internal static Action<object> LogDebug;
        internal static Action<object> LogWarning;
        internal static Action<object> LogError;

        private static Harmony _harmony;

        internal static void Init(object loader)
        {
#if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                LogMessage = plugin.Log.LogMessage;
                LogInfo = plugin.Log.LogInfo;
                LogDebug = plugin.Log.LogDebug;
                LogWarning = plugin.Log.LogWarning;
                LogError = plugin.Log.LogError;

                _harmony = plugin.Harmony;
                LifecycleDirector.Patch(_harmony);
                BoardSyncer.Patch(_harmony);
            }
#endif

#if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                LogMessage = mod.LoggerInstance.Msg;
                LogInfo = mod.LoggerInstance.Msg;
                LogDebug = mod.LoggerInstance.Msg;
                LogWarning = mod.LoggerInstance.Warning;
                LogError = mod.LoggerInstance.Error;

                _harmony = mod.HarmonyInstance;
                LifecycleDirector.Patch(_harmony);
                BoardSyncer.Patch(_harmony);
            }
#endif

            HR.Rulebook.Register(Ruleset.None);
        }

        internal static void PatchRegisteredRules()
        {
            var patchableRules = HR.Rulebook.RuleTypes.Where(typ => typeof(IPatchable).IsAssignableFrom(typ)).ToList();
            LogDebug($"Found [{patchableRules.Count}] registered rules that require game patching.");

            foreach (var ruleType in patchableRules)
            {
                LogDebug($"Patching game with rule type: {ruleType}");

                var traverse = Traverse.Create(ruleType)
                    .Method("Patch", paramTypes: new[] { typeof(Harmony) }, arguments: new object[] { _harmony });
                if (!traverse.MethodExists())
                {
                    LogWarning($"Could not find expected Patch method for rule [{ruleType}]. Skipping patching for that rule.");
                    continue;
                }

                try
                {
                    traverse.GetValue();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Perm disable rules/rulesets that fail to patch/load.
                    LogError($"Failed to patch game with rule type [{ruleType}]: {e}");
                }
            }
        }
    }
}
