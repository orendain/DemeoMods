﻿namespace RoomFinder
{
    using System;
    using HarmonyLib;
    using RoomFinder.UI;
    using UnityEngine;

    internal static class RoomFinderBase
    {
        internal const string ModId = "com.orendain.demeomods.roomfinder";
        internal const string ModName = "RoomFinder";
        internal const string ModVersion = "2.0.0";
        internal const string ModAuthor = "DemeoMods Team";

        private const int NonVrSteamWindowsLobbySceneIndex = 2;
        private const int NonVrOculusWindowsLobbySceneIndex = 2;
        private const int SteamVRLobbySceneIndex = 1;
        private const int RiftLobbySceneIndex = 1;

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

            RoomManager.Patch(_harmony);
        }

        internal static void OnSceneLoaded(int buildIndex)
        {
            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.NonVrSteamWindows)
            {
                if (buildIndex == NonVrSteamWindowsLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in NonVrSteamWindows. Loading UI.");
                    _ = new GameObject("RoomFinderUiNonVr", typeof(RoomFinderUiNonVr));
                }

                return;
            }

            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.NonVrOculusWindows)
            {
                if (buildIndex == NonVrOculusWindowsLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in NonVrOculusWindows. Loading UI.");
                    _ = new GameObject("RoomFinderUiNonVr", typeof(RoomFinderUiNonVr));
                }

                return;
            }

            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.SteamVR)
            {
                if (buildIndex == SteamVRLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in SteamVR. Loading UI.");
                    _ = new GameObject("RoomFinderUiVr", typeof(RoomFinderUiVr));
                }

                return;
            }

            if (MotherbrainGlobalVars.SelectedPlatform == MotherbrainPlatform.Rift)
            {
                if (buildIndex == RiftLobbySceneIndex)
                {
                    LogDebug("Recognized lobby in Rift. Loading UI.");
                    _ = new GameObject("RoomFinderUiVr", typeof(RoomFinderUiVr));
                }
            }
        }
    }
}
