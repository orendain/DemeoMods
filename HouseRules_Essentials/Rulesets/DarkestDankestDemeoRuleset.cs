namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DarkestDankestDemeoRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Darkest Dankest Demeo";
            const string description = "Watch your step... Can you defeat the darkness?";
            const string longdesc = "Gain temporary vision range increases by defeating enemies\n";

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Torch, Property = "StartHealth", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinChieftan, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinRanger, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinFighter, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinMadUn, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMystic, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ScabRat, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileArcher, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileMutantWizard, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GeneralRonthian, Property = "WaterTrailChance", Value = 0.5f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenSpearman, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidArcher, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidHoundMaster, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootHound, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinChieftan, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinRanger, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinFighter, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinMadUn, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMystic, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ScabRat, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileArcher, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileMutantWizard, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GeneralRonthian, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenSpearman, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidArcher, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidHoundMaster, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootHound, Property = "WaterTrailChance", Value = 1 },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Torch,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            var tileEffectDuration = new TileEffectDurationOverriddenRule(new Dictionary<TileEffect, int>
            {
                { TileEffect.Gas, 3 },
                { TileEffect.Acid, 15 },
                { TileEffect.Web, 3 },
                { TileEffect.Water, 4 },
                { TileEffect.Corruption, 5 },
                { TileEffect.Target, 0 },
            });

            var darknessRule = new DarknessRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroGuardian, 1 },
                { BoardPieceId.HeroHunter, 1 },
                { BoardPieceId.HeroBard, 1 },
                { BoardPieceId.HeroBarbarian, 1 },
                { BoardPieceId.HeroRogue, 1 },
                { BoardPieceId.HeroWarlock, 1 },
                { BoardPieceId.HeroSorcerer, 1 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                piecesAdjustedRule,
                statusEffectRule,
                tileEffectDuration,
                darknessRule);
        }
    }
}
