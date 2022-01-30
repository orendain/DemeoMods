namespace Rules.Rule
{
    using Data.GameData;
    using HarmonyLib;

    public sealed class CardEnergyFromAttackMultipliedRule : RulesAPI.Rule
    {
        public override string Description => "Card energy from attack is multiplied";

        private readonly float _multiplier;
        private readonly float _originalValue;

        public CardEnergyFromAttackMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
            _originalValue = AIDirectorConfig.CardEnergy_EnergyToGetFromKill;
        }

        protected override void OnPostGameCreated()
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue * _multiplier;
        }

        protected override void OnDeactivate()
        {
            Traverse.Create(typeof(AIDirectorConfig))
                    .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue;
        }
    }
}
