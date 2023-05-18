namespace SkipIntro
{
    using MelonLoader;

    internal class SkipIntroMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("SkipIntro");

        public override void OnInitializeMelon()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.skipintro");
            ModPatcher.Patch(harmony);
        }
    }
}
