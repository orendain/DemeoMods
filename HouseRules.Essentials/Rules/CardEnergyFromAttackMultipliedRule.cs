namespace HouseRules.Essentials.Rules
{
    using Boardgame;
    using Data.GameData;
    using HarmonyLib;
    using HouseRules.Core;
    using HouseRules.Core.Types;

    public sealed class CardEnergyFromAttackMultipliedRule : Rule, IConfigWritable<float>, IMultiplayerSafe
    {
        public override string Description => "Card energy (mana) gained from attacks is adjusted";

        private readonly float _multiplier;
        private readonly float _originalValue;
        private readonly float _directKill;
        private readonly float _indirectKill;

        public CardEnergyFromAttackMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
            _originalValue = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
            _directKill = AIDirectorConfig.CardEnergy_EnergyToGetFromDirectKill;
            _indirectKill = AIDirectorConfig.CardEnergy_EnergyToGetFromIndirectKill;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnPostGameCreated(GameContext gameContext)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue * _multiplier;
            if (HR.SelectedRuleset.Name.Contains("Revolutions"))
            {
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDirectKill").Value = 0.25f;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = 0.5f;
            }
        }

        protected override void OnDeactivate(GameContext gameContext)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue;
            if (HR.SelectedRuleset.Name.Contains("Revolutions"))
            {
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDirectKill").Value = _directKill;
                Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromIndirectKill").Value = _indirectKill;
            }
        }
    }
}
