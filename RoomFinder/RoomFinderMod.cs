namespace RoomFinder
{
    using MelonLoader;
    using RoomFinder.UI;
    using UnityEngine;

    internal class RoomFinderMod : MelonMod
    {
        private const int PC1LobbySceneIndex = 1;
        private const int PC2LobbySceneIndex = 3;

        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RoomFinder");
        internal static readonly SharedState SharedState = SharedState.NewInstance();

        public override void OnInitializeMelon()
        {
            Patcher.Patch(HarmonyInstance);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (MotherbrainGlobalVars.IsRunningOnNonVRPlatform)
            {
                if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
                {
                    MelonLogger.Msg($"{buildIndex} scene failed to load PC lobby");
                    return;
                }

                Logger.Msg("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("RoomFinderUiNonVr", typeof(RoomFinderUiNonVr));
                return;
            }

            if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
            {
                return;
            }

            Logger.Msg("Recognized lobby in VR. Loading UI.");
            _ = new GameObject("RoomFinderUiVr", typeof(RoomFinderUiVr));
        }
    }
}
