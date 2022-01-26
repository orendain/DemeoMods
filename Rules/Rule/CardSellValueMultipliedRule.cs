namespace Rules.Rule
{
    using System.Reflection;
    using HarmonyLib;

    public sealed class CardSellValueMultipliedRule : RulesAPI.Rule
    {
        public override string Description => "Card sell values are multiplied";

        private static double _multiplier;
        private static bool _isActivated;

        public CardSellValueMultipliedRule(double multiplier)
        {
            _multiplier = multiplier;
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void OnPatch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Inner(typeof(CardShopController), "CardShopInfoProvider").GetTypeInfo()
                    .GetDeclaredMethod("GetCardSellValue"),
                postfix: new HarmonyMethod(typeof(CardSellValueMultipliedRule), nameof(CardShopInfoProvider_GetCardSellValue_Postfix)));
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
