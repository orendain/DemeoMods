namespace SkipIntro
{
    using System;

    internal static class SkipIntroBase
    {
        internal const string ModId = "com.orendain.demeomods.skipintro";
        internal const string ModName = "SkipIntro";
        internal const string ModVersion = "1.5.0";
        internal const string ModAuthor = "DemeoMods Team";

        private static Action<object>? _logDebug;

        internal static void LogDebug(object data) => _logDebug?.Invoke(data);

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Log == null)
                {
                    return;
                }

                _logDebug = plugin.Log.LogDebug;
            }
            #endif

            #if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                _logDebug = mod.LoggerInstance.Msg;
            }
            #endif
        }
    }
}
