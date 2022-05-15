namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class LuckyDip
    {
        internal static Ruleset Create()
        {
            const string name = "LuckyDip";
            const string description = "Life is like a box of chocolates + AOE changes, so stay close to your allies";

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>> { { AbilityKey.Zap, new List<int> { 2, 5 } } });

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WebBomb, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Bone, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PanicPowder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DropChest, ReplenishFrequency = 0 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
            });

            var abilityAoeRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.CourageShanty, 1 },
                { AbilityKey.ReplenishArmor, 1 },
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.Antitoxin, 1 },
                { AbilityKey.AdamantPotion, 1 },
                { AbilityKey.HealingPotion, 1 },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 15 },

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 5 },

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Lure, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "ActionPoint", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "StartHealth", Value = 20 },

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "MoveRange", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "VisionRange", Value = 40 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
                { AbilityKey.CourageShanty, false },
                { AbilityKey.Sneak, false },
            });

            var cardEnergyFromAttackRule = new CardEnergyFromAttackMultipliedRule(1.25f);

            var allowedHunterCards = new List<AbilityKey>
            {
                AbilityKey.StrengthPotion,
                AbilityKey.SwiftnessPotion,
                AbilityKey.Bone,
                AbilityKey.Fireball,
                AbilityKey.Freeze,
                AbilityKey.BottleOfLye,
                AbilityKey.Teleportation,
                AbilityKey.HeavensFury,
                AbilityKey.RevealPath,
            };

            var allowedSorcererCards = new List<AbilityKey>
            {
                AbilityKey.CallCompanion,
                AbilityKey.HuntersMark,
                AbilityKey.BeastWhisperer,
                AbilityKey.RevealPath,
            };

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroHunter, allowedHunterCards },
                { BoardPieceId.HeroSorcerer, allowedSorcererCards },
            });

            var bardImmunities = new List<EffectStateType> { EffectStateType.Weaken, EffectStateType.MarkOfAvalon };
            var guardianImmunities = new List<EffectStateType> { EffectStateType.Diseased, EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Petrified };
            var hunterImmunities = new List<EffectStateType> { EffectStateType.Diseased, EffectStateType.Frozen, EffectStateType.Petrified };
            var rogueImmunities = new List<EffectStateType> { EffectStateType.Frozen, EffectStateType.Petrified };
            var sorcererImmunities = new List<EffectStateType> { EffectStateType.Diseased, EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Petrified };

            var pieceImmunityRule = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>>
            {
                { BoardPieceId.HeroBard, bardImmunities },
                { BoardPieceId.HeroGuardian, guardianImmunities },
                { BoardPieceId.HeroHunter, hunterImmunities },
                { BoardPieceId.HeroRogue, rogueImmunities },
                { BoardPieceId.HeroSorcerer, sorcererImmunities },
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

            var hunterMarkRule = new PetsFocusHunterMarkRule(true);

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 15,
                    healPerTurn = 0,
                    damagePerTurn = 0,
                    stacks = true,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Stealthed,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 5,
                    healPerTurn = 3,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Courageous,
                    durationTurns = 5,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Heroic,
                    durationTurns = 6,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },

                new StatusEffectData
                {
                    effectStateType = EffectStateType.Fearless,
                    durationTurns = 8,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Recovery,
                    durationTurns = 5,
                    healPerTurn = 3,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            return Ruleset.NewInstance(
                name,
                description,
                abilityDamageRule,
                startingCardsRule,
                abilityAoeRule,
                piecesAdjustedRule,
                abilityActionCostRule,
                cardEnergyFromAttackRule,
                allowedCardsRule,
                pieceImmunityRule,
                levelPropertiesRule,
                hunterMarkRule,
                statusEffectRule);
        }
    }
}
