#if MELONLOADER
using HouseRules.Configuration;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), HouseRulesConfigurationBase.ModName, HouseRulesConfigurationBase.ModVersion, HouseRulesConfigurationBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
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
            HouseRulesConfigurationBase.Init(this);
        }

        public override void OnLateInitializeMelon()
        {
            HouseRulesConfigurationBase.LoadConfiguration();
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            HouseRulesConfigurationBase.OnSceneLoaded(buildIndex, sceneName);
        }

        public override void OnSceneWasUnloaded(int buildIndex, string sceneName)
        {
            HouseRulesConfigurationBase.OnSceneUnloaded(buildIndex, sceneName);
        }
    }
}
#endif
