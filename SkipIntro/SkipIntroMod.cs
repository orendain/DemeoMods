using MelonLoader;

namespace SkipIntro
{
    internal class SkipIntroMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.skipintro");
            SkipIntroPatcher.Patch(harmony);
        }
    }
}