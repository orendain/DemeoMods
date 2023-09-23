#if MELONLOADER
using MelonLoader;
using SkipIntro;

[assembly: MelonInfo(typeof(MelonLoaderMod), SkipIntro.SkipIntroBase.ModName, SkipIntro.SkipIntroBase.ModVersion, SkipIntro.SkipIntroBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: MelonID("566782")]
[assembly: VerifyLoaderVersion("0.5.3", true)]

namespace SkipIntro
{
    using MelonLoader;

    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            SkipIntroBase.Init(this);
            ModPatcher.Patch(HarmonyInstance);
        }
    }
}
#endif
