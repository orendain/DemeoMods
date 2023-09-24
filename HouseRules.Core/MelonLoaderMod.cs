#if MELONLOADER
using HouseRules.Core;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), HouseRulesCoreCore.ModName, HouseRulesCoreCore.ModVersion, HouseRulesCoreCore.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("574512")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace HouseRules.Core
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HouseRulesCoreCore.Init(this);
        }

        public override void OnLateInitializeMelon()
        {
            HouseRulesCoreCore.PatchRegisteredRules();
        }
    }
}
#endif
