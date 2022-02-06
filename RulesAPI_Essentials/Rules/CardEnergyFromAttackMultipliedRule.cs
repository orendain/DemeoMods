namespace RulesAPI.Essentials.Rules
{
    using Boardgame;
    using Data.GameData;
    using HarmonyLib;

    public sealed class CardEnergyFromAttackMultipliedRule : Rule, IConfigWritable<float>
    {
        public override string Description => "CardConfig energy from attack is multiplied";

        private readonly float _multiplier;
        private float _originalValue;

        public CardEnergyFromAttackMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            _originalValue = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue * _multiplier;
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue;
        }
    }
}
