namespace HouseRules.Essentials.Rules
{
    using System.Collections.Generic;
    using Boardgame;
    using HarmonyLib;
    using HouseRules.Types;
    using UnityEngine;

    public sealed class CardShopConfigurationOverriddenRule : Rule, IConfigWritable<bool>, IPatchable, IMultiplayerSafe
    {
        public override string Description => "Card Shop Configuraiton is overridden";

        private static bool _isActivated;

        public CardShopConfigurationOverriddenRule(bool value)
        {
        }

        public bool GetConfigObject() => true;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(CardShopConfigurationContainer), "GetCardShopConfiguration"),
                prefix: new HarmonyMethod(
                    typeof(CardShopConfigurationOverriddenRule),
                    nameof(CardShopConfigurationContainer_GetCardShopConfiguration_Prefix)));
        }

        private static bool CardShopConfigurationContainer_GetCardShopConfiguration_Prefix(CardShopConfiguration __result)
        {
            if (!_isActivated)
            {
                return true;
            }

            __result = ScriptableObject.CreateInstance<CardShopConfiguration>();
            __result.columnSpacing = 1.8f;
            __result.cardsPerRow = 10;
            __result.sellCardSlot = 9;
            __result.insertTempColumnAt = 5;
            __result.cards = new CardShopConfiguration.CardSlot[]
                {
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Potion,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.Class,
                    CardShopConfiguration.CardSlot.HealthPotion,
                    CardShopConfiguration.CardSlot.Antidote,
                    CardShopConfiguration.CardSlot.Torch,
                };
            EssentialsMod.Logger.Msg($"CSC got called");

            return false;
        }
    }
}
