namespace RoomFinder
{
    using Common.Patches;
    using MelonLoader;
    using RoomFinder.UI;
    using UnityEngine;

    internal class RoomFinderMod : MelonMod
    {
        public static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomFinder");

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
                RoomFinderMod.Logger.Msg($"Initializing RoomFinder in scene [{sceneName}] with scene index [{buildIndex}].");
                new GameObject("RoomListUI", typeof(RoomListUI));
            }
        }
    }
}
