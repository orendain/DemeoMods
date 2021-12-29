namespace SkipIntro
{
    using Boardgame;
    using HarmonyLib;
    using UnityEngine;

    internal class ModPatcher
    {
        internal static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: typeof(MotherbrainSceneUtil).GetMethod("LoadIntro"),
                prefix: new HarmonyMethod(typeof(ModPatcher), nameof(MotherbrainSceneUtil_LoadIntro_Prefix)));
        }

        private static bool MotherbrainSceneUtil_LoadIntro_Prefix(ref AsyncOperation __result)
        {
            SkipIntroMod.Logger.Msg("Skipping the intro scene.");
            __result = MotherbrainSceneUtil.LoadLobby();
            return false;
        }
    }
}
