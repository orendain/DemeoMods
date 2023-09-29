#if BEPINEX
namespace HouseRules.Configuration
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin(HouseRulesConfigurationBase.ModId, HouseRulesConfigurationBase.ModName, HouseRulesConfigurationBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            HouseRulesConfigurationBase.Init(this);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Start()
        {
            HouseRulesConfigurationBase.LoadConfiguration();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            HouseRulesConfigurationBase.OnSceneLoaded(scene.buildIndex, scene.name);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            HouseRulesConfigurationBase.OnSceneUnloaded(scene.buildIndex, scene.name);
        }
    }
}
#endif
