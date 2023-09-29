namespace RoomCode
{
    using System;
    using System.Collections.Generic;

    internal static class RoomCodeBase
    {
        internal const string ModId = "com.orendain.demeomods.roomcode";
        internal const string ModName = "RoomCode";
        internal const string ModVersion = "1.2.1";
        internal const string ModAuthor = "DemeoMods Team";

        private static Action<object>? _logInfo;
        private static Action<object>? _logDebug;

        internal static void LogInfo(object data) => _logInfo?.Invoke(data);

        internal static void LogDebug(object data) => _logDebug?.Invoke(data);

        internal static bool Enabled { get; private set; }

        internal static List<string> RoomCodes { get; private set; } = new();

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Log != null)
                {
                    _logInfo = plugin.Log.LogInfo;
                    _logDebug = plugin.Log.LogDebug;
                }

                Enabled = plugin.Enabled;
                RoomCodes = plugin.RoomCodes;

                ModPatcher.Patch(plugin.Harmony);
            }
            #endif

            #if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                _logInfo = mod.LoggerInstance.Msg;
                _logDebug = mod.LoggerInstance.Msg;

                Enabled = mod.Enabled.Value;
                RoomCodes = mod.RoomCodes.Value;

                ModPatcher.Patch(mod.HarmonyInstance);
            }
            #endif
        }
    }
}
