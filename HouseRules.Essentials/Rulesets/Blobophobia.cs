namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class Blobophobia
    {
        internal static Ruleset Create()
        {
            const string name = "Blobophobia";
            const string description = "Money Slimes everywhere. On my face and in my hair.";
            const string longdesc = "";

            var blobophobiaRule = new PieceBlobophobiaRule(true);

            var abilityDamageAllRule = new AbilityDamageAllOverriddenRule(new Dictionary<AbilityKey, List<int>>
            {
                { AbilityKey.Zap, new List<int> { 2, 4, 2, 4 } },
            });

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingPush, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingSmash, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
            };
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
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.LeapHeavy, ReplenishFrequency = 2 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Bone, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.EnemyFireball, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.OilLamp, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DiseasedBite, ReplenishFrequency = 2 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WaterLamp, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HeavensFury, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DetectEnemies, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MinionCharge, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MissileSwarm, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HeavensFury, ReplenishFrequency = 0 },
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

            var abilityAoeRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.CourageShanty, 1 },
                { AbilityKey.ReplenishArmor, 1 },
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.MagicPotion, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
                { AbilityKey.Antitoxin, 1 },
                { AbilityKey.HealingPotion, 1 },
            });

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.StrengthPotion, false },
                { AbilityKey.SwiftnessPotion, false },
                { AbilityKey.MagicPotion, false },
                { AbilityKey.DamageResistPotion, false },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Sigataur, Property = "PowerIndex", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSlime, Property = "PowerIndex", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSlime, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "StartHealth", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Lure, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "ActionPoint", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "MoveRange", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "VisionRange", Value = 40 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Slimeling, Property = "StartHealth", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Slimeling, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Slimeling, Property = "MoveRange", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Slimeling, Property = "PowerIndex", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RatKing, Property = "StartHealth", Value = 120 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenQueen, Property = "StartHealth", Value = 90 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
                { AbilityKey.MinionCharge, false },
                { AbilityKey.CourageShanty, false },
                { AbilityKey.Sneak, false },
                { AbilityKey.Grapple, false },
                { AbilityKey.Arrow, false },
                { AbilityKey.LeapHeavy, false },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneGoldMaxAmount", 900 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOneLootChests", 12 },
                { "FloorOnePotionStand", 2 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoLootChests", 7 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorTwoGoldMaxAmount", 1100 },
                { "FloorThreeHealingFountains", 0 },
                { "FloorThreeLootChests", 0 },
                { "FloorThreeElvenSummoners", 0 },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Slimeling, new List<AbilityKey> { AbilityKey.EnemyDropStolenGoods } },
            });

            var entranceDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Slimeling, 0 }, // Unlimited slimes.
                { BoardPieceId.GiantSlime, 2 },
                { BoardPieceId.ElvenArcher, 1 },
            };
            var exitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Slimeling, 0 }, // Unlimited slimes.
                { BoardPieceId.GiantSlime, 3 },
                { BoardPieceId.Sigataur, 1 },
            };
            var entranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Slimeling, 0 }, // Unlimited slimes.
                { BoardPieceId.GiantSlime, 4 },
                { BoardPieceId.DruidArcher, 3 },
            };
            var exitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Slimeling, 0 }, // Unlimited slimes.
                { BoardPieceId.GiantSlime, 5 },
                { BoardPieceId.Sigataur, 1 },
            };
            var bossDeck = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Slimeling, 0 },
                { BoardPieceId.GiantSlime, 5 },
                { BoardPieceId.TheUnseen, 0 },
                { BoardPieceId.TheUnheard, 0 },
                { BoardPieceId.TheUnspoken, 0 },
                { BoardPieceId.DruidArcher, 3 },
                { BoardPieceId.ElvenPriest, 3 },
                { BoardPieceId.ElvenMystic, 3 },
                { BoardPieceId.Sigataur, 2 },
            };
            var myMonsterDeckConfig = new MyMonsterDeckOverriddenRule.MyDeckConfig
            {
                EntranceDeckFloor1 = entranceDeckFloor1,
                ExitDeckFloor1 = exitDeckFloor1,
                EntranceDeckFloor2 = entranceDeckFloor2,
                ExitDeckFloor2 = exitDeckFloor2,
                BossDeck = bossDeck,
                KeyHolderFloor1 = BoardPieceId.ElvenQueen,
                KeyHolderFloor2 = BoardPieceId.RatKing,
            };
            var myMonsterDeckRule = new MyMonsterDeckOverriddenRule(myMonsterDeckConfig);

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Slimeling, new List<AbilityKey> { AbilityKey.AcidSpit, AbilityKey.EnemyStealCard, AbilityKey.EnemyStealGold } },
                { BoardPieceId.ElvenArcher, new List<AbilityKey> { AbilityKey.EnemyFrostball, AbilityKey.EnemyMelee, AbilityKey.EnemyArrowSnipe } },
            });

            var pieceBehaviorsRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.Slimeling, new List<Behaviour> { Behaviour.AttackAndRetreat, Behaviour.Patrol, Behaviour.RangedSpellCaster, Behaviour.FleeToFOW, Behaviour.SlimeFusion } },
                { BoardPieceId.ElvenArcher, new List<Behaviour> { Behaviour.Patrol, Behaviour.RangedSpellCaster, Behaviour.FollowPlayerRangedAttacker } },
            });

            var allowedChestCardsRule = new CardChestAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.GrapplingTotem,
                        AbilityKey.PlayerLeap,
                        AbilityKey.MarkOfVerga,
                        AbilityKey.GrapplingSmash,
                        AbilityKey.GrapplingTotem,
                        AbilityKey.TauntingScream,
                        AbilityKey.PlayerLeap,
                        AbilityKey.MarkOfVerga,
                        AbilityKey.GrapplingPush,
                        AbilityKey.GrapplingSmash,
                        AbilityKey.TauntingScream,
                        AbilityKey.PlayerLeap,
                        AbilityKey.MarkOfVerga,
                        AbilityKey.GrapplingPush,
                        AbilityKey.GrapplingSmash,
                    }
                },
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingWard,
                        AbilityKey.WhirlwindAttack,
                        AbilityKey.WarCry,
                        AbilityKey.PiercingThrow,
                        AbilityKey.Charge,
                        AbilityKey.HealingWard,
                        AbilityKey.WhirlwindAttack,
                        AbilityKey.WarCry,
                        AbilityKey.TheBehemoth,
                        AbilityKey.PiercingThrow,
                        AbilityKey.Charge,
                        AbilityKey.WhirlwindAttack,
                        AbilityKey.WarCry,
                        AbilityKey.TheBehemoth,
                        AbilityKey.PiercingThrow,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.PiercingVoice,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.SongOfResilience,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.HurricaneAnthem,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.HurricaneAnthem,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.HuntersMark,
                        AbilityKey.Lure,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.HuntersMark,
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.Lure,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.BoobyTrap,
                        AbilityKey.FlashBomb,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CursedDagger,
                        AbilityKey.FlashBomb,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CursedDagger,
                        AbilityKey.BoobyTrap,
                        AbilityKey.FlashBomb,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.Vortex,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicShield,
                        AbilityKey.MagicBarrier,
                        AbilityKey.Vortex,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicBarrier,
                        AbilityKey.Vortex,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.Deflect,
                        AbilityKey.GuidingLight,
                        AbilityKey.Implode,
                        AbilityKey.MissileSwarm,
                        AbilityKey.Portal,
                        AbilityKey.Deflect,
                        AbilityKey.GuidingLight,
                        AbilityKey.Implode,
                        AbilityKey.MissileSwarm,
                        AbilityKey.Portal,
                        AbilityKey.Deflect,
                        AbilityKey.GuidingLight,
                        AbilityKey.Implode,
                        AbilityKey.MissileSwarm,
                        AbilityKey.Portal,
                    }
                },
            });

            var electricityDamageRule = new PartyElectricityDamageOverriddenRule(true);
            var enemyRespawnRule = new EnemyRespawnDisabledRule(true);
            var grappleUnhookedRule = new GrappleUnhookedRule(true);
            var backstabConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId> { BoardPieceId.HeroBard, BoardPieceId.HeroRogue });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Invulnerable1,
                    durationTurns = 2,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                blobophobiaRule,
                abilityDamageAllRule,
                startingCardsRule,
                abilityAoeRule,
                abilityMaxedRule,
                piecesAdjustedRule,
                abilityActionCostRule,
                levelPropertiesRule,
                myMonsterDeckRule,
                pieceAbilityRule,
                pieceBehaviorsRule,
                allowedChestCardsRule,
                electricityDamageRule,
                enemyRespawnRule,
                grappleUnhookedRule,
                backstabConfigRule,
                pieceUseWhenKilledRule,
                statusEffectRule);
        }
    }
}
