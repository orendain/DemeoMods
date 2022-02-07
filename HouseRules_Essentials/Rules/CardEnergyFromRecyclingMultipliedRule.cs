namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using HarmonyLib;

    public sealed class CardEnergyFromRecyclingMultipliedRule : Rule, IConfigWritable<float>, IPatchable
    {
        public override string Description => "CardConfig energy from recycling is multiplied";

        private static float _multiplier;
        private static bool _isActivated;

        public CardEnergyFromRecyclingMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnActivate(GameContext gameContext) => _isActivated = true;

        protected override void OnDeactivate(GameContext gameContext) => _isActivated = false;

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
