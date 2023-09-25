#if MELONLOADER
using HouseRules.Configuration;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), HouseRulesConfigurationCore.ModName, HouseRulesConfigurationCore.ModVersion, HouseRulesConfigurationCore.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("574512")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace HouseRules.Configuration
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HouseRulesConfigurationCore.Init(this);
        }

        public override void OnLateInitializeMelon()
        {
            HouseRulesConfigurationCore.LoadConfiguration();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            HouseRulesConfigurationCore.OnSceneLoaded(buildIndex, sceneName);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            HouseRulesConfigurationCore.OnSceneUnloaded(buildIndex, sceneName);
        }
    }
}
#endif
