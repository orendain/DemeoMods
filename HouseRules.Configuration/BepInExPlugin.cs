#if BEPINEX
namespace HouseRules.Configuration
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;
    using UnityEngine.SceneManagement;

    [BepInPlugin(HouseRulesConfigurationCore.ModId, HouseRulesConfigurationCore.ModName, HouseRulesConfigurationCore.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            HouseRulesConfigurationCore.Init(this);

            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private void Start()
        {
            HouseRulesConfigurationCore.LoadConfiguration();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            HouseRulesConfigurationCore.OnSceneLoaded(scene.buildIndex, scene.name);
        }

        private void OnSceneUnloaded(Scene scene)
        {
            HouseRulesConfigurationCore.OnSceneUnloaded(scene.buildIndex, scene.name);
        }
    }
}
#endif
