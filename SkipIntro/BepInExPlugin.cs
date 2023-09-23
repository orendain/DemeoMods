#if BEPINEX
namespace SkipIntro
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin(SkipIntro.ModId, SkipIntro.ModName, SkipIntro.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            SkipIntro.Init(this);

            var harmony = new Harmony(SkipIntro.ModId);
            ModPatcher.Patch(harmony);
        }
    }
}
#endif
