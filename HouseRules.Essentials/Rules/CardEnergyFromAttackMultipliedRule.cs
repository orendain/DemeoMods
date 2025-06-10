namespace HouseRules.Essentials.Rules
{
    using Data.GameData;
    using HarmonyLib;
    using HouseRules.Core.Types;

    public sealed class CardEnergyFromAttackMultipliedRule : Rule, IConfigWritable<float>, IMultiplayerSafe
    {
        public override string Description => "Card energy (mana) given from attacks is adjusted";

        private readonly float _multiplier;
        private readonly float _originalValue;

        public CardEnergyFromAttackMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
            _originalValue = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
        }

        public float GetConfigObject() => _multiplier;

        protected override void OnPostGameCreated(Context context)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue * _multiplier;
        }

        protected override void OnDeactivate(Context context)
        {
            Traverse.Create(typeof(AIDirectorConfig))
                .Field<float>("CardEnergy_EnergyToGetFromDealingDamage").Value = _originalValue;
        }
    }
}
