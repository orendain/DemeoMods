#if BEPINEX
namespace HouseRules.Essentials
{
    using BepInEx;
    using BepInEx.Logging;

    [BepInPlugin(HouseRulesEssentialsBase.ModId, HouseRulesEssentialsBase.ModName, Core.BuildVersion.Version)]
    [BepInDependency("com.orendain.demeomods.houserules.core")]
    public class BepInExPlugin : BaseUnityPlugin
    {
        internal ManualLogSource? Log { get; private set; }

        private void Awake()
        {
            Log = Logger;
            HouseRulesEssentialsBase.Init(this);
        }
    }
}
#endif
