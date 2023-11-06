#if BEPINEX
namespace FreeCamera
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin(FreeCameraBase.ModId, FreeCameraBase.ModName, FreeCameraBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        internal Harmony? Harmony { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony = new Harmony(FreeCameraBase.ModId);
            FreeCameraBase.Init(this);
        }
    }
}
#endif
