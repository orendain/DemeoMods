namespace Rules.Rule
{
    using System.Reflection;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class CardSellValueMultipliedRule : RulesAPI.Rule, RulesAPI.IConfigWritable, RulesAPI.IPatchable
    {
        public override string Description => "Card sell values are multiplied";

        private static float _multiplier;
        private static bool _isActivated;

        public CardSellValueMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public static CardSellValueMultipliedRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out float conf);
            return new CardSellValueMultipliedRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_multiplier, EncodeOptions.NoTypeHints);
        }

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
