#if MELONLOADER
using FreeCamera;
using MelonLoader;

[assembly: MelonInfo(typeof(MelonLoaderMod), FreeCameraBase.ModName, FreeCameraBase.ModVersion, FreeCameraBase.ModAuthor, "https://github.com/orendain/DemeoMods")]
[assembly: MelonGame("Resolution Games", "Demeo")]
[assembly: MelonGame("Resolution Games", "Demeo PC Edition")]
[assembly: VerifyLoaderVersion("0.5.7")]

namespace FreeCamera
{
    internal class MelonLoaderMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            FreeCameraBase.Init(this);
        }
    }
}
#endif
