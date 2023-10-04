#if BEPINEX
namespace HouseRules.Core
{
    using BepInEx;
    using BepInEx.Logging;
    using HarmonyLib;

    [BepInPlugin(HouseRulesCoreBase.ModId, HouseRulesCoreBase.ModName, HouseRulesCoreBase.ModVersion)]
    internal class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        internal Harmony? Harmony { get; private set; }

        private void Awake()
        {
            Log = Logger;
            Harmony = new Harmony(HouseRulesCoreBase.ModId);
            HouseRulesCoreBase.Init(this);
        }

        private void Start()
        {
            HouseRulesCoreBase.PatchRegisteredRules();
        }
    }
}
#endif
