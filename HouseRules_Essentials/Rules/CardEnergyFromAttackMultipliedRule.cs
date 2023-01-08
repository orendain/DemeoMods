namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Data.GameData;
    using HarmonyLib;
    using HouseRules.Types;

    public sealed class CardEnergyFromAttackMultipliedRule : Rule, IConfigWritable<float>, IMultiplayerSafe
    {
        public override string Description => "CardConfig energy from attack is multiplied";

        private readonly float _multiplier;
        private readonly float _originalValue;
        private readonly float _minionDamage;
        private readonly float _minionKill;
        private readonly float _directKill;
        private readonly float _indirectKill;

        public CardEnergyFromAttackMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
            _originalValue = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
            _directKill = AIDirectorConfig.CardEnergy_EnergyToGetFromDirectKill;
            _indirectKill = AIDirectorConfig.CardEnergy_EnergyToGetFromIndirectKill;
            _minionDamage = AIDirectorConfig.CardEnergy_EnergyForMinionWhenDealingDamage;
            _minionKill = AIDirectorConfig.CardEnergy_EnergyForMinionWhenKillingPiece;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue * _multiplier;
            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDirectKill").Value = 0.25f;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = 0.5f;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyForMinionWhenDealingDamage").Value = _minionDamage * _multiplier;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyForMinionWhenKillingPiece").Value = _minionKill * _multiplier;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue;
            if (HR.SelectedRuleset.Name.Contains("Demeo Revolutions"))
            {
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDirectKill").Value = _directKill;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = _indirectKill;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyForMinionWhenDealingDamage").Value = _minionDamage;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyForMinionWhenKillingPiece").Value = _minionKill;
            }
        }
    }
}
