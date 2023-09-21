namespace SkipIntro
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin("com.orendain.demeomods.skipintro", "SkipIntro", "2.0.0")]
    public class SkipIntroMod : BaseUnityPlugin
    {
        internal static ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            var harmony = new Harmony("com.orendain.demeomods.skipintro");
            ModPatcher.Patch(harmony);
        }
    }
}
