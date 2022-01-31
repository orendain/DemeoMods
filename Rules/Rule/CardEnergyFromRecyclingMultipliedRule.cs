﻿namespace Rules.Rule
{
    using Boardgame;
    using Data.GameData;
    using HarmonyLib;

    public sealed class CardEnergyFromRecyclingMultipliedRule : RulesAPI.Rule
    {
        public override string Description => "Card energy from recycling is multiplied";

        private static bool _isActivated;

        private static float _multiplier;

        public CardEnergyFromRecyclingMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        protected override void OnActivate() => _isActivated = true;

        protected override void OnDeactivate() => _isActivated = false;

        protected override void OnPostGameCreated()
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<int>("CardEnergy_PowerRankRequiredToGiveEnergy").Value = 0;
        }

        private static void OnPatch(Harmony harmony)
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
