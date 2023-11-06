namespace FreeCamera
{
    using System;
    using HarmonyLib;

    internal static class FreeCameraBase
    {
        internal const string ModId = "com.orendain.demeomods.freecamera";
        internal const string ModName = "FreeCamera";
        internal const string ModVersion = "1.0.0";
        internal const string ModAuthor = "DemeoMods Team";

        private static Action<object>? _logError;

        private static Harmony _harmony;

        internal static void LogError(object data) => _logError?.Invoke(data);

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Log == null)
                {
                    return;
                }

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
                _logError = mod.LoggerInstance.Error;
                _harmony = mod.HarmonyInstance;
            }
            #endif

            NonVrMouseSupporter.Patch(_harmony);
        }
    }
}
