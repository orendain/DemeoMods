namespace HouseRules.Essentials
{
    using System;
    using HouseRules.Core;
    using HouseRules.Essentials.Rules;
    using HouseRules.Essentials.Rulesets;

    internal static class HouseRulesEssentialsBase
    {
        internal const string ModId = "com.orendain.demeomods.houserules.essentials";
        internal const string ModName = "HouseRules.Essentials";
        internal const string ModAuthor = "DemeoMods Team";

        private static Action<object>? _logDebug;
        private static Action<object>? _logWarning;
        private static Action<object>? _logError;

        internal static void LogDebug(object data) => _logDebug?.Invoke(data);

        internal static void LogWarning(object data) => _logWarning?.Invoke(data);

        internal static void LogError(object data) => _logError?.Invoke(data);

        internal static void Init(object loader)
        {
            #if BEPINEX
            if (loader is BepInExPlugin plugin)
            {
                if (plugin.Log == null)
                {
                    LogError("Logger instance is invalid. Cannot initialize.");
                    return;
                }

                _logDebug = plugin.Log.LogDebug;
                _logWarning = plugin.Log.LogWarning;
                _logError = plugin.Log.LogError;
            }
            #endif

            #if MELONLOADER
            if (loader is MelonLoaderMod mod)
            {
                _logDebug = mod.LoggerInstance.Msg;
                _logWarning = mod.LoggerInstance.Warning;
                _logError = mod.LoggerInstance.Error;
            }
            #endif

            RegisterRuleTypes();
            RegisterRulesets();
        }

        private static void RegisterRuleTypes()
        {
            HR.Rulebook.Register(typeof(AbilityAoeAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityBackstabAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityBreaksStealthAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityDamageAllOverriddenRule));
            HR.Rulebook.Register(typeof(AbilityDamageOverriddenRule));
            HR.Rulebook.Register(typeof(AbilityHealOverriddenRule));
            HR.Rulebook.Register(typeof(AbilityActionCostAdjustedRule));
            HR.Rulebook.Register(typeof(AbilityRandomPieceListRule));
            HR.Rulebook.Register(typeof(AbilityStealthDamageOverriddenRule));
            HR.Rulebook.Register(typeof(AbilityTargetEffectsRule));
            HR.Rulebook.Register(typeof(ApplyEffectOnHitAdjustedRule));
            HR.Rulebook.Register(typeof(BackstabConfigOverriddenRule));
            HR.Rulebook.Register(typeof(CourageShantyAddsHpRule));
            HR.Rulebook.Register(typeof(CardAdditionOverriddenRule));
            HR.Rulebook.Register(typeof(CardChestAdditionOverriddenRule));
            HR.Rulebook.Register(typeof(CardClassRestrictionOverriddenRule));
            HR.Rulebook.Register(typeof(CardEnergyAdditionOverriddenRule));
            HR.Rulebook.Register(typeof(CardEnergyFromAttackMultipliedRule));
            HR.Rulebook.Register(typeof(CardEnergyFromRecyclingMultipliedRule));
            HR.Rulebook.Register(typeof(CardLimitModifiedRule));
            HR.Rulebook.Register(typeof(CardSellValueMultipliedRule));
            HR.Rulebook.Register(typeof(DarknessRule));
            HR.Rulebook.Register(typeof(ElvenQueenBuffsRule));
            HR.Rulebook.Register(typeof(ElvenQueenSuperBuffRule));
            HR.Rulebook.Register(typeof(EnemyAttackScaledRule));
            HR.Rulebook.Register(typeof(EnemyCooldownOverriddenRule));
            HR.Rulebook.Register(typeof(EnemyDoorOpeningDisabledRule));
            HR.Rulebook.Register(typeof(EnemyDoorOpeningEnabledRule));
            HR.Rulebook.Register(typeof(EnemyHealthScaledRule));
            HR.Rulebook.Register(typeof(EnemyRespawnDisabledRule));
            HR.Rulebook.Register(typeof(FreeAbilityOnCritRule));
            HR.Rulebook.Register(typeof(FreeActionPointsOnCritRule));
            HR.Rulebook.Register(typeof(FreeHealOnCritRule));
            HR.Rulebook.Register(typeof(FreeHealOnHitRule));
            HR.Rulebook.Register(typeof(FreeReplenishablesOnCritRule));
            HR.Rulebook.Register(typeof(GoldPickedUpMultipliedRule));
            HR.Rulebook.Register(typeof(GrappleUnhookedRule));
            HR.Rulebook.Register(typeof(LampTypesOverriddenRule));
            HR.Rulebook.Register(typeof(LevelExitLockedUntilAllEnemiesDefeatedRule));
            HR.Rulebook.Register(typeof(LevelExitLockedUntilOneHeroRemainsRule));
            HR.Rulebook.Register(typeof(LevelPropertiesModifiedRule));
            HR.Rulebook.Register(typeof(LevelSequenceOverriddenRule));
            HR.Rulebook.Register(typeof(SmallLevelSequenceOverriddenRule));
            HR.Rulebook.Register(typeof(MerchantOfferRarityOverriddenRule));
            HR.Rulebook.Register(typeof(MonsterDeckOverriddenRule));
            HR.Rulebook.Register(typeof(MyMonsterDeckOverriddenRule));
            HR.Rulebook.Register(typeof(PartyElectricityDamageOverriddenRule));
            HR.Rulebook.Register(typeof(PetsFocusHunterMarkRule));
            HR.Rulebook.Register(typeof(PieceAbilityListOverriddenRule));
            HR.Rulebook.Register(typeof(PieceReplenishAbilityEveryLevelRule));
            HR.Rulebook.Register(typeof(PieceBehavioursListOverriddenRule));
            HR.Rulebook.Register(typeof(PieceConfigAdjustedRule));
            HR.Rulebook.Register(typeof(PieceCounterDamageRule));
            HR.Rulebook.Register(typeof(PieceDamageResistRule));
            HR.Rulebook.Register(typeof(PieceDownedCountAdjustedRule));
            HR.Rulebook.Register(typeof(PieceExtraStatsAdjustedRule));
            HR.Rulebook.Register(typeof(PieceExtraImmunitiesRule));
            HR.Rulebook.Register(typeof(PieceImmunityListAdjustedRule));
            HR.Rulebook.Register(typeof(PieceKeyholderRule));
            HR.Rulebook.Register(typeof(PieceMagicStatAddedRule));
            HR.Rulebook.Register(typeof(PiecePieceTypeListOverriddenRule));
            HR.Rulebook.Register(typeof(PieceProgressRule));
            HR.Rulebook.Register(typeof(PieceProgressLostRule));
            HR.Rulebook.Register(typeof(PieceUseOnCreatedOverriddenRule));
            HR.Rulebook.Register(typeof(PieceUseWhenKilledOverriddenRule));
            HR.Rulebook.Register(typeof(PointGainRule));
            HR.Rulebook.Register(typeof(PotionAdditionOverriddenRule));
            HR.Rulebook.Register(typeof(RatNestsSpawnGoldRule));
            HR.Rulebook.Register(typeof(RegainAbilityIfMaxxedOutOverriddenRule));
            HR.Rulebook.Register(typeof(ReviveRemovesEffectsRule));
            HR.Rulebook.Register(typeof(RevolutionsRule));
            HR.Rulebook.Register(typeof(RoundCountLimitedRule));
            HR.Rulebook.Register(typeof(SpawnCategoryOverriddenRule));
            HR.Rulebook.Register(typeof(StartCardsModifiedRule));
            HR.Rulebook.Register(typeof(StatModifiersOverriddenRule));
            HR.Rulebook.Register(typeof(StatusEffectConfigRule));
            HR.Rulebook.Register(typeof(TickAdjustedRule));
            HR.Rulebook.Register(typeof(TileEffectDurationOverriddenRule));
            HR.Rulebook.Register(typeof(TurnOrderOverriddenRule));
            HR.Rulebook.Register(typeof(TurnOrderRandomizedRule));
        }

        private static void RegisterRulesets()
        {
            HR.Rulebook.Register(DemeoRevolutions.Create());
            HR.Rulebook.Register(DemeoRevolutions_Easy.Create());
            HR.Rulebook.Register(DemeoRevolutions_Hard.Create());
            HR.Rulebook.Register(DemeoRevolutions_Legendary.Create());
            HR.Rulebook.Register(DemeoRevolutions_Progressive.Create());
            HR.Rulebook.Register(DemeoRevolutions_Small_Progressive.Create());
            HR.Rulebook.Register(DemeoRevolutions_Legendary_Progressive.Create());
            HR.Rulebook.Register(FriendlyCompetition.Create());
            HR.Rulebook.Register(DarkestDankestDemeoRuleset.Create());
            HR.Rulebook.Register(HardcoreRuleset.Create());
            HR.Rulebook.Register(Arachnophobia.Create());
            HR.Rulebook.Register(LuckyDip.Create());
            HR.Rulebook.Register(TheSwirlRuleset.Create());
            HR.Rulebook.Register(BeatTheClockRuleset.Create());
            HR.Rulebook.Register(ItsATrapRuleset.Create());
            HR.Rulebook.Register(HuntersParadiseRuleset.Create());
            HR.Rulebook.Register(HoardersRuleset.Create());
            HR.Rulebook.Register(DemeoReloaded.Create());
            HR.Rulebook.Register(FlippingOut.Create());
            HR.Rulebook.Register(DifficultyEasyRuleset.Create());
            HR.Rulebook.Register(DifficultyHardRuleset.Create());
            HR.Rulebook.Register(DifficultyLegendaryRuleset.Create());
            HR.Rulebook.Register(EarthWindAndFire.Create());
            HR.Rulebook.Register(AoePotionsAndBuffsRuleset.Create());
            HR.Rulebook.Register(BetterSorcererRuleset.Create());
            HR.Rulebook.Register(NoSurprisesRuleset.Create());
            HR.Rulebook.Register(QuickAndDeadRuleset.Create());
            HR.Rulebook.Register(PotionCommotionRuleset.Create());
            HR.Rulebook.Register(NakedRunRuleset.Create());
        }
    }
}
