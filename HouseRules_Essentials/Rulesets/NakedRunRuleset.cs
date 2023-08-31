namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class NakedRunRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Naked Run";
            const string description = "No Chests, No Healing Fountains, One pile of Gold and only the 1 life...";
            const string longdesc = "By KennGuy and ShaunSheep\n\nYou get no chests, no healing fountains, no potion racks, ONE pile of gold, no starting cards, and only ONE life.\nThe rest of the gameplay and maps are the same.\nYou can get more class cards as the mana pool fills.\nBe afraid. Be resourceful.";

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
            };

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
            };

            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };

            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
            };

            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
            };

            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Overcharge, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
            };

            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
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

            var pieceDownedCountRule = new PieceDownedCountAdjustedRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroBarbarian, 0 },
                { BoardPieceId.HeroGuardian, 0 },
                { BoardPieceId.HeroHunter, 0 },
                { BoardPieceId.HeroRogue, 0 },
                { BoardPieceId.HeroSorcerer, 0 },
                { BoardPieceId.HeroWarlock, 0 },
                { BoardPieceId.HeroBard, 0 },
            });

            var levelSequenceRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneHealingFountains", 0 },
                { "FloorOneLootChests", 0 },
                { "FloorOneGoldMaxAmount", 1 },
                { "FloorTwoPotionStand", 0 },
                { "FloorTwoHealingFountains", 0 },
                { "FloorTwoLootChests", 0 },
                { "FloorTwoGoldMaxAmount", 1 },
                { "FloorThreeHealingFountains", 0 },
                { "FloorThreeLootChests", 0 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                startingCardsRule,
                pieceDownedCountRule,
                levelSequenceRule);
        }
    }
}
