namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class TheSwirlRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "The Swirl";
            const string description = "Ultra health. Ultra card recycling. Only 15 rounds to escape...";

            var startingCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, IsReplenishable = true },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBard, startingCards },
                { BoardPieceId.HeroGuardian, startingCards },
                { BoardPieceId.HeroHunter, startingCards },
                { BoardPieceId.HeroRouge, startingCards },
                { BoardPieceId.HeroSorcerer, startingCards },
            });

            var allowedCards = new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball };
            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBard, allowedCards },
                { BoardPieceId.HeroGuardian, allowedCards },
                { BoardPieceId.HeroHunter, allowedCards },
                { BoardPieceId.HeroRouge, allowedCards },
                { BoardPieceId.HeroSorcerer, allowedCards },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<List<string>>
            {
                new List<string> { "HeroBard", "StartHealth", "50" },
                new List<string> { "HeroGuardian", "StartHealth", "50" },
                new List<string> { "HeroHunter", "StartHealth", "50" },
                new List<string> { "HeroRouge", "StartHealth", "50" },
                new List<string> { "HeroSorcerer", "StartHealth", "50" },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 9 },
                { "FloorOneLootChests", 9 },
                { "FloorTwoHealingFountains", 9 },
                { "FloorTwoLootChests", 9 },
                { "FloorThreeHealingFountains", 9 },
                { "FloorThreeLootChests", 9 },
            });

            var respawnsDisabledRule = new EnemyRespawnDisabledRule(true);
            var levelExitLockedRule = new LevelExitLockedUntilAllEnemiesDefeatedRule(true);

            return Ruleset.NewInstance(
                name,
                description,
                startingCardsRule,
                allowedCardsRule,
                piecesAdjustedRule,
                levelPropertiesRule,
                respawnsDisabledRule,
                levelExitLockedRule);
        }
    }
}
