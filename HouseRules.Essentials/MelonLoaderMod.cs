#if MELONLOADER
using HouseRules.Essentials;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), HouseRulesEssentialsBase.ModName, HouseRulesEssentialsBase.ModVersion, HouseRulesEssentialsBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("574512")]
[assembly: VerifyLoaderVersion("0.5.7", true)]

namespace HouseRules.Essentials
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HouseRulesEssentialsBase.Init(this);
        }
    }
}
#endif
