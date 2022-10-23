namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DemeoRevolutions
    {
        internal static Ruleset Create()
        {
            const string name = "Demeo Revolutions";
            const string description = "Everything that has a beginning has an end.";

            var spawnCategoriesRule = new SpawnCategoryOverriddenRule(new Dictionary<BoardPieceId, List<int>>
            {
                { BoardPieceId.ScarabSandPile, new List<int> { 0, 1, 2 } },
                { BoardPieceId.LargeCorruption, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ReptileArcher, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ReptileMutantWizard, new List<int> { 3, 1, 1 } },
                { BoardPieceId.SandScorpion, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ServantOfAlfaragh, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ScorpionSandPile, new List<int> { 3, 2, 1 } },
                { BoardPieceId.GoldSandPile, new List<int> { 2, 1, 1 } },
                { BoardPieceId.SmallCorruption, new List<int> { 4, 2, 1 } },
                { BoardPieceId.GeneralRonthian, new List<int> { 3, 1, 1 } },
                { BoardPieceId.TheUnseen, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ElvenArcher, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenHound, new List<int> { 4, 2, 1 } },
                { BoardPieceId.RootHound, new List<int> { 4, 2, 1 } },
                { BoardPieceId.TheUnspoken, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ElvenCultist, new List<int> { 4, 2, 1 } },
                { BoardPieceId.Bandit, new List<int> { 4, 1, 1 } },
                { BoardPieceId.DruidArcher, new List<int> { 4, 2, 1 } },
                { BoardPieceId.DruidHoundMaster, new List<int> { 4, 1, 1 } },
                { BoardPieceId.GoblinChieftan, new List<int> { 4, 1, 1 } },
                { BoardPieceId.GoblinMadUn, new List<int> { 4, 1, 1 } },
                { BoardPieceId.RootBeast, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ScabRat, new List<int> { 4, 1, 1 } },
                { BoardPieceId.Spider, new List<int> { 4, 2, 1 } },
                { BoardPieceId.Rat, new List<int> { 4, 2, 1 } },
                { BoardPieceId.TheUnheard, new List<int> { 4, 2, 1 } },
                { BoardPieceId.Slimeling, new List<int> { 4, 1, 1 } },
                { BoardPieceId.Thug, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenMystic, new List<int> { 4, 2, 1 } },
                { BoardPieceId.ElvenPriest, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ElvenSkirmisher, new List<int> { 4, 2, 1 } },
                { BoardPieceId.GoblinFighter, new List<int> { 4, 2, 1 } },
                { BoardPieceId.GoblinRanger, new List<int> { 4, 2, 1 } },
                { BoardPieceId.SpiderEgg, new List<int> { 4, 1, 1 } },
                { BoardPieceId.SporeFungus, new List<int> { 4, 1, 1 } },
                { BoardPieceId.RatNest, new List<int> { 4, 1, 1 } },
                { BoardPieceId.CultMemberElder, new List<int> { 4, 1, 1 } },
                { BoardPieceId.RootMage, new List<int> { 4, 2, 1 } },
                { BoardPieceId.KillerBee, new List<int> { 4, 1, 1 } },
                { BoardPieceId.ChestGoblin, new List<int> { 2, 1, 1 } },
                { BoardPieceId.EarthElemental, new List<int> { 2, 1, 1 } },
                { BoardPieceId.Sigataur, new List<int> { 2, 1, 1 } },
                { BoardPieceId.GiantSlime, new List<int> { 2, 1, 1 } },
                { BoardPieceId.FireElemental, new List<int> { 2, 1, 1 } },
                { BoardPieceId.ElvenMarauder, new List<int> { 2, 1, 1 } },
                { BoardPieceId.IceElemental, new List<int> { 2, 1, 1 } },
                { BoardPieceId.GiantSpider, new List<int> { 2, 1, 1 } },
                { BoardPieceId.Mimic, new List<int> { 2, 1, 1 } },
                { BoardPieceId.Cavetroll, new List<int> { 2, 1, 2 } },
                { BoardPieceId.RootGolem, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Brookmare, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Gorgon, new List<int> { 2, 1, 2 } },
                { BoardPieceId.SilentSentinel, new List<int> { 2, 1, 2 } },
                { BoardPieceId.Wyvern, new List<int> { 2, 1, 2 } },
                { BoardPieceId.BigBoiMutant, new List<int> { 2, 1, 1 } },
                { BoardPieceId.EmptySandPile, new List<int> { 2, 1, 1 } },
            });

            /*var cardChestRarityRule = new CardChestRarityOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.Barricade, 15 },
                { AbilityKey.Bone, 15 },
                { AbilityKey.BottleOfLye, 15 },
                { AbilityKey.WaterBottle, 15 },
                { AbilityKey.AdamantPotion, 3 },
                { AbilityKey.HeavensFury, 3 },
                { AbilityKey.Rejuvenation, 3 },
                { AbilityKey.OilLamp, 0 },
                { AbilityKey.GasLamp, 0 },
                { AbilityKey.VortexLamp, 0 },
                { AbilityKey.IceLamp, 0 },
            });

            var cardShopRarityRule = new CardShopRarityOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.Barricade, 15 },
                { AbilityKey.Bone, 15 },
                { AbilityKey.BottleOfLye, 15 },
                { AbilityKey.WaterBottle, 15 },
                { AbilityKey.AdamantPotion, 5 },
                { AbilityKey.HeavensFury, 5 },
                { AbilityKey.Rejuvenation, 5 },
                { AbilityKey.OilLamp, 0 },
                { AbilityKey.GasLamp, 0 },
                { AbilityKey.VortexLamp, 0 },
                { AbilityKey.IceLamp, 0 },
            });*/

            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implode, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MissileSwarm, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Deflect, ReplenishFrequency = 0 },
                //new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Portal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MinionCharge, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
            };

            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingVoice, ReplenishFrequency = 0 },
                //new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BlockAbilities, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.EnemyFlashbang, ReplenishFrequency = 2 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Charge, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WarCry, ReplenishFrequency = 0 },
                //new StartCardsModifiedRule.CardConfig { Card = AbilityKey.TheBehemoth, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grab, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                //new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BeastWhisperer, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Lure, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.EnemyFireball, ReplenishFrequency = 1 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, ReplenishFrequency = 0 },
                //new StartCardsModifiedRule.CardConfig { Card = AbilityKey.BoobyTrap, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CursedDagger, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.FlashBomb, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DiseasedBite, ReplenishFrequency = 2 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Freeze, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Vortex, ReplenishFrequency = 0 },
                //new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SummonElemental, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Banish, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                // new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Overcharge, ReplenishFrequency = 1 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroWarlock, warlockCards },
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
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "AttackDamage", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "CriticalHitDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "AttackDamage", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "CriticalHitDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "CriticalHitDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "CriticalHitDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "CriticalHitDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "CriticalHitDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "BerserkBelowHealth", Value = 0.99f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "StartHealth", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "MoveRange", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "ActionPoint", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "StartHealth", Value = 8 },
                /*new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartArmor", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartHealth", Value = 35 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ChestGoblin, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ChestGoblin, Property = "StartHealth", Value = 16 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ChestGoblin, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "BarkArmor", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "AttackDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "StartHealth", Value = 59 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Barricade, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HealingBeacon, Property = "StartHealth", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Lure, Property = "StartHealth", Value = 27 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "StartHealth", Value = 17 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "StartHealth", Value = 17 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "AttackDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Tornado, Property = "ActionPoint", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootMage, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SnakeBoss, Property = "StartHealth", Value = 60 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SnakeTailBoss, Property = "StartHealth", Value = 60 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WizardBoss, Property = "StartHealth", Value = 222 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenQueen, Property = "StartHealth", Value = 150 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RatKing, Property = "StartHealth", Value = 175 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.MotherCy, Property = "StartHealth", Value = 110 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootLord, Property = "StartHealth", Value = 90 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RatNest, Property = "StartHealth", Value = 16 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SnakeBoss, Property = "AttackDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SnakeTailBoss, Property = "AttackDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WizardBoss, Property = "AttackDamage", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenQueen, Property = "AttackDamage", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RatKing, Property = "AttackDamage", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.MotherCy, Property = "AttackDamage", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootLord, Property = "AttackDamage", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "AttackDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "AttackDamage", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Cavetroll, Property = "AttackDamage", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Sigataur, Property = "AttackDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootGolem, Property = "AttackDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMarauder, Property = "AttackDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSlime, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSpider, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ScabRat, Property = "AttackDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinMadUn, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.TheUnseen, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EnemyTurret, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinRanger, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SandScorpion, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.OilLamp, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ProximityMine, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Tornado, Property = "AliveForRounds", Value = 2 },
                // new AttackDamage
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenArcher, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenPriest, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenSkirmisher, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinChieftan, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinFighter, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Slimeling, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.FireElemental, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.IceElemental, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Gorgon, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.TheUnspoken, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Bandit, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Thug, Property = "AttackDamage", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.KillerBee, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.CultMemberElder, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidHoundMaster, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidArcher, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootHound, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootMage, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.JeweledScarab, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileArcher, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.BigBoiMutant, Property = "AttackDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileMutantWizard, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GeneralRonthian, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ServantOfAlfaragh, Property = "AttackDamage", Value = 3 },
                // new StartHealth
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootGolem, Property = "StartHealth", Value = 32 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "StartHealth", Value = 58 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.JeweledScarab, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenArcher, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenPriest, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenSkirmisher, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinChieftan, Property = "StartHealth", Value = 16 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinFighter, Property = "StartHealth", Value = 11 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Slimeling, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.FireElemental, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.IceElemental, Property = "StartHealth", Value = 16 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Gorgon, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.TheUnspoken, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.TheUnheard, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.TheUnseen, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "StartHealth", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "StartHealth", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Bandit, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Thug, Property = "StartHealth", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.KillerBee, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.CultMemberElder, Property = "StartHealth", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidHoundMaster, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidArcher, Property = "StartHealth", Value = 17 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMystic, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootHound, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootMage, Property = "StartHealth", Value = 14 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.JeweledScarab, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileArcher, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.BigBoiMutant, Property = "StartHealth", Value = 44 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileMutantWizard, Property = "StartHealth", Value = 28 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GeneralRonthian, Property = "StartHealth", Value = 13 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ServantOfAlfaragh, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SandScorpion, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "StartHealth", Value = 78 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "StartHealth", Value = 80 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Cavetroll, Property = "StartHealth", Value = 86 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Sigataur, Property = "StartHealth", Value = 69 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootGolem, Property = "StartHealth", Value = 37 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "StartHealth", Value = 36 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMarauder, Property = "StartHealth", Value = 47 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSlime, Property = "StartHealth", Value = 50 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GiantSpider, Property = "StartHealth", Value = 58 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ScabRat, Property = "StartHealth", Value = 37 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinMadUn, Property = "StartHealth", Value = 31 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EnemyTurret, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinRanger, Property = "StartHealth", Value = 8 },*/
            });

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.HealingWard,
                        AbilityKey.WhirlwindAttack,
                        AbilityKey.WarCry,
                        AbilityKey.TheBehemoth,
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
                        AbilityKey.Charge,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.PiercingVoice,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.HurricaneAnthem,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.BlockAbilities,
                        AbilityKey.PiercingVoice,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.HurricaneAnthem,
                        AbilityKey.SongOfRecovery,
                        AbilityKey.SongOfResilience,
                        AbilityKey.PiercingVoice,
                        AbilityKey.ShatteringVoice,
                        AbilityKey.HurricaneAnthem,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.WebBomb,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.StrengthPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.BeastWhisperer,
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.HuntersMark,
                        AbilityKey.Lure,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.BeastWhisperer,
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.PoisonedTip,
                        AbilityKey.HuntersMark,
                        AbilityKey.Lure,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.BeastWhisperer,
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.HuntersMark,
                        AbilityKey.Lure,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.StrengthPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.LuckPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.BoobyTrap,
                        AbilityKey.FlashBomb,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.BoobyTrap,
                        AbilityKey.FlashBomb,
                        AbilityKey.Blink,
                        AbilityKey.PoisonBomb,
                        AbilityKey.CoinFlip,
                        AbilityKey.CursedDagger,
                        AbilityKey.FlashBomb,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.Regroup,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.Teleportation,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.MagicPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Banish,
                        AbilityKey.Fireball,
                        AbilityKey.Freeze,
                        AbilityKey.MagicShield,
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
                        AbilityKey.MagicShield,
                        AbilityKey.MagicBarrier,
                        AbilityKey.Vortex,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.PanicPowder,
                        AbilityKey.Barricade,
                        AbilityKey.BottleOfLye,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.HealingPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.MagicPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.WaterBottle,
                        AbilityKey.Deflect,
                        AbilityKey.GuidingLight,
                        AbilityKey.Implode,
                        AbilityKey.MissileSwarm,
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

            /*var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 20,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                    healPerTurn = 2,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Recovery,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                    healPerTurn = 2,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.MagicShield,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Deflect,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.FireImmunity,
                    durationTurns = 12,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.IceImmunity,
                    durationTurns = 12,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });*/

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyKnockbackMelee, AbilityKey.EarthShatter, AbilityKey.EnemyJavelin } },
                { BoardPieceId.Mimic, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.AcidSpit, AbilityKey.Grab } },
                { BoardPieceId.RootMage, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.TeleportEnemy } },
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyStealGold } },
                { BoardPieceId.KillerBee, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.ThornPowder } },
                { BoardPieceId.CultMemberElder, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.Weaken, AbilityKey.EnemyFireball } },
                { BoardPieceId.Wyvern, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.DiseasedBite, AbilityKey.LightningBolt, AbilityKey.LeapHeavy } },
                { BoardPieceId.SilentSentinel, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.LeapHeavy, AbilityKey.Grab } },
                { BoardPieceId.ElvenArcher, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyArrowSnipe, AbilityKey.EnemyFrostball } },
                { BoardPieceId.ElvenCultist, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.LeechMelee } },
                { BoardPieceId.TheUnseen, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.Zap } },
                { BoardPieceId.ElvenQueen, new List<AbilityKey> { AbilityKey.SummonBossMinions, AbilityKey.LightningBolt, AbilityKey.EarthShatter, AbilityKey.EnemyFireball } },
                { BoardPieceId.BigBoiMutant, new List<AbilityKey> { AbilityKey.EnemyKnockbackMelee, AbilityKey.Shockwave, AbilityKey.LeapHeavy } },
                { BoardPieceId.GoblinFighter, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyFlashbang } },
            });

            var pieceBehaviourListRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.EarthElemental, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.EarthShatter, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Mimic, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.RootMage, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.CastOnTeam } },
                { BoardPieceId.KillerBee, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.ChestGoblin, new List<Behaviour> { Behaviour.Patrol, Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackAndRetreat } },
                { BoardPieceId.CultMemberElder, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackAndRetreat, Behaviour.RangedSpellCaster } },
                { BoardPieceId.SilentSentinel, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedSpellCaster } },
                { BoardPieceId.ElvenArcher, new List<Behaviour> { Behaviour.Patrol, Behaviour.RangedSpellCaster, Behaviour.FollowPlayerRangedAttacker } },
                { BoardPieceId.TheUnseen, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedSpellCaster } },
                { BoardPieceId.GoblinFighter, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
            });

            var pieceImmunityRule = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>>
            {
                { BoardPieceId.HeroSorcerer, new List<EffectStateType> { EffectStateType.Stunned } },
                { BoardPieceId.HeroGuardian, new List<EffectStateType> { EffectStateType.Weaken } },
                //{ BoardPieceId.HeroBard, new List<EffectStateType> { EffectStateType.Diseased } },
                { BoardPieceId.HeroRogue, new List<EffectStateType> { EffectStateType.Tangled, EffectStateType.Blinded } },
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
                /*{ AbilityKey.LightningBolt, false },
                { AbilityKey.Overcharge, false },
                { AbilityKey.Overload, false },*/
                { AbilityKey.Sneak, false },
                { AbilityKey.Grab, false },
                { AbilityKey.Arrow, false },
                { AbilityKey.CourageShanty, false },
                { AbilityKey.MinionCharge, false },
                //{ AbilityKey.SpawnRandomLamp, false },
                { AbilityKey.SpellPowerPotion, false },
            });

            var abilityHealOverriddenRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.HealingPotion, 5 },
                { AbilityKey.Rejuvenation, 8 },
                { AbilityKey.AltarHeal, 8 },
                { AbilityKey.TurretHealProjectile, 2 },
            });

            var aoeAdjustedRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.SongOfRecovery, 2 },
                { AbilityKey.SongOfResilience, 2 },
                { AbilityKey.FlashBomb, 1 },
                //{ AbilityKey.WebBomb, 1 },
                //{ AbilityKey.PoisonBomb, 1 },
                { AbilityKey.WarCry, 1 },
                { AbilityKey.WhirlwindAttack, 1 },
                //{ AbilityKey.Deflect, 2 },
                //{ AbilityKey.PoisonGas, 1 },
                { AbilityKey.BlindingLight, 1 },
                { AbilityKey.BlockAbilities, 1 },
                //{ AbilityKey.EnemyFlashbang, 1 },
            });

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>>
            {
                { AbilityKey.PiercingVoice, new List<int> { 2, 4, 2, 4 } },
                { AbilityKey.Arrow, new List<int> { 3, 8, 3, 8 } },
                /*{ AbilityKey.ShatteringVoice, new List<int> { 4, 8, 4, 8 } },
                { AbilityKey.EnemyFireball, new List<int> { 7, 7, 7, 7 } },
                { AbilityKey.Zap, new List<int> { 2, 4, 2, 4 } },
                { AbilityKey.LightningBolt, new List<int> { 3, 6, 3, 6 } },
                { AbilityKey.Overload, new List<int> { 2, 4, 2, 4 } },
                { AbilityKey.Overcharge, new List<int> { 2, 4, 2, 4 } },
                { AbilityKey.Fireball, new List<int> { 12, 30, 6, 15 } },
                { AbilityKey.Freeze, new List<int> { 5, 15, 5, 15 } },
                { AbilityKey.Vortex, new List<int> { 3, 12, 2, 6 } },
                { AbilityKey.WhirlwindAttack, new List<int> { 5, 11, 5, 11 } },
                { AbilityKey.Charge, new List<int> { 5, 15, 5, 15 } },
                { AbilityKey.PiercingThrow, new List<int> { 6, 13, 6, 13 } },
                { AbilityKey.Blink, new List<int> { 8, 20, 8, 20 } },
                { AbilityKey.CursedDagger, new List<int> { 4, 12, 4, 12 } },
                { AbilityKey.PoisonedTip, new List<int> { 6, 16, 6, 16 } },
                { AbilityKey.HailOfArrows, new List<int> { 6, 16, 6, 16 } },
                { AbilityKey.Arrow, new List<int> { 4, 12, 4, 12 } },
                { AbilityKey.EnemyFrostball, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.Electricity, new List<int> { 3, 6, 1, 2 } },
                { AbilityKey.TurretDamageProjectile, new List<int> { 3, 3, 3, 3 } },
                { AbilityKey.TurretHighDamageProjectile, new List<int> { 6, 6, 3, 3 } },
                { AbilityKey.MinionCharge, new List<int> { 4, 9, 4, 9 } },
                { AbilityKey.MissileSwarm, new List<int> { 2, 2, 2, 2 } },
                { AbilityKey.MagicMissile, new List<int> { 2, 5, 2, 5 } },
                { AbilityKey.TornadoCharge, new List<int> { 3, 3, 3, 3 } },
                { AbilityKey.AcidSpit, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.EnemyTurretDamageProjectile, new List<int> { 2, 2, 2, 2 } },
                { AbilityKey.EnemyArrow, new List<int> { 3, 3, 3, 3 } },
                { AbilityKey.EnemyArrowSnipe, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.EnemyJavelin, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.EnemyPikeMeleeAttack, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.IceExplosion, new List<int> { 6, 6, 6, 6 } },
                { AbilityKey.LeapHeavy, new List<int> { 6, 6, 6, 6 } },
                { AbilityKey.Leap, new List<int> { 2, 2, 2, 2 } },
                { AbilityKey.Shockwave, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.Tsunami, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.ScrollTsunami, new List<int> { 5, 5, 5, 5 } },*/
            });

            var turnOrderRule = new TurnOrderOverriddenRule(new TurnOrderOverriddenRule.Scores
            { Bard = 15, Guardian = 14, Sorcerer = 13, Warlock = 12, Hunter = 11, Assassin = 10, Downed = -6, Javelin = 12 });

            var freeAbilityOnCritRule = new FreeAbilityOnCritRule(new Dictionary<BoardPieceId, AbilityKey>
            {
                { BoardPieceId.HeroHunter, AbilityKey.Bone },
                { BoardPieceId.HeroSorcerer, AbilityKey.WaterBottle },
                { BoardPieceId.HeroWarlock, AbilityKey.SpellPowerPotion },
                { BoardPieceId.HeroBard, AbilityKey.PanicPowder },
            });

            var freeHealOnHitRule = new FreeHealOnHitRule(new List<BoardPieceId> { BoardPieceId.HeroRogue, BoardPieceId.HeroWarlock, BoardPieceId.HeroBard });
            var freeHealOnCritRule = new FreeHealOnCritRule(new List<BoardPieceId> { BoardPieceId.HeroRogue, BoardPieceId.HeroWarlock, BoardPieceId.HeroBard });
            var freeActionPointsOnCritRule = new FreeActionPointsOnCritRule(new List<BoardPieceId> { BoardPieceId.HeroGuardian, BoardPieceId.HeroRogue });
            var freeRepleishablesOnCritRule = new FreeReplenishablesOnCritRule(new List<BoardPieceId> { BoardPieceId.HeroBard, BoardPieceId.HeroRogue, BoardPieceId.HeroGuardian, BoardPieceId.HeroSorcerer, BoardPieceId.HeroHunter, BoardPieceId.HeroWarlock });
            var backstabConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId> { BoardPieceId.HeroBard, BoardPieceId.HeroRogue });
            var abilityBackstabRule = new AbilityBackstabAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.PoisonBomb, true },
                { AbilityKey.PiercingVoice, true },
                { AbilityKey.ShatteringVoice, true },
                { AbilityKey.DiseasedBite, true },
            });

            var abilityStealthDamageRule = new AbilityStealthDamageOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.DiseasedBite, 1 },
                { AbilityKey.PoisonBomb, 1 },
                { AbilityKey.PlayerMelee, 2 },
            });

            var enemyCooldownRule = new EnemyCooldownOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.Zap, 2 },
                { AbilityKey.LightningBolt, 2 },
                { AbilityKey.LeapHeavy, 2 },
                { AbilityKey.EnemyFrostball, 2 },
                { AbilityKey.EnemyFireball, 2 },
                { AbilityKey.EnemyFlashbang, 2 },
                { AbilityKey.Petrify, 2 },
            });

            var pieceExtraImmunities = new PieceExtraImmunitiesRule(true);
            var partyElectricity = new PartyElectricityDamageOverriddenRule(true);
            var petsFocusHuntersMarkRule = new PetsFocusHunterMarkRule(true);
            var enemyRespawnDisabledRule = new EnemyRespawnDisabledRule(true);
            var cardEnergyFromAttackRule = new CardEnergyFromAttackMultipliedRule(0.75f);
            var cardEnergyFromRecyclingRule = new CardEnergyFromRecyclingMultipliedRule(0.66f);
            //var enemyAttackScaledRule = new EnemyAttackScaledRule(1.83f);
            //var enemyHealthScaledRule = new EnemyHealthScaledRule(2.334f);
            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "ElvenFloor17",
                "SewersFloor08",
                "SewersFloor11",
                "ForestFloor02",
                "ForestFloor01",
                "ElvenFloor14",
                "fixhydra",
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "BigGoldPileChance", 30 },
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 0 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 2 },
                { "FloorOneGoldMaxAmount", 500 },
                { "FloorTwoHealingFountains", 1 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoLootChests", 3 },
                { "FloorTwoGoldMaxAmount", 600 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 1 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                spawnCategoriesRule,
                /*cardChestRarityRule,
                cardShopRarityRule,*/
                startingCardsRule,
                piecesAdjustedRule,
                allowedCardsRule,
                //statusEffectRule,
                pieceAbilityRule,
                pieceBehaviourListRule,
                pieceImmunityRule,
                pieceUseWhenKilledRule,
                abilityActionCostRule,
                abilityHealOverriddenRule,
                aoeAdjustedRule,
                //abilityDamageRule,
                backstabConfigRule,
                turnOrderRule,
                freeHealOnHitRule,
                freeHealOnCritRule,
                freeRepleishablesOnCritRule,
                freeActionPointsOnCritRule,
                freeAbilityOnCritRule,
                abilityBackstabRule,
                abilityStealthDamageRule,
                enemyCooldownRule,
                pieceExtraImmunities,
                partyElectricity,
                petsFocusHuntersMarkRule,
                enemyRespawnDisabledRule,
                cardEnergyFromAttackRule,
                cardEnergyFromRecyclingRule,
                //enemyAttackScaledRule,
                //enemyHealthScaledRule,
                levelSequenceOverriddenRule,
                levelPropertiesRule);
        }
    }
}
