#if BEPINEX
namespace AdvancedStats
{
    using BepInEx;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin(AdvancedStatsCore.ModId, AdvancedStatsCore.ModName, AdvancedStatsCore.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal Harmony Harmony { get; private set; }

        private void Awake()
        {
            Harmony = new Harmony(AdvancedStatsCore.ModId);
            AdvancedStatsCore.Init(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AdvancedStatsCore.OnSceneLoaded(scene.name);
        }
    }
}
#endif
