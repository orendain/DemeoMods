﻿namespace Rules.Rule
{
    using System.Reflection;
    using HarmonyLib;

    public sealed class CardSellValueMultipliedRule : RulesAPI.Rule, RulesAPI.IConfigWritable<float>, RulesAPI.IPatchable
    {
        public override string Description => "CardConfig sell values are multiplied";

        private static float _multiplier;
        private static bool _isActivated;

        public CardSellValueMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(CardShopController), "CardShopInfoProvider")
                    .GetTypeInfo()
                    .GetDeclaredMethod("GetCardSellValue"),
                postfix: new HarmonyMethod(
                    typeof(CardSellValueMultipliedRule),
                    nameof(CardShopInfoProvider_GetCardSellValue_Postfix)));
        }

        private static void CardShopInfoProvider_GetCardSellValue_Postfix(ref int __result)
        {
            if (!_isActivated)
            {
                return;
            }

            __result = (int)(__result * _multiplier);
        }
    }
}
