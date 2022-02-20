namespace HouseRules.Essentials
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Essentials.Rulesets;
    using MelonLoader;

    internal class EssentialsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Essentials");

        public override void OnApplicationStart()
        {
            RegisterRuleTypes();
            RegisterRulesets();
        }

        private static void RegisterRuleTypes()
        {
            HR.Rulebook.Register(typeof(AbilityAoeAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityDamageAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityActionCostAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityRandomPieceListRule));
            HR.Rulebook.Register(typeof(CardAdditionOverriddenRule));
            HR.Rulebook.Register(typeof(CardEnergyFromAttackMultipliedRule));
            HR.Rulebook.Register(typeof(CardEnergyFromRecyclingMultipliedRule));
            HR.Rulebook.Register(typeof(CardLimitModifiedRule));
            HR.Rulebook.Register(typeof(CardSellValueMultipliedRule));
            HR.Rulebook.Register(typeof(EnemyAttackScaledRule));
            HR.Rulebook.Register(typeof(EnemyDoorOpeningDisabledRule));
            HR.Rulebook.Register(typeof(EnemyHealthScaledRule));
            HR.Rulebook.Register(typeof(EnemyRespawnDisabledRule));
            HR.Rulebook.Register(typeof(GoldPickedUpMultipliedRule));
            HR.Rulebook.Register(typeof(LevelExitLockedUntilAllEnemiesDefeatedRule));
            HR.Rulebook.Register(typeof(LevelPropertiesModifiedRule));
            HR.Rulebook.Register(typeof(PetsFocusHunterMarkRule));
            HR.Rulebook.Register(typeof(PieceConfigAdjustedRule));
            HR.Rulebook.Register(typeof(PieceImmunityListAdjustedRule));
            HR.Rulebook.Register(typeof(PieceAbilityListOverriddenRule));
            HR.Rulebook.Register(typeof(PieceBehavioursListOverriddenRule));
            HR.Rulebook.Register(typeof(PiecePieceTypeListOverriddenRule));
            HR.Rulebook.Register(typeof(RatNestsSpawnGoldRule));
            HR.Rulebook.Register(typeof(RoundCountLimitedRule));
            HR.Rulebook.Register(typeof(StartCardsModifiedRule));
            HR.Rulebook.Register(typeof(StatModifiersOverridenRule));
            HR.Rulebook.Register(typeof(StatusEffectConfigRule));
        }

        private static void RegisterRulesets()
        {
            HR.Rulebook.Register(TheSwirlRuleset.Create());
            HR.Rulebook.Register(BeatTheClockRuleset.Create());
            HR.Rulebook.Register(HuntersParadiseRuleset.Create());
            HR.Rulebook.Register(DifficultyEasyRuleset.Create());
            HR.Rulebook.Register(DifficultyHardRuleset.Create());
            HR.Rulebook.Register(DifficultyLegendaryRuleset.Create());
            HR.Rulebook.Register(NoSurprisesRuleset.Create());
            HR.Rulebook.Register(QuickandDeadRuleset.Create());
        }
    }
}
