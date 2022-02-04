namespace Rules.Rule
{
    using Data.GameData;
    using HarmonyLib;
    using MelonLoader.TinyJSON;

    public sealed class CardEnergyFromAttackMultipliedRule : RulesAPI.Rule, RulesAPI.IConfigWritable
    {
        public override string Description => "Card energy from attack is multiplied";

        private readonly float _multiplier;
        private readonly float _originalValue;

        public CardEnergyFromAttackMultipliedRule(float multiplier)
        {
            _multiplier = multiplier;
            _originalValue = AIDirectorConfig.CardEnergy_EnergyToGetFromDealingDamage;
        }

        public static CardEnergyFromAttackMultipliedRule FromConfigString(string configString)
        {
            JSON.MakeInto(JSON.Load(configString), out float conf);
            return new CardEnergyFromAttackMultipliedRule(conf);
        }

        public string ToConfigString()
        {
            return JSON.Dump(_multiplier, EncodeOptions.NoTypeHints);
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
