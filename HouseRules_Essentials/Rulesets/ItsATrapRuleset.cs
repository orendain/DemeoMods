﻿namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class ItsATrapRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "It's A Trap!";
            const string description = "Everything you need to build devious traps for your enemies, but try not to kill your friends.";

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, IsReplenishable = false },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, IsReplenishable = false },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, IsReplenishable = false },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, IsReplenishable = false },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, IsReplenishable = false },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.PoisonBomb,
                        AbilityKey.MagicBarrier,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.TheBehemoth,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.TheBehemoth,
                        AbilityKey.VortexLamp,
                        AbilityKey.HealingPotion,
                        AbilityKey.Torch,
                        AbilityKey.Sneak,
                        AbilityKey.Bone,
                        AbilityKey.DetectEnemies,
                    }
                },
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.PoisonBomb,
                        AbilityKey.MagicBarrier,
                        AbilityKey.Bone,
                        AbilityKey.HealingPotion,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.VortexLamp,
                        AbilityKey.RevealPath,
                        AbilityKey.Torch,
                        AbilityKey.Sneak,
                        AbilityKey.DetectEnemies,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.PoisonBomb,
                        AbilityKey.MagicBarrier,
                        AbilityKey.HealingPotion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Torch,
                        AbilityKey.Sneak,
                        AbilityKey.DetectEnemies,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.PoisonBomb,
                        AbilityKey.HealingPotion,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Vortex,
                        AbilityKey.Torch,
                        AbilityKey.Bone,
                        AbilityKey.DetectEnemies,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.HealingPotion,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Vortex,
                        AbilityKey.Torch,
                        AbilityKey.Sneak,
                        AbilityKey.DetectEnemies,
                    }
                },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Torch, Property = "VisionRange", Value = 40 },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 0 },
                { "FloorOneLootChests", 11 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoLootChests", 14 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreeLootChests", 12 },
                { "FloorOneEndZoneSpikeMaxBudget", 12 },
                { "PacingSpikeSegmentFloorOneBudget", 12 },
            });

            var aoePotions = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.HealingPotion, 1 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.BoobyTrap, false },
            });

            var lampTypesRule = new LampTypesOverriddenRule(new Dictionary<int, List<BoardPieceId>>
            {
                {
                    1, new List<BoardPieceId>
                {
                    BoardPieceId.GasLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.HealingBeacon,
                }
                },
                {
                    2, new List<BoardPieceId>
                {
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.HealingBeacon,
                }
                },
                {
                    3, new List<BoardPieceId>
                {
                    BoardPieceId.OilLamp,
                    BoardPieceId.IceLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.IceLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.HealingBeacon,
                }
                },
            });

            var piecePieceTypeRule = new PiecePieceTypeListOverriddenRule(new Dictionary<BoardPieceId, List<PieceType>>
            {
                { BoardPieceId.Torch, new List<PieceType> { PieceType.Prop, PieceType.UpdateFogOfWar, PieceType.ShowNameplate } },
                { BoardPieceId.EyeOfAvalon, new List<PieceType> { PieceType.Prop, PieceType.UpdateFogOfWar, PieceType.Immovable, PieceType.ShowHealthbar, PieceType.ShowNameplate } },
                { BoardPieceId.HealingBeacon, new List<PieceType> { PieceType.Prop, PieceType.Bot, PieceType.ShowNameplate } },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Stealthed,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
            });

            var tileEffectDuration = new TileEffectDurationOverriddenRule(new Dictionary<Boardgame.Board.TileEffect, int>
            {
                { TileEffect.Gas, 10 },
                { TileEffect.Acid, 1 },
                { TileEffect.Web, 10 },
                { TileEffect.Water, 10 },
                { TileEffect.Target, 0 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                abilityActionCostRule,
                allowedCardsRule,
                aoePotions,
                lampTypesRule,
                levelPropertiesRule,
                piecesAdjustedRule,
                piecePieceTypeRule,
                startingCardsRule,
                statusEffectRule,
                tileEffectDuration,
                new EnemyDoorOpeningDisabledRule(true));
        }
    }
}
