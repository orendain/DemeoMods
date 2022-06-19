namespace RoomFinder
{
    using Common;
    using HarmonyLib;
    using MelonLoader;
    using RoomFinder.UI;
    using UnityEngine;

    internal class RoomFinderMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomFinder");
        internal static readonly SharedState SharedState = SharedState.NewInstance();

        private const int LobbySceneIndex = 1;

        private GameObject _roomFinderUi;

        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.roomfinder");
            Patcher.Patch(harmony);
            CommonModule.Initialize();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex != LobbySceneIndex)
            {
                return;
            }

            _roomFinderUi = new GameObject("RoomFinderUI", typeof(RoomFinderUI));
        }
    }
}
