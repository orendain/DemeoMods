namespace AdvancedStats
{
    using HarmonyLib;
    using MelonLoader;

    internal class AdvancedStatsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("AdvancedStats");

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            var harmony = new Harmony("com.orendain.demeomods.advancedstats");
            if (sceneName == "StartupDesktop")
            {
                NonVRAdvancedStatsView.Patch(harmony);
            }
            else if (sceneName.Contains("Startup"))
            {
                VRAdvancedStatsView.Patch(harmony);
            }
        }
    }
}
