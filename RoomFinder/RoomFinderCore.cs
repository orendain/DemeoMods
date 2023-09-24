namespace RoomFinder
{
    using System;
    using RoomFinder.UI;
    using UnityEngine;

    internal static class RoomFinderCore
    {
        internal const string ModId = "com.orendain.demeomods.roomfinder";
        internal const string ModName = "RoomFinder";
        internal const string ModVersion = "1.8.0";
        internal const string ModAuthor = "DemeoMods Team";

        internal static Action<object> LogMessage;
        internal static Action<object> LogInfo;
        internal static Action<object> LogDebug;
        internal static Action<object> LogWarning;
        internal static Action<object> LogError;

        internal static readonly SharedState SharedState = SharedState.NewInstance();

        private const int PC1LobbySceneIndex = 1;
        private const int PC2LobbySceneIndex = 3;

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

                Patcher.Patch(plugin.Harmony);
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

                Patcher.Patch(mod.HarmonyInstance);
            }
#endif
        }

        internal static void OnSceneLoaded(int buildIndex)
        {
            LogDebug("OnSceneLoaded");
            if (MotherbrainGlobalVars.IsRunningOnNonVRPlatform)
            {
                if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
                {
                    LogInfo($"{buildIndex} scene failed to load PC lobby");
                    return;
                }

                LogDebug("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("RoomFinderUiNonVr", typeof(RoomFinderUiNonVr));
                return;
            }

            if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
            {
                return;
            }

            LogDebug("Recognized lobby in VR. Loading UI.");
            _ = new GameObject("RoomFinderUiVr", typeof(RoomFinderUiVr));
        }
    }
}
