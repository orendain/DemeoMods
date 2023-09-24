#if BEPINEX
namespace HouseRules.Essentials
{
    using BepInEx;
    using BepInEx.Logging;

    [BepInPlugin(HouseRulesEssentialsCore.ModId, HouseRulesEssentialsCore.ModName, HouseRulesEssentialsCore.ModVersion)]
    public class BepInExPlugin : BaseUnityPlugin
    {

        internal ManualLogSource Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            HouseRulesEssentialsCore.Init(this);
            HouseRulesEssentialsCore.RegisterRuleTypes();
            HouseRulesEssentialsCore.RegisterRulesets();
        }
    }
}
#endif
