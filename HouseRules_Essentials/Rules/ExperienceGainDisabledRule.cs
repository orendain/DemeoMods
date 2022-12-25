namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.PlayerData;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class ExperienceGainDisabledRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Experience gain is disabled";

        private static bool _isActivated;

        public ExperienceGainDisabledRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext)
        {
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerDataController), "GiveExperienceAfterLevel"),
                prefix: new HarmonyMethod(
                    typeof(ExperienceGainDisabledRule),
                    nameof(PlayerDataController_GiveExperienceAfterLevel_Prefix)));
        }

        private static bool PlayerDataController_GiveExperienceAfterLevel_Prefix()
        {
            if (!_isActivated)
            {
                return true;
            }

            return false;
        }
    }
}
