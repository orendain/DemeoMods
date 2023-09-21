namespace RoomFinder
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;
    using RoomFinder.UI;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    [BepInPlugin("com.orendain.demeomods.roomfinder", "RoomFinder", "2.0.0")]
    public class RoomFinderMod : BaseUnityPlugin
    {
        private const int PC1LobbySceneIndex = 1;
        private const int PC2LobbySceneIndex = 3;

        internal static ManualLogSource Log { get; private set; }

        internal static readonly SharedState SharedState = SharedState.NewInstance();

        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony("com.orendain.demeomods.roomfinder");
            Patcher.Patch(harmony);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var buildIndex = scene.buildIndex;

            if (MotherbrainGlobalVars.IsRunningOnNonVRPlatform)
            {
                if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
                {
                    Logger.LogInfo($"{buildIndex} scene failed to load PC lobby");
                    return;
                }

                Logger.LogDebug("Recognized lobby in PC. Loading UI.");
                _ = new GameObject("RoomFinderUiNonVr", typeof(RoomFinderUiNonVr));
                return;
            }

            if (buildIndex != PC1LobbySceneIndex && buildIndex != PC2LobbySceneIndex)
            {
                return;
            }

            Logger.LogDebug("Recognized lobby in VR. Loading UI.");
            _ = new GameObject("RoomFinderUiVr", typeof(RoomFinderUiVr));
        }
    }
}
