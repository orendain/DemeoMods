#if BEPINEX
namespace RoomFinder
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin(RoomFinderCore.ModId, RoomFinderCore.ModName, RoomFinderCore.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource Log { get; private set; }

        internal Harmony Harmony { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony = new Harmony(RoomFinderCore.ModId);
            RoomFinderCore.Init(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            RoomFinderCore.OnSceneLoaded(scene.buildIndex);
        }
    }
}
#endif
