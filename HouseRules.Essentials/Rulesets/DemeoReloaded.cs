namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class DemeoReloaded
    {
        internal static Ruleset Create()
        {
            const string name = "Demeo Reloaded";
            const string description = "The fight for the future begins...";
            const string longdesc = "";

            var spawnCategoriesRule = new SpawnCategoryOverriddenRule(new Dictionary<BoardPieceId, List<int>>
            {
                { BoardPieceId.BigBoiMutant, new List<int> { 3, 1, 1 } },
                { BoardPieceId.LargeCorruption, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ReptileArcher, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ReptileMutantWizard, new List<int> { 3, 1, 1 } },
                { BoardPieceId.SandScorpion, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ServantOfAlfaragh, new List<int> { 2, 1, 1 } },
                { BoardPieceId.ScorpionSandPile, new List<int> { 3, 1, 1 } },
                { BoardPieceId.EmptySandPile, new List<int> { 2, 1, 1 } },
                { BoardPieceId.GoldSandPile, new List<int> { 2, 1, 1 } },
                { BoardPieceId.SmallCorruption, new List<int> { 4, 1, 1 } },
                { BoardPieceId.GeneralRonthian, new List<int> { 2, 1, 1 } },
                { BoardPieceId.Wyvern, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Cavetroll, new List<int> { 2, 1, 1 } },
                { BoardPieceId.RootGolem, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Brookmare, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Gorgon, new List<int> { 2, 1, 2 } },
                { BoardPieceId.SilentSentinel, new List<int> { 2, 1, 2 } },
                { BoardPieceId.ElvenArcher, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ElvenHound, new List<int> { 5, 2, 1 } },
                { BoardPieceId.RootHound, new List<int> { 5, 2, 1 } },
                { BoardPieceId.TheUnspoken, new List<int> { 4, 1, 1 } },
                { BoardPieceId.Mimic, new List<int> { 2, 1, 1 } },
                { BoardPieceId.EarthElemental, new List<int> { 2, 1, 1 } },
                { BoardPieceId.Sigataur, new List<int> { 2, 1, 1 } },
                { BoardPieceId.ChestGoblin, new List<int> { 2, 1, 1 } },
                { BoardPieceId.CultMemberElder, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ElvenCultist, new List<int> { 4, 2, 1 } },
                { BoardPieceId.SpiderEgg, new List<int> { 4, 1, 1 } },
                { BoardPieceId.GiantSlime, new List<int> { 2, 1, 1 } },
                { BoardPieceId.FireElemental, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ElvenMarauder, new List<int> { 2, 1, 1 } },
                { BoardPieceId.IceElemental, new List<int> { 2, 1, 1 } },
                { BoardPieceId.GiantSpider, new List<int> { 2, 1, 1 } },
                { BoardPieceId.Bandit, new List<int> { 4, 1, 1 } },
                { BoardPieceId.DruidArcher, new List<int> { 4, 2, 1 } },
                { BoardPieceId.DruidHoundMaster, new List<int> { 4, 2, 1 } },
                { BoardPieceId.GoblinChieftan, new List<int> { 4, 2, 1 } },
                { BoardPieceId.GoblinMadUn, new List<int> { 4, 2, 1 } },
                { BoardPieceId.RootBeast, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ScabRat, new List<int> { 4, 2, 1 } },
                { BoardPieceId.Spider, new List<int> { 4, 2, 1 } },
                { BoardPieceId.Rat, new List<int> { 4, 2, 1 } },
                { BoardPieceId.TheUnheard, new List<int> { 3, 1, 2 } },
                { BoardPieceId.Slimeling, new List<int> { 4, 1, 1 } },
                { BoardPieceId.Thug, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ElvenMystic, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenPriest, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenSkirmisher, new List<int> { 4, 2, 1 } },
                { BoardPieceId.GoblinFighter, new List<int> { 5, 2, 1 } },
                { BoardPieceId.GoblinRanger, new List<int> { 5, 2, 1 } },
                { BoardPieceId.KillerBee, new List<int> { 4, 2, 1 } },
                { BoardPieceId.RatNest, new List<int> { 4, 2, 1 } },
                { BoardPieceId.RootMage, new List<int> { 4, 2, 1 } },
                { BoardPieceId.SporeFungus, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenSpearman, new List<int> { 4, 1, 1 } },
            });

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingSmash, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingPush, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TauntingScream, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implode, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MissileSwarm, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Deflect, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MinionCharge, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TurretHealProjectile, ReplenishFrequency = 5 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.StrengthenCourage, ReplenishFrequency = 1 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Whirlwind, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingSpear, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Charge, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grab, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SplittingArrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MonsterBait, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HunterArrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HunterArrow, ReplenishFrequency = 1 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonGasGrenade, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CursedDagger, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DiseasedBite, ReplenishFrequency = 1 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TorchLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Freeze, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SummonElemental, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBarbarian, barbarianCards },
                { BoardPieceId.HeroWarlock, warlockCards },
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
            });

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "CriticalHitDamage", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 11 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 16 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "AttackDamage", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "CriticalHitDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "CriticalHitDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "CriticalHitDamage", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "CriticalHitDamage", Value = 11 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "BerserkBelowHealth", Value = 0.99f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartArmor", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ChestGoblin, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ChestGoblin, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ChestGoblin, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "BerserkBelowHealth", Value = 0.65f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "BarkArmor", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "StartHealth", Value = 25 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Bandit, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Thug, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.KillerBee, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "WaterTrailChance", Value = 0.15f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Barricade, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HealingBeacon, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.MonsterBait, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "AttackDamage", Value = 3 },
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.FreeAP,
                        AbilityKey.ScarePowder,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.Teleport,
                        AbilityKey.Strength,
                        AbilityKey.Speed,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.BeaconOfHealing,
                        AbilityKey.Invulnerability,
                        AbilityKey.Whirlwind,
                        AbilityKey.WarCry,
                        AbilityKey.BeaconOfSmite,
                        AbilityKey.PiercingSpear,
                        AbilityKey.Charge,
                        AbilityKey.BeaconOfHealing,
                        AbilityKey.Whirlwind,
                        AbilityKey.WarCry,
                        AbilityKey.BeaconOfSmite,
                        AbilityKey.PiercingSpear,
                        AbilityKey.Charge,
                        AbilityKey.BeaconOfHealing,
                        AbilityKey.Whirlwind,
                        AbilityKey.WarCry,
                        AbilityKey.BeaconOfSmite,
                        AbilityKey.PiercingSpear,
                        AbilityKey.Charge,
                    }
                },
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.FreeAP,
                        AbilityKey.ScarePowder,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.GodsFury,
                        AbilityKey.Teleport,
                        AbilityKey.Speed,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.PlayerLeap,
                        AbilityKey.TauntingScream,
                        AbilityKey.GrapplingPush,
                        AbilityKey.GrapplingSmash,
                        AbilityKey.TauntingScream,
                        AbilityKey.MarkOfVerga,
                        AbilityKey.GrapplingTotem,
                        AbilityKey.PlayerLeap,
                        AbilityKey.GrapplingPush,
                        AbilityKey.GrapplingSmash,
                        AbilityKey.MarkOfVerga,
                        AbilityKey.GrapplingTotem,
                        AbilityKey.PlayerLeap,
                        AbilityKey.GrapplingPush,
                        AbilityKey.GrapplingSmash,
                        AbilityKey.TauntingScream,
                        AbilityKey.MarkOfVerga,
                        AbilityKey.GrapplingTotem,
                        AbilityKey.PlayerLeap,
                        AbilityKey.GrapplingPush,
                        AbilityKey.GrapplingSmash,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.Rejuvenation,
                        AbilityKey.FreeAP,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.GodsFury,
                        AbilityKey.Teleport,
                        AbilityKey.Speed,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.NotesOfConfusion,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.Tornado,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.NotesOfConfusion,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.Tornado,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.NotesOfConfusion,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.Tornado,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.SwordOfAvalon,
                        AbilityKey.ScarePowder,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.Teleport,
                        AbilityKey.Speed,
                        AbilityKey.Strength,
                        AbilityKey.Invulnerability,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.Confuse,
                        AbilityKey.RatWhisperer,
                        AbilityKey.SplittingArrow,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.MarkOfAvalon,
                        AbilityKey.MonsterBait,
                        AbilityKey.Confuse,
                        AbilityKey.RatWhisperer,
                        AbilityKey.SplittingArrow,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.MarkOfAvalon,
                        AbilityKey.MonsterBait,
                        AbilityKey.Confuse,
                        AbilityKey.RatWhisperer,
                        AbilityKey.SplittingArrow,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.MarkOfAvalon,
                        AbilityKey.MonsterBait,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.ScarePowder,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.GodsFury,
                        AbilityKey.Teleport,
                        AbilityKey.Strength,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.Blink,
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.ProximityMine,
                        AbilityKey.Flashbang,
                        AbilityKey.Blink,
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.ProximityMine,
                        AbilityKey.Flashbang,
                        AbilityKey.Blink,
                        AbilityKey.PoisonGasGrenade,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.ProximityMine,
                        AbilityKey.Flashbang,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.Regroup,
                        AbilityKey.ScarePowder,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.Teleport,
                        AbilityKey.Speed,
                        AbilityKey.GodsFury,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.MagicPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicShield,
                        AbilityKey.MagicWall,
                        AbilityKey.VortexDust,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicShield,
                        AbilityKey.MagicWall,
                        AbilityKey.VortexDust,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicShield,
                        AbilityKey.MagicWall,
                        AbilityKey.VortexDust,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.Regroup,
                        AbilityKey.ScarePowder,
                        AbilityKey.MiniBarricade,
                        AbilityKey.SodiumHydroxide,
                        AbilityKey.Teleport,
                        AbilityKey.Speed,
                        AbilityKey.Invulnerability,
                        AbilityKey.Heal,
                        AbilityKey.VigorPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.MagicPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.VialOfIceImmunity,
                        AbilityKey.VialOfFireImmunity,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
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

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 25,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                    healPerTurn = 3,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Recovery,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                    healPerTurn = 3,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Courageous,
                    durationTurns = 3,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Heroic,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Fearless,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyKnockbackMelee, AbilityKey.EarthShatter, AbilityKey.EnemyJavelin } },
                { BoardPieceId.Mimic, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.AcidSpit } },
                { BoardPieceId.RootMage, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.TeleportEnemy } },
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyStealGold } },
                { BoardPieceId.KillerBee, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.ThornPowder } },
            });

            var pieceBehaviourListRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.EarthElemental, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.EarthShatter, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Mimic, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.RootMage, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.CastOnTeam } },
                { BoardPieceId.KillerBee, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.ChestGoblin, new List<Behaviour> { Behaviour.Patrol, Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackAndRetreat } },
            });

            var pieceImmunityRule = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>>
            {
                { BoardPieceId.HeroBarbarian, new List<EffectStateType> { EffectStateType.Netted } },
                { BoardPieceId.HeroSorcerer, new List<EffectStateType> { EffectStateType.Frozen } },
                { BoardPieceId.HeroHunter, new List<EffectStateType> { EffectStateType.Petrified } },
                { BoardPieceId.HeroGuardian, new List<EffectStateType> { EffectStateType.Weaken1Turn, EffectStateType.Weaken2Turns } },
                { BoardPieceId.HeroBard, new List<EffectStateType> { EffectStateType.Diseased } },
                { BoardPieceId.HeroRogue, new List<EffectStateType> { EffectStateType.Tangled } },
                { BoardPieceId.HeroWarlock, new List<EffectStateType> { EffectStateType.CorruptedRage } },
                { BoardPieceId.WarlockMinion, new List<EffectStateType> { EffectStateType.CorruptedRage } },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyDropStolenGoods, AbilityKey.DropChest } },
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.Explosion, AbilityKey.DeathDropJavelin } },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
                { AbilityKey.Overcharge, false },
                { AbilityKey.LightningBolt, false },
                { AbilityKey.Grab, false },
                { AbilityKey.Stealth, false },
                { AbilityKey.StrengthenCourage, false },
                { AbilityKey.MinionCharge, false },
                { AbilityKey.Grapple, false },
            });

            var abilityHealOverriddenRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.Heal, 10 },
                { AbilityKey.Rejuvenation, 10 },
                { AbilityKey.AltarHeal, 15 },
                { AbilityKey.TurretHealProjectile, 4 },
            });

            var aoeAdjustedRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.SongOfRecovery, 2 },
                { AbilityKey.SongOfResilience, 2 },
                { AbilityKey.Flashbang, 1 },
                { AbilityKey.WebBomb, 1 },
                { AbilityKey.PoisonGasGrenade, 1 },
                { AbilityKey.WarCry, 1 },
                { AbilityKey.Whirlwind, 1 },
            });

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>>
            {
                { AbilityKey.Zap, new List<int> { 3, 8, 3, 8 } },
                { AbilityKey.Fireball, new List<int> { 12, 30, 7, 15 } },
                { AbilityKey.Freeze, new List<int> { 7, 20, 7, 20 } },
                { AbilityKey.VortexDust, new List<int> { 3, 12, 2, 5 } },
                { AbilityKey.Whirlwind, new List<int> { 4, 9, 4, 9 } },
                { AbilityKey.Charge, new List<int> { 4, 12, 4, 12 } },
                { AbilityKey.PiercingSpear, new List<int> { 5, 11, 5, 11 } },
                { AbilityKey.Blink, new List<int> { 7, 19, 7, 19 } },
                { AbilityKey.CursedDagger, new List<int> { 4, 13, 4, 13 } },
                { AbilityKey.PoisonedTip, new List<int> { 6, 16, 6, 16 } },
                { AbilityKey.SplittingArrow, new List<int> { 6, 16, 6, 16 } },
                { AbilityKey.HunterArrow, new List<int> { 4, 12, 4, 12 } },
                { AbilityKey.NotesOfConfusion, new List<int> { 4, 9, 4, 8 } },
                { AbilityKey.ShatteringVoice, new List<int> { 7, 15, 7, 14 } },
                { AbilityKey.Grapple, new List<int> { 4, 11, 4, 11 } },
                { AbilityKey.GrapplingSmash, new List<int> { 4, 9, 4, 9 } },
            });

            var backstabConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId> { BoardPieceId.HeroBard, BoardPieceId.HeroRogue });
            var petsFocusHuntersMarkRule = new PetsFocusHunterMarkRule(true);
            var enemyRespawnDisabledRule = new EnemyRespawnDisabledRule(true);
            var cardEnergyFromAttackRule = new CardEnergyFromAttackMultipliedRule(0.5f);
            var cardEnergyFromRecyclingRule = new CardEnergyFromRecyclingMultipliedRule(0.5f);
            var enemyAttackScaledRule = new EnemyAttackScaledRule(1.2f);
            var enemyHealthScaledRule = new EnemyHealthScaledRule(1.334f);
            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "SewersFloor01",
                "ForestShopFloor",
                "ForestFloor02",
                "ShopFloor02",
                "ElvenFloor14",
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 2 },
                { "FloorOneGoldMaxAmount", 550 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 0 },
                { "FloorTwoLootChests", 3 },
                { "FloorTwoGoldMaxAmount", 750 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 1 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                spawnCategoriesRule,
                startingCardsRule,
                piecesAdjustedRule,
                allowedCardsRule,
                statusEffectRule,
                pieceAbilityRule,
                pieceBehaviourListRule,
                pieceImmunityRule,
                pieceUseWhenKilledRule,
                abilityActionCostRule,
                abilityHealOverriddenRule,
                aoeAdjustedRule,
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
