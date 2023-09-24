#if MELONLOADER
using HouseRules.Essentials;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), HouseRulesEssentialsCore.ModName, HouseRulesEssentialsCore.ModVersion, HouseRulesEssentialsCore.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("574512")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace HouseRules.Essentials
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HouseRulesEssentialsCore.Init(this);
            HouseRulesEssentialsCore.RegisterRuleTypes();
            HouseRulesEssentialsCore.RegisterRulesets();
        }
    }
}
#endif
