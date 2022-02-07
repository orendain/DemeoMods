namespace HouseRules.Essentials
{
    using System.Collections.Generic;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;
    using MelonLoader;

    internal class EssentialsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Essentials");

        public override void OnApplicationStart()
        {
            RegisterNewRuleTypes();
            RegisterNewRulesets();
        }

        private static void RegisterNewRuleTypes()
        {
            var registrar = HR.Rulebook;
            registrar.Register(typeof(SampleRule));
            registrar.Register(typeof(AbilityAoeAdjustedRule));
            registrar.Register(typeof(AbilityDamageAdjustedRule));
            registrar.Register(typeof(AbilityActionCostAdjustedRule));
            registrar.Register(typeof(ActionPointsAdjustedRule));
            registrar.Register(typeof(CardEnergyFromAttackMultipliedRule));
            registrar.Register(typeof(CardEnergyFromRecyclingMultipliedRule));
            registrar.Register(typeof(CardLimitModifiedRule));
            registrar.Register(typeof(CardSellValueMultipliedRule));
            registrar.Register(typeof(EnemyAttackScaledRule));
            registrar.Register(typeof(EnemyDoorOpeningDisabledRule));
            registrar.Register(typeof(EnemyHealthScaledRule));
            registrar.Register(typeof(EnemyRespawnDisabledRule));
            registrar.Register(typeof(GoldPickedUpMultipliedRule));
            registrar.Register(typeof(GoldPickedUpScaledRule));
            registrar.Register(typeof(LevelPropertiesModifiedRule));
            registrar.Register(typeof(PieceConfigAdjustedRule));
            registrar.Register(typeof(RatNestsSpawnGoldRule));
            registrar.Register(typeof(StartCardsModifiedRule));
            registrar.Register(typeof(StartHealthAdjustedRule));
        }

        private static void RegisterNewRulesets()
        {
            var sampleRules = new List<Rule> { new SampleRule() };
            var sampleRuleset = Ruleset.NewInstance("SampleRuleset", "Just a sample ruleset.", sampleRules);

            var registrar = HR.Rulebook;
            registrar.Register(sampleRuleset);
        }
    }
}
