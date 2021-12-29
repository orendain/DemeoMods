using System;
using Common.Patches;
using MelonLoader;
using RoomFinder.UI;
using UnityEngine;

namespace RoomFinder
{
    internal class RoomFinderMod : MelonMod
    {
        private const int SteamLobbySceneIndex = 1;
        private const string QuestLobbySceneName = "Lobby";

        public override void OnApplicationStart()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.roomfinder");
            RoomFinderPatcher.Patch(harmony);
            GameContextPatcher.Patch(harmony);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex == SteamLobbySceneIndex || sceneName.Equals(QuestLobbySceneName, StringComparison.OrdinalIgnoreCase))
            {
                MelonLogger.Msg($"Initializing RoomFinder in scene [{sceneName}] with scene index [{buildIndex}].");
                new GameObject("RoomListUI", typeof(RoomListUI));
            }
        }
    }
}
