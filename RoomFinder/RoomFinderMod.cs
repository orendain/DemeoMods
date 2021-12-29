namespace RoomFinder
{
    using Common;
    using Common.States;
    using MelonLoader;
    using RoomFinder.UI;
    using UnityEngine;

    internal class RoomFinderMod : MelonMod
    {
        public static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomFinder");
        public static readonly GameContextState GameContextState = CommonModule.GameContextState;
        public static readonly ModState ModState = ModState.NewInstance();

        private const int LobbySceneIndex = 1;

        private GameObject _roomListUI;

        public override void OnApplicationStart()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.roomfinder");
            ModPatcher.Patch(harmony);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex == LobbySceneIndex)
            {
                Logger.Msg($"Initializing RoomFinder in scene [{sceneName}] with scene index [{buildIndex}].");
                _roomListUI = new GameObject("RoomListUI", typeof(RoomListUI));
            }
        }
    }
}
