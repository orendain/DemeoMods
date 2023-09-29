#if BEPINEX
namespace RoomFinder
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin(RoomFinderBase.ModId, RoomFinderBase.ModName, RoomFinderBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        internal Harmony? Harmony { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony = new Harmony(RoomFinderBase.ModId);
            RoomFinderBase.Init(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RoomFinderBase.OnSceneLoaded(scene.buildIndex);
        }
    }
}
#endif
