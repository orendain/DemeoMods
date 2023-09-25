#if BEPINEX
namespace HouseRules.Core
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin(HouseRulesCoreCore.ModId, HouseRulesCoreCore.ModName, HouseRulesCoreCore.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource Log { get; private set; }

        internal Harmony Harmony { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony = new Harmony(HouseRulesCoreCore.ModId);
            HouseRulesCoreCore.Init(this);
        }

        private void Start()
        {
            HouseRulesCoreCore.PatchRegisteredRules();
        }
    }
}
#endif
