#if BEPINEX
namespace SkipIntro
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin(SkipIntroBase.ModId, SkipIntroBase.ModName, SkipIntroBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            SkipIntroBase.Init(this);

            var harmony = new Harmony(SkipIntroBase.ModId);
            ModPatcher.Patch(harmony);
        }
    }
}
#endif
