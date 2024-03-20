namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Boardgame.PlayerData;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class XpGainDisabledRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Xp gain is disabled";

        private static bool _isActivated;

        public XpGainDisabledRule(bool value)
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
                    typeof(XpGainDisabledRule),
                    nameof(PlayerDataController_GiveExperienceAfterLevel_Prefix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerDataController), "GetSavedExperience"),
                postfix: new HarmonyMethod(
                    typeof(XpGainDisabledRule),
                    nameof(PlayerDataController_GetSavedExperience_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerDataController), "GetHighestUnlockedItemIndex"),
                postfix: new HarmonyMethod(
                    typeof(XpGainDisabledRule),
                    nameof(PlayerDataController_GetHighestUnlockedItemIndex_Postfix)));

            harmony.Patch(
                original: AccessTools.Method(typeof(PlayerDataController), "GetHeroRankByExperience"),
                postfix: new HarmonyMethod(
                    typeof(XpGainDisabledRule),
                    nameof(PlayerDataController_GetHeroRankByExperience_Postfix)));
        }

        private static bool PlayerDataController_GiveExperienceAfterLevel_Prefix()
        {
            if (!_isActivated)
            {
                return true;
            }

            return false; // No xp given
        }

        private static void PlayerDataController_GetSavedExperience_Postfix(ref int __result)
        {
            /*if (!_isActivated)
            {
                return;
            }*/

            __result = 117000; // Rank 69 is 117000 - 119999
            return;
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

            __result = 96; // Why be higher?
            return;
        }
    }
}
