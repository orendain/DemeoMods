namespace RoomFinder
{
    using HarmonyLib;
    using MelonLoader;
    using RoomFinder.UI;
    using UnityEngine;

    internal class RoomFinderMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomFinder");
        internal static readonly ModState ModState = ModState.NewInstance();

        private const int LobbySceneIndex = 1;

        private GameObject _roomListUI;

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.roomfinder");
            ModPatcher.Patch(harmony);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != LobbySceneIndex)
            {
                return;
            }

            _roomListUI = new GameObject("RoomListUI", typeof(RoomListUI));
        }
    }
}
