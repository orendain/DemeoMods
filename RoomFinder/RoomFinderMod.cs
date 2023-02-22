﻿namespace RoomFinder
{
    using Common;
    using HarmonyLib;
    using MelonLoader;
    using RoomFinder.UI;
    using UnityEngine;

    internal class RoomFinderMod : MelonMod
    {
        private const string DemeoPCEditionString = "Demeo PC Edition";
        private const int LobbySceneIndex = 1;

        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomFinder");
        internal static readonly SharedState SharedState = SharedState.NewInstance();

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.roomfinder");
            Patcher.Patch(harmony);
            CommonModule.Initialize();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (MotherbrainGlobalVars.IsRunningOnNonVRPlatform)
                {
                    if (buildIndex != LobbySceneIndex)
                {
                    return;
                }

                Logger.Msg("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("RoomFinderUiNonVr", typeof(RoomFinderUiNonVr));
                return;
            }

            if (buildIndex != LobbySceneIndex)
            {
                return;
            }

            Logger.Msg("Recognized lobby in VR. Loading UI.");
            _ = new GameObject("RoomFinderUiVr", typeof(RoomFinderUiVr));
        }
    }
}
