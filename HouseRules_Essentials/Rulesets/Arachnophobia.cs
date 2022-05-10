namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class Arachnophobia
    {
        internal static Ruleset Create()
        {
            const string name = "Arachnophobia";
            const string description = "Money Spiders everywhere. On my face and in my hair.";

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>> { { AbilityKey.Zap, new List<int> { 2, 4 } } });

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, ReplenishFrequency = 0 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Bone, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PanicPowder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HeavensFury, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
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

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.StrengthPotion, false },
                { AbilityKey.SwiftnessPotion, false },
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

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "MoveRange", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "VisionRange", Value = 40 },

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "StartHealth", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "MoveRange", Value = 2 },

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RatKing, Property = "StartHealth", Value = 50 },

                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenQueen, Property = "StartHealth", Value = 60 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Gorgon, Property = "StartHealth", Value = 85 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
                { AbilityKey.CourageShanty, false },
                { AbilityKey.Sneak, false },
            });

            var ints = new Dictionary<string, int>
            {
                { "FloorOneGoldMaxAmount", 1200 },
                { "FloorOneHealingFountains", 2 },
                { "FloorOneLootChests", 12 },
                { "FloorOneClassCardChests", 8 },
                { "FloorTwoHealingFountains", 3 },
                { "FloorTwoLootChests", 9 },
                { "FloorTwoGoldMaxAmount", 1500 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreeLootChests", 0 },
            };

            var levelPropertiesRule = new LevelPropertiesModifiedRule(ints);

            var heroImmunities = new List<EffectStateType> { EffectStateType.Diseased };

            var pieceImmunityRule = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>>
            {
                { BoardPieceId.HeroBard, heroImmunities },
                { BoardPieceId.HeroGuardian, heroImmunities },
                { BoardPieceId.HeroHunter, heroImmunities },
                { BoardPieceId.HeroRogue, heroImmunities },
                { BoardPieceId.HeroSorcerer, heroImmunities },
                { BoardPieceId.Verochka, new List<EffectStateType> { EffectStateType.Diseased } },
                { BoardPieceId.RatKing, new List<EffectStateType> { EffectStateType.Petrified, EffectStateType.Stunned, EffectStateType.Panic, EffectStateType.Frozen, EffectStateType.Disoriented, EffectStateType.Confused } },
                { BoardPieceId.ElvenQueen, new List<EffectStateType> { EffectStateType.Petrified, EffectStateType.Stunned, EffectStateType.Disoriented, EffectStateType.Confused } },
                { BoardPieceId.Gorgon, new List<EffectStateType> { EffectStateType.Petrified, EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.MarkOfAvalon, EffectStateType.Panic, EffectStateType.Disoriented, EffectStateType.Confused } },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Spider, new List<AbilityKey> { AbilityKey.EnemyDropStolenGoods } },
            });

            var entranceDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 5 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.GiantSlime, 1 },
            };
            var exitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 2 },
                { BoardPieceId.GiantSpider, 3 },
                { BoardPieceId.ElvenQueen, 1 },
            };
            var entranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 2 },
                { BoardPieceId.GiantSpider, 3 },
                { BoardPieceId.DruidArcher, 4 },
            };
            var exitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 3 },
                { BoardPieceId.GiantSpider, 2 },
                { BoardPieceId.RatKing, 1 },
            };
            var bossDeck = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 },
                { BoardPieceId.GiantSpider, 4 },
                { BoardPieceId.TheUnseen, 0 },
                { BoardPieceId.TheUnheard, 0 },
                { BoardPieceId.TheUnspoken, 0 },
                { BoardPieceId.DruidArcher, 3 },
                { BoardPieceId.ElvenPriest, 3 },
                { BoardPieceId.ElvenMystic, 3 },
                { BoardPieceId.Sigataur, 2 },
            };
            var monsterDeckConfig = new MonsterDeckOverriddenRule.DeckConfig
            {
                EntranceDeckFloor1 = entranceDeckFloor1,
                ExitDeckFloor1 = exitDeckFloor1,
                EntranceDeckFloor2 = entranceDeckFloor2,
                ExitDeckFloor2 = exitDeckFloor2,
                BossDeck = bossDeck,
                KeyHolderFloor1 = BoardPieceId.CavetrollBoss,
                KeyHolderFloor2 = BoardPieceId.Sigataur,
                Boss = BoardPieceId.Gorgon,
            };
            var monsterDeckRule = new MonsterDeckOverriddenRule(monsterDeckConfig);

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Spider, new List<AbilityKey> { AbilityKey.SpiderWebshot, AbilityKey.AcidSpit, AbilityKey.EnemyStealCard, AbilityKey.EnemyStealGold } },
                { BoardPieceId.ElvenQueen, new List<AbilityKey> { AbilityKey.EnemyKnockbackMelee, AbilityKey.EnemyFireball, AbilityKey.EarthShatter } },
                { BoardPieceId.RatKing, new List<AbilityKey> { AbilityKey.DigRatsNest, AbilityKey.DiseasedBite, AbilityKey.VerminFrenzy } },
            });

            var pieceBehaviorsRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.Spider, new List<Behaviour> { Behaviour.AttackAndRetreat, Behaviour.Patrol, Behaviour.FleeToFOW, Behaviour.ChargeMove } },
            });

            var piecePieceTypeRule = new PiecePieceTypeListOverriddenRule(new Dictionary<BoardPieceId, List<PieceType>>
            {
                { BoardPieceId.Spider, new List<PieceType> { PieceType.Enemy, PieceType.Thief, PieceType.Canine } },
            });

            var cardClassRestrictionRule = new CardClassRestrictionOverriddenRule(new Dictionary<AbilityKey, BoardPieceId>
            {
                { AbilityKey.BeastWhisperer, BoardPieceId.SporeFungus },
            });

            var enemyRespawnRule = new EnemyRespawnDisabledRule(true);

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 5,
                    healPerTurn = 3,
                    stacks = true,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Courageous,
                    durationTurns = 2,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Heroic,
                    durationTurns = 4,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Fearless,
                    durationTurns = 6,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Invulnerable1,
                    durationTurns = 2,
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
                abilityMaxedRule,
                piecesAdjustedRule,
                abilityActionCostRule,
                levelPropertiesRule,
                pieceImmunityRule,
                monsterDeckRule,
                pieceAbilityRule,
                pieceBehaviorsRule,
                piecePieceTypeRule,
                cardClassRestrictionRule,
                enemyRespawnRule,
                pieceUseWhenKilledRule,
                statusEffectRule);
        }
    }
}
