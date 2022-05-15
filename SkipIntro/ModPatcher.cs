namespace SkipIntro
{
    using Boardgame;
    using Boardgame.NonVR;
    using Boardgame.NonVR.Ui;
    using HarmonyLib;
    using UnityEngine;

    internal static class ModPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: typeof(MotherbrainSceneUtil).GetMethod("LoadIntro"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(MotherbrainSceneUtil_LoadIntro_Prefix)));

            harmony.Patch(
                original: AccessTools.Constructor(typeof(NonVrBoot)),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(NonVrBoot_Constructor_Prefix)));

            harmony.Patch(
                original: typeof(GlobalCanvasSystem).GetMethod("RunLoading"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(GlobalCanvasSystem_RunLoading_Prefix)));
        }

        private static bool MotherbrainSceneUtil_LoadIntro_Prefix(ref AsyncOperation __result)
        {
            SkipIntroMod.Logger.Msg("Skipping the intro scene.");
            __result = MotherbrainSceneUtil.LoadLobby();
            return false;
        }

        private static void NonVrBoot_Constructor_Prefix()
        {
            MotherbrainGlobalVars.isRunningBrainTests = true;
            SkipIntroMod.Logger.Msg("Temporarily setting a field required to skip the intro scene.");
        }

        private static void GlobalCanvasSystem_RunLoading_Prefix()
        {
            MotherbrainGlobalVars.isRunningBrainTests = false;
            SkipIntroMod.Logger.Msg("Reverting temporarily field setting.");
        }
    }
}
