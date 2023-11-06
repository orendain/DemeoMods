﻿#if MELONLOADER
using HouseRules.Core;
using MelonLoader;

[assembly: MelonInfo(
    typeof(MelonLoaderMod),
    HouseRulesCoreBase.ModName,
    BuildVersion.Version,
    HouseRulesCoreBase.ModAuthor,
    "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("574512")]
[assembly: VerifyLoaderVersion("0.5.7", true)]

namespace HouseRules.Core
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            HouseRulesCoreBase.Init(this);
        }

        public override void OnLateInitializeMelon()
        {
            HouseRulesCoreBase.PatchRegisteredRules();
        }
    }
}
#endif
