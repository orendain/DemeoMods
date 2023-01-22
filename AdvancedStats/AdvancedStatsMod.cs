namespace AdvancedStats
{
    using HarmonyLib;
    using MelonLoader;

    internal class AdvancedStatsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("AdvancedStats");

        [System.Obsolete]
        public override void OnApplicationStart()
        {
            var harmony = new Harmony("com.orendain.demeomods.advancedstats");
            if (MotherbrainGlobalVars.IsRunningOnDesktop)
            {
                NonVRAdvancedStatsView.Patch(harmony);
            }
            else
            {
                VRAdvancedStatsView.Patch(harmony);
            }
        }
    }
}
