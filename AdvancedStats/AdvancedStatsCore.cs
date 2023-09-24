﻿namespace AdvancedStats
{
    using System;
    using HarmonyLib;

    internal static class AdvancedStatsCore
    {
        internal const string ModId = "com.orendain.demeomods.advancedstats";
        internal const string ModName = "AdvancedStats";
        internal const string ModVersion = "1.0.0";
        internal const string ModAuthor = "TheGrayAlien";

        private static Harmony _harmony;

        internal static void Init(object loader)
        {
#if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                _harmony = plugin.Harmony;
            }
#endif

#if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                _harmony = mod.HarmonyInstance;
            }
#endif
        }

        internal static void OnSceneLoaded(string sceneName)
        {
            if (sceneName == "StartupDesktop")
            {
                NonVRAdvancedStatsView.Patch(_harmony);
            }
            else if (sceneName.Contains("Startup"))
            {
                VRAdvancedStatsView.Patch(_harmony);
            }
        }
    }
}
