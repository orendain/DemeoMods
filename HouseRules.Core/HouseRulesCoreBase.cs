namespace HouseRules.Core
{
    using System;
    using System.Linq;
    using HarmonyLib;
    using HouseRules.Core.Types;

    internal static class HouseRulesCoreBase
    {
        internal const string ModId = "com.orendain.demeomods.houserules.core";
        internal const string ModName = "HouseRules.Core";
        internal const string ModAuthor = "DemeoMods Team";

        private static Action<object>? _logInfo;
        private static Action<object>? _logDebug;
        private static Action<object>? _logWarning;
        private static Action<object>? _logError;

        private static Harmony _harmony;

        internal static void LogInfo(object data) => _logInfo?.Invoke(data);

        internal static void LogDebug(object data) => _logDebug?.Invoke(data);

        internal static void LogWarning(object data) => _logWarning?.Invoke(data);

        internal static void LogError(object data) => _logError?.Invoke(data);

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Log == null)
                {
                    LogError("Logger instance is invalid. Cannot initialize.");
                    return;
                }

                _logInfo = plugin.Log.LogInfo;
                _logDebug = plugin.Log.LogDebug;
                _logWarning = plugin.Log.LogWarning;
                _logError = plugin.Log.LogError;

                if (plugin.Harmony == null)
                {
                    LogError("Harmony instance is invalid. Cannot initialize.");
                    return;
                }

                _harmony = plugin.Harmony;
            }
            #endif

            #if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                _logInfo = mod.LoggerInstance.Msg;
                _logDebug = mod.LoggerInstance.Msg;
                _logWarning = mod.LoggerInstance.Warning;
                _logError = mod.LoggerInstance.Error;

                _harmony = mod.HarmonyInstance;
            }
            #endif

            LifecycleDirector.Patch(_harmony);
            BoardSyncer.Patch(_harmony);
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
