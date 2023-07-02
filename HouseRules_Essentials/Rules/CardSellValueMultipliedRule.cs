namespace HouseRules.Essentials.Rules
{
    using System.Reflection;
    using Boardgame;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardSellValueMultipliedRule : Rule, IConfigWritable<float>, IPatchable
    {
        public override string Description => "Card sell values are adjusted <Skirmish Only>";

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public CardSellValueMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate(GameContext gameContext)
        {
            _globalMultiplier = _multiplier;
            _isActivated = true;
        }

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools
                    .Inner(typeof(CardShopController), "CardShopInfoProvider").GetTypeInfo()
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

            __result = (int)(__result * _globalMultiplier);
        }
    }
}
