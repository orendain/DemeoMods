namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DemeoReloaded
    {
        internal static Ruleset Create()
        {
            const string name = "Demeo Reloaded";
            const string description = "MANY class changes. NEW enemies. BETTER loot. No respawns. Yet somehow challenging...";

            var spawnCatetoriesRule = new SpawnCategoryOverriddenRule(new Dictionary<BoardPieceId, List<int>>
            {
                { BoardPieceId.Mimic, new List<int> { 1, 1, 1 } },
                { BoardPieceId.Wyvern, new List<int> { 1, 1, 2 } },
                { BoardPieceId.ChestGoblin, new List<int> { 1, 1, 2 } },
                { BoardPieceId.RootGolem, new List<int> { 1, 0, 2 } },
                { BoardPieceId.Brookmare, new List<int> { 1, 0, 2 } },
                { BoardPieceId.EarthElemental, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Bandit, new List<int> { 2, 0, 1 } },
                { BoardPieceId.Cavetroll, new List<int> { 1, 0, 1 } },
                { BoardPieceId.DruidArcher, new List<int> { 4, 0, 1 } },
                { BoardPieceId.DruidHoundMaster, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenArcher, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenHound, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenMarauder, new List<int> { 1, 0, 1 } },
                { BoardPieceId.GoblinChieftan, new List<int> { 4, 0, 1 } },
                { BoardPieceId.GoblinMadUn, new List<int> { 6, 1, 1 } },
                { BoardPieceId.RootBeast, new List<int> { 4, 0, 1 } },
                { BoardPieceId.RootHound, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ScabRat, new List<int> { 3, 0, 1 } },
                { BoardPieceId.TheUnspoken, new List<int> { 3, 0, 1 } },
                { BoardPieceId.Spider, new List<int> { 6, 4, 1 } },
                { BoardPieceId.SpiderEgg, new List<int> { 6, 3, 1 } },
                { BoardPieceId.GiantSpider, new List<int> { 4, 0, 1 } },
                { BoardPieceId.Rat, new List<int> { 8, 6, 1 } },
                { BoardPieceId.Slimeling, new List<int> { 4, 1, 1 } },
                { BoardPieceId.Thug, new List<int> { 3, 1, 2 } },
                { BoardPieceId.ElvenMystic, new List<int> { 6, 0, 1 } },
                { BoardPieceId.ElvenPriest, new List<int> { 4, 0, 1 } },
                { BoardPieceId.ElvenSkirmisher, new List<int> { 4, 0, 1 } },
                { BoardPieceId.FireElemental, new List<int> { 3, 0, 1 } },
                { BoardPieceId.GoblinFighter, new List<int> { 6, 3, 1 } },
                { BoardPieceId.GoblinRanger, new List<int> { 6, 3, 1 } },
                { BoardPieceId.Gorgon, new List<int> { 1, 0, 2 } },
                { BoardPieceId.IceElemental, new List<int> { 2, 0, 1 } },
                { BoardPieceId.KillerBee, new List<int> { 8, 4, 1 } },
                { BoardPieceId.RatNest, new List<int> { 4, 2, 1 } },
                { BoardPieceId.RootMage, new List<int> { 4, 1, 1 } },
                { BoardPieceId.SporeFungus, new List<int> { 6, 2, 1 } },
                { BoardPieceId.TheUnheard, new List<int> { 3, 0, 1 } },
                { BoardPieceId.SilentSentinel, new List<int> { 1, 0, 2 } },
                { BoardPieceId.GiantSlime, new List<int> { 2, 0, 1 } },
            });

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },

            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Charge, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grab, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BeastWhisperer, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CursedDagger, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DiseasedBite, ReplenishFrequency = 1 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Freeze, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SummonElemental, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 11 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "AttackDamage", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "CriticalHitDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "CriticalHitDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "CriticalHitDamage", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "BerserkBelowHealth", Value = 0.9f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartArmor", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartHealth", Value = 24 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "BerserkBelowHealth", Value = 0.75f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "BarkArmor", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "MoveRange", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.KillerBee, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "WaterTrailChance", Value = 0.15f },
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.Antitoxin,
                        AbilityKey.Bone,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.OneMoreThing,
                        AbilityKey.BottleOfLye,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.AdamantPotion,
                        AbilityKey.HealingPotion,
                        AbilityKey.HealingWard,
                        AbilityKey.WhirlwindAttack,
                        AbilityKey.WarCry,
                        AbilityKey.TheBehemoth,
                        AbilityKey.PiercingThrow,
                        AbilityKey.Charge,
                        AbilityKey.HealingWard,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.AdamantPotion,
                        AbilityKey.HealingPotion,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.PiercingVoice,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.HurricaneAnthem,

                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.Antitoxin,
                        AbilityKey.BeastWhisperer,
                        AbilityKey.WebBomb,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.RevealPath,
                        AbilityKey.DetectEnemies,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.AdamantPotion,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.HuntersMark,
                        AbilityKey.Lure,

                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Bone,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.AdamantPotion,
                        AbilityKey.HealingPotion,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.BoobyTrap,
                        AbilityKey.FlashBomb,

                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.Antitoxin,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.AdamantPotion,
                        AbilityKey.HealingPotion,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicShield,
                        AbilityKey.MagicBarrier,
                        AbilityKey.Vortex,

                    }
                },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Recovery,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Courageous,
                    durationTurns = 3,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Heroic,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Fearless,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyKnockbackMelee, AbilityKey.EarthShatter, AbilityKey.EnemyJavelin } },
                { BoardPieceId.Mimic, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.AcidSpit } },
                { BoardPieceId.RootMage, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.TeleportEnemy } },
                { BoardPieceId.KillerBee, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.ThornPowder } },
            });

            var pieceBehavourListRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.EarthElemental, new List<Behaviour> { Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackPlayer, Behaviour.EarthShatter, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Mimic, new List<Behaviour> { Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.RootMage, new List<Behaviour> { Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackPlayer, Behaviour.CastOnTeam } },
                { BoardPieceId.KillerBee, new List<Behaviour> { Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
            });

            var pieceImmunityRule = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>>
            {
                { BoardPieceId.HeroSorcerer, new List<EffectStateType> {EffectStateType.Stunned, EffectStateType.Frozen } },
                { BoardPieceId.HeroHunter, new List<EffectStateType> {EffectStateType.Tangled, EffectStateType.Petrified } },
                { BoardPieceId.HeroGuardian, new List<EffectStateType> {EffectStateType.Stunned, EffectStateType.Weaken } },
                { BoardPieceId.HeroBard, new List<EffectStateType> {EffectStateType.Diseased } },
                { BoardPieceId.HeroRogue, new List<EffectStateType> {EffectStateType.Tangled, EffectStateType.Diseased } },
                { BoardPieceId.Mimic, new List<EffectStateType> {EffectStateType.Panic, EffectStateType.Stunned, EffectStateType.Weaken, EffectStateType.Diseased } },
                { BoardPieceId.Wyvern, new List<EffectStateType> {EffectStateType.Panic, EffectStateType.Tangled, EffectStateType.Frozen, EffectStateType.Diseased, EffectStateType.Tangled } },
                { BoardPieceId.KillerBee, new List<EffectStateType> {EffectStateType.Tangled, EffectStateType.Diseased } },
                { BoardPieceId.EarthElemental, new List<EffectStateType> {EffectStateType.Stunned, EffectStateType.Diseased, EffectStateType.Panic, EffectStateType.Tangled, EffectStateType.Weaken } },
            });

            var tileEffectDuration = new TileEffectDurationOverriddenRule(new Dictionary<Boardgame.Board.TileEffect, int>
            {
                { TileEffect.Gas, 3 },
                { TileEffect.Acid, 4 },
                { TileEffect.Web, 4 },
                { TileEffect.Water, 3 },
                { TileEffect.Target, 0 },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
                { AbilityKey.Sneak, false },
                { AbilityKey.Grab, false },
                { AbilityKey.BeastWhisperer, false },
            });

            var abilityHealOverriddenRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.HealingPotion, 12 },
                { AbilityKey.Rejuvenation, 12 },
                { AbilityKey.AltarHeal, 12 },
            });

            var aoeAdjustmentedRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.SongOfRecovery, 2 },
                { AbilityKey.SongOfResilience, 2 },
                { AbilityKey.FlashBomb, 1 },
                { AbilityKey.WebBomb, 1 },
                { AbilityKey.PoisonBomb, 1 },
                { AbilityKey.HailOfArrows, 1 },
                { AbilityKey.WarCry, 1 },
                { AbilityKey.WhirlwindAttack, 1 },
                { AbilityKey.Fireball, 1 },
                { AbilityKey.Freeze, 1 },
            });

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>>
            {
                { AbilityKey.Zap, new List<int> { 3, 8 } },
                { AbilityKey.Fireball, new List<int> { 12, 25 } },
                { AbilityKey.Freeze, new List<int> { 7, 15 } },
                { AbilityKey.WhirlwindAttack, new List<int> { 4, 9 } },
                { AbilityKey.Charge, new List<int> { 4, 12 } },
                { AbilityKey.PiercingThrow, new List<int> { 5, 11 } },
                { AbilityKey.Blink, new List<int> { 7, 17 } },
                { AbilityKey.CursedDagger, new List<int> { 4, 12 } },
                { AbilityKey.PoisonedTip, new List<int> { 6, 13 } },
                { AbilityKey.HailOfArrows, new List<int> { 5, 11 } },
                { AbilityKey.Arrow, new List<int> { 4, 11 } },
                { AbilityKey.Electricity, new List<int> { 2, 5 } },
                { AbilityKey.DiseasedBite, new List<int> { 2, 5 } },
            });

            var backstabConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId> { BoardPieceId.HeroBard, BoardPieceId.HeroRogue });
            var petsFocusHuntersMarkRule = new PetsFocusHunterMarkRule(true);
            var enemyRespawnDisabledRule = new EnemyRespawnDisabledRule(true);
            var cardEnergyFromAttackRule = new CardEnergyFromAttackMultipliedRule(0.75f);
            var cardEnergyFromRecyclingRule = new CardEnergyFromRecyclingMultipliedRule(0.75f);
            var enemyAttackScaledRule = new EnemyAttackScaledRule(1.25f);
            var enemyHealthScaledRule = new EnemyHealthScaledRule(1.334f);

            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "SewersFloor12",
                "ShopFloor02",
                "ElvenFloor02",
                "ForestShopFloor",
                "ForestFloor02",
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOneLootChests", 1 },
                { "FloorOneClassCardChests", 2 },
                { "FloorOneGoldMaxAmount", 600 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoLootChests", 2 },
                { "FloorTwoClassCardChests", 3 },
                { "FloorTwoGoldMaxAmount", 800 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreeLootChests", 1 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                spawnCatetoriesRule,
                startingCardsRule,
                piecesAdjustedRule,
                allowedCardsRule,
                statusEffectRule,
                pieceAbilityRule,
                pieceBehavourListRule,
                pieceImmunityRule,
                tileEffectDuration,
                abilityActionCostRule,
                abilityHealOverriddenRule,
                aoeAdjustmentedRule,
                abilityDamageRule,
                backstabConfigRule,
                petsFocusHuntersMarkRule,
                enemyRespawnDisabledRule,
                cardEnergyFromAttackRule,
                cardEnergyFromRecyclingRule,
                enemyAttackScaledRule,
                enemyHealthScaledRule,
                levelSequenceOverriddenRule,
                levelPropertiesRule);
        }
    }
}
