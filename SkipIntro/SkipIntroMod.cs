namespace SkipIntro
{
    using MelonLoader;

    internal class SkipIntroMod : MelonMod
    {
        public static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("SkipIntro");

        public override void OnApplicationStart()
        {
            var harmony = new HarmonyLib.Harmony("com.orendain.demeomods.skipintro");
            ModPatcher.Patch(harmony);
        }
    }
}
