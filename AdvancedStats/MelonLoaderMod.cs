#if MELONLOADER
using AdvancedStats;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), AdvancedStatsCore.ModName, AdvancedStatsCore.ModVersion, AdvancedStatsCore.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: VerifyLoaderVersion("0.5.7", true)]

namespace AdvancedStats
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            AdvancedStatsCore.Init(this);
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            AdvancedStatsCore.OnSceneLoaded(sceneName);
        }
    }
}
#endif
