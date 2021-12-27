using Common.Patches;
using MelonLoader;
using RoomFinder.UI;
using UnityEngine;

namespace RoomFinder
{
    internal class RoomFinderMod : MelonMod
    {
        private const int LobbySceneIndex = 1;

        public override void OnApplicationStart()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.roomfinder");
            RoomFinderPatcher.Patch(harmony);
            GameContextPatcher.Patch(harmony);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex == LobbySceneIndex)
            {
                new GameObject("RoomListUI", typeof(RoomListUI));
            }
        }
    }
}