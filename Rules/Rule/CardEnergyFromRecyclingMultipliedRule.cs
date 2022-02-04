namespace Rules.Rule
{
    using Boardgame;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class CardEnergyFromRecyclingMultipliedRule : RulesAPI.Rule, RulesAPI.IPatchable
    {
        public override string Description => "Card energy from recycling is multiplied";

        private static bool _isActivated;

        private static float _multiplier;

        public CardEnergyFromRecyclingMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public static CardEnergyFromRecyclingMultipliedRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out float conf);
            return new CardEnergyFromRecyclingMultipliedRule(conf);
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
