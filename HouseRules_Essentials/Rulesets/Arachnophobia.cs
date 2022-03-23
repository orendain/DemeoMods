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

            var abilityDamageRule = new AbilityDamageAdjustedRule(new Dictionary<AbilityKey, int> { { AbilityKey.Zap, 1 } });

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.IceLamp, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, IsReplenishable = false },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Bone, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PanicPowder, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, IsReplenishable = false },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HeavensFury, IsReplenishable = false },
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
                { "FloorOneBudget", 200 },
                { "FloorOneOuterRingZoneBudget", 160 },
                { "FloorOneBudgetPostSpike", 160 },
                { "FloorTwoBudget", 200 },
                { "FloorTwoOuterRingZoneBudget", 190 },
                { "FloorTwoBudgetPostSpike", 230 },
                { "FloorThreeBudget", 60 },
                { "FloorThreeOuterRingZoneBudget", 60 },
                { "FloorThreeBudgetPostSpike", 80 },
                { "PacingSpikeSegmentFloorOneBudget", 2 },
                { "PacingSpikeSegmentFloorTwoBudget", 6 },
                { "PacingSpikeSegmentFloorThreeBudget", 1 },
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
                { BoardPieceId.GiantSpider, 3 },
                { BoardPieceId.GiantSlime, 1 },
            };
            var exitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 5 },
                { BoardPieceId.GiantSpider, 4 },
                { BoardPieceId.ElvenQueen, 1 },
            };
            var entranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 2 },
                { BoardPieceId.GiantSpider, 8 },
                { BoardPieceId.DruidArcher, 4 },
            };
            var exitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.SpiderEgg, 3 },
                { BoardPieceId.GiantSpider, 8 },
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
                statusEffectRule);
        }
    }
}
