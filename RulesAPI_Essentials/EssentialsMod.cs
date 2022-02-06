namespace RulesAPI.Essentials
{
    using System.Collections.Generic;
    using MelonLoader;

    internal class EssentialsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI:Essentials");

        public override void OnApplicationStart()
        {
            RegisterNewRuleTypes();
            RegisterNewRulesets();
        }

        private static void RegisterNewRuleTypes()
        {
            var registrar = Registrar.Instance();
            registrar.Register(typeof(Rules.SampleRule));
            registrar.Register(typeof(Rules.AbilityAoeAdjustedRule));
            registrar.Register(typeof(Rules.AbilityDamageAdjustedRule));
            registrar.Register(typeof(Rules.AbilityActionCostAdjustedRule));
            registrar.Register(typeof(Rules.ActionPointsAdjustedRule));
            registrar.Register(typeof(Rules.CardEnergyFromAttackMultipliedRule));
            registrar.Register(typeof(Rules.CardEnergyFromRecyclingMultipliedRule));
            registrar.Register(typeof(Rules.CardLimitModifiedRule));
            registrar.Register(typeof(Rules.CardSellValueMultipliedRule));
            registrar.Register(typeof(Rules.EnemyAttackScaledRule));
            registrar.Register(typeof(Rules.EnemyDoorOpeningDisabledRule));
            registrar.Register(typeof(Rules.EnemyHealthScaledRule));
            registrar.Register(typeof(Rules.EnemyRespawnDisabledRule));
            registrar.Register(typeof(Rules.GoldPickedUpMultipliedRule));
            registrar.Register(typeof(Rules.GoldPickedUpScaledRule));
            registrar.Register(typeof(Rules.LevelPropertiesModifiedRule));
            registrar.Register(typeof(Rules.PieceConfigAdjustedRule));
            registrar.Register(typeof(Rules.RatNestsSpawnGoldRule));
            registrar.Register(typeof(Rules.SorcererStartCardsModifiedRule));
            registrar.Register(typeof(Rules.StartCardsModifiedRule));
            registrar.Register(typeof(Rules.StartHealthAdjustedRule));
            registrar.Register(typeof(Rules.ZapStartingInventoryAdjustedRule));
        }

        private static void RegisterNewRulesets()
        {
            var sampleRules = new List<Rule> { new Rules.SampleRule() };
            var sampleRuleset = Ruleset.NewInstance("SampleRuleset", "Just a sample ruleset.", sampleRules);

            var registrar = Registrar.Instance();
            registrar.Register(sampleRuleset);
        }
    }
}
