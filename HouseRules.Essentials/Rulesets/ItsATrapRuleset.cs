namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class ItsATrapRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "It's A Trap!";
            const string description = "Everything you need to build devious traps for your enemies, but try not to kill your friends.";
            const string longdesc = "";

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.StrengthenCourage, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HunterArrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ProximityMine, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GasLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TeleportLamp, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectStealthedUnits, ReplenishFrequency = 0 },
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
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.Heal,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Implosion,
                        AbilityKey.TorchLight,
                        AbilityKey.Bone,
                        AbilityKey.DetectStealthedUnits,
                        AbilityKey.WebBomb,
                        AbilityKey.VortexLamp,
                        AbilityKey.Teleport,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.MagicWall,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.BeaconOfSmite,
                        AbilityKey.SwordOfAvalon,
                        AbilityKey.BeaconOfSmite,
                        AbilityKey.VortexLamp,
                        AbilityKey.Heal,
                        AbilityKey.TorchLight,
                        AbilityKey.Stealth,
                        AbilityKey.Bone,
                        AbilityKey.DetectStealthedUnits,
                    }
                },
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.MagicWall,
                        AbilityKey.Bone,
                        AbilityKey.Heal,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.VortexLamp,
                        AbilityKey.RevealPath,
                        AbilityKey.TorchLight,
                        AbilityKey.Stealth,
                        AbilityKey.DetectStealthedUnits,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.MagicWall,
                        AbilityKey.Heal,
                        AbilityKey.PoisonedTip,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.TorchLight,
                        AbilityKey.Stealth,
                        AbilityKey.DetectStealthedUnits,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.Heal,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Implosion,
                        AbilityKey.TorchLight,
                        AbilityKey.Bone,
                        AbilityKey.DetectStealthedUnits,
                        AbilityKey.WebBomb,
                        AbilityKey.VortexLamp,
                        AbilityKey.Teleport,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.Heal,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Implosion,
                        AbilityKey.TorchLight,
                        AbilityKey.Stealth,
                        AbilityKey.DetectStealthedUnits,
                        AbilityKey.Banish,
                        AbilityKey.MonsterBait,
                        AbilityKey.WebBomb,
                        AbilityKey.Teleport,
                        AbilityKey.Regroup,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.Heal,
                        AbilityKey.OilLamp,
                        AbilityKey.GasLamp,
                        AbilityKey.IceLamp,
                        AbilityKey.Implosion,
                        AbilityKey.TorchLight,
                        AbilityKey.MissileSwarm,
                        AbilityKey.DetectStealthedUnits,
                        AbilityKey.Banish,
                        AbilityKey.MonsterBait,
                        AbilityKey.MinionCharge,
                        AbilityKey.Teleport,
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
                { AbilityKey.Strength, 1 },
                { AbilityKey.Speed, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
                { AbilityKey.Heal, 1 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.ProximityMine, false },
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
