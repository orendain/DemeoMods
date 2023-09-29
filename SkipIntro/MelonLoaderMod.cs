#if MELONLOADER
using MelonLoader;
using SkipIntro;

<<<<<<< HEAD
[assembly: MelonInfo(typeof(MelonLoaderMod), SkipIntro.SkipIntro.ModName, SkipIntro.SkipIntro.ModVersion, SkipIntro.SkipIntro.ModAuthor, "https://github.com/orendain/DemeoMods")]
=======
[assembly: MelonInfo(typeof(MelonLoaderMod), SkipIntroBase.ModName, SkipIntroBase.ModVersion, SkipIntroBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
>>>>>>> main
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
<<<<<<< HEAD
            SkipIntro.Init(this);
=======
            SkipIntroBase.Init(this);
>>>>>>> main
            ModPatcher.Patch(HarmonyInstance);
        }
    }
}
#endif
