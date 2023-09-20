namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class FlippingOut
    {
        internal static Ruleset Create()
        {
            const string name = "Flipping Out!";
            const string description = "Coin Flips ONLY! BIG ENEMIES! Heads... or tails?";
            const string longdesc = "";

            var entranceDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Cavetroll, 0 },
                { BoardPieceId.Sigataur, 0 },
                { BoardPieceId.GiantSpider, 0 },
                { BoardPieceId.GiantSlime, 0 },
                { BoardPieceId.ElvenMarauder, 0 },
                { BoardPieceId.Brookmare, 0 },
                { BoardPieceId.Gorgon, 0 },
                { BoardPieceId.SilentSentinel, 0 },
            };
            var exitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Cavetroll, 0 },
                { BoardPieceId.Sigataur, 0 },
                { BoardPieceId.GiantSpider, 0 },
                { BoardPieceId.GiantSlime, 0 },
                { BoardPieceId.ElvenMarauder, 0 },
                { BoardPieceId.Brookmare, 0 },
                { BoardPieceId.Gorgon, 0 },
                { BoardPieceId.SilentSentinel, 0 },
            };
            var entranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Cavetroll, 0 },
                { BoardPieceId.Sigataur, 0 },
                { BoardPieceId.GiantSpider, 0 },
                { BoardPieceId.GiantSlime, 0 },
                { BoardPieceId.ElvenMarauder, 0 },
                { BoardPieceId.Brookmare, 0 },
                { BoardPieceId.Gorgon, 0 },
                { BoardPieceId.SilentSentinel, 0 },
            };
            var exitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Cavetroll, 0 },
                { BoardPieceId.Sigataur, 0 },
                { BoardPieceId.GiantSpider, 0 },
                { BoardPieceId.GiantSlime, 0 },
                { BoardPieceId.ElvenMarauder, 0 },
                { BoardPieceId.Brookmare, 0 },
                { BoardPieceId.Gorgon, 0 },
                { BoardPieceId.SilentSentinel, 0 },
            };
            var bossDeck = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Cavetroll, 0 },
                { BoardPieceId.Sigataur, 0 },
                { BoardPieceId.GiantSpider, 0 },
                { BoardPieceId.GiantSlime, 0 },
                { BoardPieceId.ElvenMarauder, 0 },
                { BoardPieceId.Brookmare, 0 },
                { BoardPieceId.Gorgon, 0 },
                { BoardPieceId.SilentSentinel, 0 },
            };
            var monsterDeckConfig = new MonsterDeckOverriddenRule.DeckConfig
            {
                EntranceDeckFloor1 = entranceDeckFloor1,
                ExitDeckFloor1 = exitDeckFloor1,
                EntranceDeckFloor2 = entranceDeckFloor2,
                ExitDeckFloor2 = exitDeckFloor2,
                BossDeck = bossDeck,
                KeyHolderFloor1 = BoardPieceId.Mimic,
                KeyHolderFloor2 = BoardPieceId.EarthElemental,
                Boss = BoardPieceId.Wyvern,
            };
            var monsterDeckRule = new MonsterDeckOverriddenRule(monsterDeckConfig);

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingSmash, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
            };

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
            };

            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };

            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
            };

            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Rejuvenation, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
            };

            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Freeze, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
            };

            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CoinFlip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implode, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MissileSwarm, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
            };

            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBarbarian, barbarianCards },
                { BoardPieceId.HeroWarlock, warlockCards },
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
            });

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Cavetroll, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.Mimic, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.Sigataur, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.GiantSlime, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.ElvenMarauder, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.GiantSpider, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.Brookmare, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.Gorgon, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.SilentSentinel, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.EnemyMelee } },
                { BoardPieceId.Wyvern, new List<AbilityKey> { AbilityKey.EnemyMelee } },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartArmor", Value = 0 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Cavetroll, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Sigataur, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSlime, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMarauder, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSpider, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Gorgon, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Cavetroll, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Sigataur, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSlime, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMarauder, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSpider, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Gorgon, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "AttackDamage", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "StartHealth", Value = 130 },
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.CoinFlip,
                    }
                },
            });

            var enemyRespawnDisabledRule = new EnemyRespawnDisabledRule(true);
            var enemyHealthScaledRule = new EnemyHealthScaledRule(3.0f);

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 1 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 9 },
                { "FloorOneGoldMaxAmount", 0 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoPotionStand", 0 },
                { "FloorTwoMerchant", 0 },
                { "FloorTwoLootChests", 9 },
                { "FloorTwoGoldMaxAmount", 0 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeElvenSummoners", 0 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                enemyRespawnDisabledRule,
                enemyHealthScaledRule,
                levelPropertiesRule,
                allowedCardsRule,
                piecesAdjustedRule,
                pieceAbilityRule,
                startingCardsRule,
                monsterDeckRule);
        }
    }
}
