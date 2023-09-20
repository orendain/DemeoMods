namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class CardEnergyFromRecyclingMultipliedRule : Rule, IConfigWritable<float>, IPatchable,
        IMultiplayerSafe
    {
        public override string Description => "Card energy (mana) gained from recycling is adjusted";

        private static float _globalMultiplier;
        private static bool _isActivated;

        private readonly float _multiplier;

        public CardEnergyFromRecyclingMultipliedRule(float multiplier)
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

            cardEnergy *= _globalMultiplier;
        }
    }
}
