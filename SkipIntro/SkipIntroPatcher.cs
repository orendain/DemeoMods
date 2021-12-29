namespace SkipIntro
{
    using Boardgame;
    using HarmonyLib;
    using MelonLoader;
    using UnityEngine;

    internal static class SkipIntroPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: typeof(MotherbrainSceneUtil).GetMethod("LoadIntro"),
                prefix: new HarmonyMethod(typeof(SkipIntroPatcher), nameof(MotherbrainSceneUtil_LoadIntro_Prefix)));
        }

        private static bool MotherbrainSceneUtil_LoadIntro_Prefix(ref AsyncOperation __result)
        {
            MelonLogger.Msg("Skipping the intro scene.");
            __result = MotherbrainSceneUtil.LoadLobby();
            return false;
        }
    }
}
