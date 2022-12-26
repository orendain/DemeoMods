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

            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerDataController), "GetHighestUnlockedItemIndex"),
                postfix: new HarmonyMethod(
                    typeof(ExperienceGainDisabledRule),
                    nameof(PlayerDataController_GetHighestUnlockedItemIndex_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerDataController), "GetHeroRankByExperience"),
                postfix: new HarmonyMethod(
                    typeof(ExperienceGainDisabledRule),
                    nameof(PlayerDataController_GetHeroRankByExperience_Postfix)));
        }

        private static bool PlayerDataController_GiveExperienceAfterLevel_Prefix()
        {
            /*if (!_isActivated)
            {
                return true;
            }*/

            // 117000 - 119999 experience is Rank 69
            return false;
        }

        private static void PlayerDataController_GetHighestUnlockedItemIndex_Postfix(ref int __result)
        {
            /*if (!_isActivated)
            {
                return;
            }*/

            __result = 74; // For some reason equals highest Rank minus one...
            return;
        }

        private static void PlayerDataController_GetHeroRankByExperience_Postfix(ref int __result)
        {
            /*if (!_isActivated)
            {
                return;
            }*/

            __result = 69;
            return;
        }
    }
}
