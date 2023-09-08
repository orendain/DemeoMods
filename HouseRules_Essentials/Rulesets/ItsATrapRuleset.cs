namespace HouseRules.Essentials.Rulesets
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
            const string longdesc = "";

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TeleportLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
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
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
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
                        AbilityKey.WebBomb,
                        AbilityKey.VortexLamp,
                        AbilityKey.Teleportation,
                    }
                },
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
                        AbilityKey.WebBomb,
                        AbilityKey.VortexLamp,
                        AbilityKey.Teleportation,
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
                        AbilityKey.Banish,
                        AbilityKey.Lure,
                        AbilityKey.WebBomb,
                        AbilityKey.Teleportation,
                        AbilityKey.Regroup,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.HealingPotion,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Vortex,
                        AbilityKey.Torch,
                        AbilityKey.MissileSwarm,
                        AbilityKey.DetectEnemies,
                        AbilityKey.Banish,
                        AbilityKey.Lure,
                        AbilityKey.MinionCharge,
                        AbilityKey.Teleportation,
                        AbilityKey.Regroup,
                        AbilityKey.TeleportLamp,
                    }
                },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Torch, Property = "VisionRange", Value = 40 },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 1 },
                { "FloorOneLootChests", 9 },
                { "FloorOnePotionStand", 2 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoLootChests", 11 },
                { "FloorTwoPotionStand", 3 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreeLootChests", 8 },
                { "FloorThreePotionStand", 2 },
                { "FloorOneEndZoneSpikeMaxBudget", 12 },
                { "PacingSpikeSegmentFloorOneBudget", 12 },
            });

            var aoePotions = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
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
                    BoardPieceId.WaterLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.WaterLamp,
                    BoardPieceId.GasLamp,
                }
                },
                {
                    2, new List<BoardPieceId>
                {
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.WaterLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.GasLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.OilLamp,
                }
                },
                {
                    3, new List<BoardPieceId>
                {
                    BoardPieceId.OilLamp,
                    BoardPieceId.IceLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.OilLamp,
                    BoardPieceId.WaterLamp,
                    BoardPieceId.VortexLamp,
                    BoardPieceId.IceLamp,
                }
                },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Stealthed,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Courageous,
                    durationTurns = 2,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    applyAfterDissipate = EffectStateType.Diseased,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Fearless,
                    durationTurns = 2,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    applyAfterDissipate = EffectStateType.Diseased,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Heroic,
                    durationTurns = 2,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    applyAfterDissipate = EffectStateType.Diseased,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
            });

            var tileEffectDuration = new TileEffectDurationOverriddenRule(new Dictionary<TileEffect, int>
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
                longdesc,
                abilityActionCostRule,
                allowedCardsRule,
                aoePotions,
                lampTypesRule,
                levelPropertiesRule,
                piecesAdjustedRule,
                startingCardsRule,
                statusEffectRule,
                tileEffectDuration,
                new EnemyDoorOpeningDisabledRule(true));
        }
    }
}
