namespace Rules.Rule
{
    using Boardgame;
    using HarmonyLib;

    public sealed class CardEnergyFromRecyclingMultipliedRule : RulesAPI.Rule, RulesAPI.IConfigWritable<float>, RulesAPI.IPatchable
    {
        public override string Description => "CardConfig energy from recycling is multiplied";

        private static float _multiplier;
        private static bool _isActivated;

        public CardEnergyFromRecyclingMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        private static void Patch(Harmony harmony)
        {
            harmony.Patch(
                original: AccessTools.Method(typeof(CardPowder), "OnTrashedCard"),
                prefix: new HarmonyMethod(
                    typeof(CardEnergyFromRecyclingMultipliedRule),
                    nameof(CardPowder_OnTrashedCard_Prefix)));
        }

        private static void CardPowder_OnTrashedCard_Prefix(ref float cardEnergy)
        {
            if (!_isActivated)
            {
                return;
            }

            cardEnergy *= _multiplier;
        }
    }
}
