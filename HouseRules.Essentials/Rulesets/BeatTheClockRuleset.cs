namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class BeatTheClockRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Beat The Clock!";
            const string description = "Ultra health. Ultra card recycling. Only 15 rounds to escape...";
            const string longdesc = "";

            var healthRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 200 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "StartHealth", Value = 100 },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Venom,
                    durationTurns = 15,
                    damagePerTurn = 0,
                    killOnExpire = true,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            var recyclingRule = new CardEnergyFromRecyclingMultipliedRule(3);
            var roundLimitRule = new RoundCountLimitedRule(15);
            var levelRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneLootChests", 12 },
                { "FloorOnePotionStand", 3 },
                { "FloorTwoLootChests", 12 },
                { "FloorTwoPotionStand", 3 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                healthRule,
                statusEffectRule,
                recyclingRule,
                roundLimitRule,
                levelRule);
        }
    }
}
