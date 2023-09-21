namespace AdvancedStats
{
    using BepInEx;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin("com.orendain.demeomods.advancedstats", "AdvancedStats", "2.0.0")]
    public class AdvancedStatsMod : BaseUnityPlugin
    {
        private Harmony _harmony;

        private void Awake()
        {
            _harmony = new Harmony("com.orendain.demeomods.advancedstats");
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var sceneName = scene.name;

            if (sceneName == "StartupDesktop")
            {
                NonVRAdvancedStatsView.Patch(_harmony);
            }
            else if (sceneName.Contains("Startup"))
            {
                VRAdvancedStatsView.Patch(_harmony);
            }
        }
    }
}
