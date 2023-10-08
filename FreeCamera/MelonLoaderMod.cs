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
            LoadConfiguration();
            FreeCameraBase.Init(this);
        }

        private void LoadConfiguration()
        {
            var configCategory = MelonPreferences.CreateCategory("FreeCamera");
            var sensitivity = configCategory.CreateEntry("sensitivity", 12.0);
            NonVrMouseSupporter.InterpolationScaler = (float)sensitivity.Value;
        }
    }
}
#endif
