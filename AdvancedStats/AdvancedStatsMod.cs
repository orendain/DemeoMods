namespace AdvancedStats
{
    using MelonLoader;

    internal class AdvancedStatsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("AdvancedStats");

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (sceneName == "StartupDesktop")
            {
                NonVRAdvancedStatsView.Patch(HarmonyInstance);
            }
            else if (sceneName.Contains("Startup"))
            {
                VRAdvancedStatsView.Patch(HarmonyInstance);
            }
        }
    }
}
