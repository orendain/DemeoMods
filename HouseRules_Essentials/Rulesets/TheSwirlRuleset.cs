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
            const string description = "Only poison, fireballs and vortexes. Health and POIs aplenty, but must defeat all enemies to escape.";

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.StrengthenCourage, IsReplenishable = true },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, IsReplenishable = true },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HunterArrow, IsReplenishable = true },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, IsReplenishable = true },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, IsReplenishable = true },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBard, bardCards }, // tordnater
                { BoardPieceId.HeroGuardian, guardianCards }, /// charge
                { BoardPieceId.HeroHunter, hunterCards }, // naturescall
                { BoardPieceId.HeroRouge, assassinCards }, //blink
                { BoardPieceId.HeroSorcerer, sorcererCards }, // spawn elemtnal
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

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRouge, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 50 },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 2 },
                { "FloorOneLootChests", 11 },
                { "FloorTwoHealingFountains", 4 },
                { "FloorTwoLootChests", 14 },
                { "FloorThreeHealingFountains", 4 },
                { "FloorThreeLootChests", 12 },
            });

            var cardEnergyRule = new CardEnergyFromAttackMultipliedRule(1f);
            var respawnsDisabledRule = new EnemyRespawnDisabledRule(true);
            var levelExitLockedRule = new LevelExitLockedUntilAllEnemiesDefeatedRule(true);

            return Ruleset.NewInstance(
                name,
                description,
                startingCardsRule,
                allowedCardsRule,
                piecesAdjustedRule,
                levelPropertiesRule,
                cardEnergyRule,
                respawnsDisabledRule,
                levelExitLockedRule);
        }
    }
}
