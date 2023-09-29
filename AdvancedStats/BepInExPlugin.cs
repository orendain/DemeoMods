#if BEPINEX
namespace AdvancedStats
{
    using BepInEx;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin(AdvancedStatsBase.ModId, AdvancedStatsBase.ModName, AdvancedStatsBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal Harmony Harmony { get; private set; }

        private void Awake()
        {
            Harmony = new Harmony(AdvancedStatsBase.ModId);
            AdvancedStatsBase.Init(this);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            AdvancedStatsBase.OnSceneLoaded(scene.name);
        }
    }
}
#endif
