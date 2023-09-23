namespace RoomCode
{
    using System;
    using System.Collections.Generic;

    internal static class RoomCode
    {
        internal const string ModId = "com.orendain.demeomods.roomcode";
        internal const string ModName = "RoomCode";
        internal const string ModVersion = "1.2.1";
        internal const string ModAuthor = "DemeoMods Team";

        internal static Action<object> LogMessage;
        internal static Action<object> LogInfo;
        internal static Action<object> LogDebug;
        internal static Action<object> LogWarning;
        internal static Action<object> LogError;

        internal static bool Enabled { get; private set; }

        internal static List<string> RoomCodes { get; private set; }

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

                Enabled = plugin.Enabled;
                RoomCodes = plugin.RoomCodes;

                ModPatcher.Patch(plugin.Harmony);
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

                Enabled = mod.Enabled.Value;
                RoomCodes = mod.RoomCodes.Value;

                ModPatcher.Patch(mod.HarmonyInstance);
            }
            #endif
        }
    }
}
