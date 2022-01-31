namespace Rules
{
    using System.Collections.Generic;
    using MelonLoader;

    internal class RulesMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("Rules");

        public override void OnApplicationStart()
        {
            RegisterNewRuleTypes();
            RegisterNewRulesets();
        }

        private static void RegisterNewRuleTypes()
        {
            var registrar = RulesAPI.Registrar.Instance();
            registrar.Register(typeof(Rule.SampleRule));
            registrar.Register(typeof(Rule.AbilityDamageAdjustedRule));
            registrar.Register(typeof(Rule.AbilityActionCostAdjustedRule));
            registrar.Register(typeof(Rule.ActionPointsAdjustedRule));
            registrar.Register(typeof(Rule.CardEnergyFromAttackMultipliedRule));
            registrar.Register(typeof(Rule.CardEnergyFromRecyclingMultipliedRule));
            registrar.Register(typeof(Rule.CardSellValueMultipliedRule));
            registrar.Register(typeof(Rule.EnemyAttackScaledRule));
            registrar.Register(typeof(Rule.EnemyDoorOpeningDisabledRule));
            registrar.Register(typeof(Rule.EnemyHealthScaledRule));
            registrar.Register(typeof(Rule.EnemyRespawnDisabledRule));
            registrar.Register(typeof(Rule.GoldPickedUpMultipliedRule));
            registrar.Register(typeof(Rule.PieceConfigAdjustedRule));
            registrar.Register(typeof(Rule.RatNestsSpawnGoldRule));
            registrar.Register(typeof(Rule.StartHealthAdjustedRule));
            registrar.Register(typeof(Rule.ZapStartingInventoryAdjustedRule));
        }

        private static void RegisterNewRulesets()
        {
            var sampleRules = new HashSet<RulesAPI.Rule> { new Rule.SampleRule() };
            var sampleRuleset = RulesAPI.Ruleset.NewInstance("SampleRuleset", "Just a sample ruleset.", sampleRules);

            var registrar = RulesAPI.Registrar.Instance();
            registrar.Register(sampleRuleset);
        }
    }
}
