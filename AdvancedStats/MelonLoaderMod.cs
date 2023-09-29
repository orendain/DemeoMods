#if MELONLOADER
using AdvancedStats;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), AdvancedStatsBase.ModName, AdvancedStatsBase.ModVersion, AdvancedStatsBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: VerifyLoaderVersion("0.5.7", true)]

namespace AdvancedStats
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            AdvancedStatsBase.Init(this);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            AdvancedStatsBase.OnSceneLoaded(sceneName);
        }
    }
}
#endif
