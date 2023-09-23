namespace SkipIntro
{
    using System;

    internal static class SkipIntro
    {
        internal const string ModId = "com.orendain.demeomods.skipintro";
        internal const string ModName = "SkipIntro";
        internal const string ModVersion = "1.4.0";
        internal const string ModAuthor = "DemeoMods Team";

        public static Action<object> LogMessage;
        public static Action<object> LogInfo;
        public static Action<object> LogDebug;
        public static Action<object> LogWarning;
        public static Action<object> LogError;

        public static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                LogMessage = plugin.Log.LogMessage;
                LogInfo = plugin.Log.LogInfo;
                LogDebug = plugin.Log.LogDebug;
                LogWarning = plugin.Log.LogWarning;
                LogError = plugin.Log.LogError;
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
            }
            #endif
        }
    }
}
