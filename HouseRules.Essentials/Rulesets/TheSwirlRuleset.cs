namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class TheSwirlRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "The Swirl";
            const string description = "Only poison, fireballs and vortexes. Health and POIs aplenty, but must defeat all enemies to escape.";
            const string longdesc = "";

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.StrengthenCourage, ReplenishFrequency = 1 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HunterArrow, ReplenishFrequency = 1 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 1 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implosion, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBarbarian, barbarianCards },
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
                { BoardPieceId.HeroWarlock, warlockCards },
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBarbarian, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.GrapplingPush } },
                { BoardPieceId.HeroBard, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.Tornado } },
                { BoardPieceId.HeroGuardian, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.Charge } },
                { BoardPieceId.HeroHunter, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.PoisonedTip } },
                { BoardPieceId.HeroRogue, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.Blink } },
                { BoardPieceId.HeroSorcerer, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.SummonElemental } },
                { BoardPieceId.HeroWarlock, new List<AbilityKey> { AbilityKey.PoisonGasGrenade, AbilityKey.Fireball, AbilityKey.MissileSwarm } },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 50 },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 2 },
                { "FloorOneLootChests", 10 },
                { "FloorOnePotionStand", 1 },
                { "FloorTwoHealingFountains", 4 },
                { "FloorTwoLootChests", 12 },
                { "FloorTwoPotionStand", 2 },
                { "FloorThreeHealingFountains", 4 },
                { "FloorThreeLootChests", 10 },
                { "FloorThreePotionStand", 2 },
            });

            var aoePotions = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.Strength, 1 },
                { AbilityKey.Speed, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
                { AbilityKey.FreeAP, 1 },
            });

            var respawnsDisabledRule = new EnemyRespawnDisabledRule(true);
            var levelExitLockedRule = new LevelExitLockedUntilAllEnemiesDefeatedRule(true);

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                startingCardsRule,
                allowedCardsRule,
                piecesAdjustedRule,
                levelPropertiesRule,
                aoePotions,
                respawnsDisabledRule,
                levelExitLockedRule);
        }
    }
}
