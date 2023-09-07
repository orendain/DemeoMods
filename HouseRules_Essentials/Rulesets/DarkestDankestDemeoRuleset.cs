namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DarkestDankestDemeoRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Darkest Dankest Demeo";
            const string description = "Watch your step... Can you defeat the darkness?";
            const string longdesc = "Gain temporary vision range increases by defeating enemies\n";

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Torch, Property = "StartHealth", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "VisionRange", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinChieftan, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinRanger, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinFighter, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinMadUn, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMystic, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ScabRat, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileArcher, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileMutantWizard, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GeneralRonthian, Property = "WaterTrailChance", Value = 0.5f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenSpearman, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidArcher, Property = "WaterTrailChance", Value = 0.2f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidHoundMaster, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootHound, Property = "WaterTrailChance", Value = 0.3f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinChieftan, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinRanger, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinFighter, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GoblinMadUn, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenMystic, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenHound, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenCultist, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Rat, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Spider, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ScabRat, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileArcher, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ReptileMutantWizard, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.GeneralRonthian, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.ElvenSpearman, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidArcher, Property = "WaterTrailTiles", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.DruidHoundMaster, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootHound, Property = "WaterTrailChance", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "PreciseHealth", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "PreciseAttack", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "PowerIndex", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "PreciseHealth", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "PreciseAttack", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "StartHealth", Value = 35 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "StartArmor", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "AttackDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "PowerIndex", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SilentSentinel, Property = "BerserkBelowHealth", Value = .5f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "PreciseHealth", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "PreciseAttack", Value = 1 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "BerserkBelowHealth", Value = 0.99f },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartHealth", Value = 55 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "BarkArmor", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "AttackDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "MoveRange", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "PowerIndex", Value = 3 },
            });

            var myEntranceDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ElvenSpearman, 1 },
                { BoardPieceId.LargeCorruption, 2 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.SmallCorruption, 2 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 2 },
                { BoardPieceId.TheUnspoken, 2 },
                { BoardPieceId.Bandit, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 2 },
                { BoardPieceId.ScabRat, 1 },
                { BoardPieceId.Spider, 2 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.SporeFungus, 2 },
                { BoardPieceId.ElvenMystic, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 1 },
                { BoardPieceId.ChestGoblin, 1 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.IceElemental, 2 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.Gorgon, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myExitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ElvenSpearman, 2 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.LargeCorruption, 2 },
                { BoardPieceId.SmallCorruption, 2 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 2 },
                { BoardPieceId.TheUnspoken, 2 },
                { BoardPieceId.Bandit, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 1 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.ElvenMystic, 1 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 1 },
                { BoardPieceId.SporeFungus, 2 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.Gorgon, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myEntranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ElvenSpearman, 2 },
                { BoardPieceId.LargeCorruption, 2 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.SmallCorruption, 2 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 2 },
                { BoardPieceId.TheUnspoken, 2 },
                { BoardPieceId.Bandit, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 2 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.SporeFungus, 2 },
                { BoardPieceId.ElvenMystic, 2 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 2 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.IceElemental, 2 },
                { BoardPieceId.ElvenMarauder, 2 },
                { BoardPieceId.GiantSpider, 2 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.Gorgon, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myExitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ElvenSpearman, 2 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.LargeCorruption, 2 },
                { BoardPieceId.SmallCorruption, 2 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 2 },
                { BoardPieceId.TheUnspoken, 1 },
                { BoardPieceId.Bandit, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 2 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.ElvenMystic, 1 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 1 },
                { BoardPieceId.SporeFungus, 2 },
                { BoardPieceId.ChestGoblin, 1 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.IceElemental, 1 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.RootGolem, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.Gorgon, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myBossDeck = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.LargeCorruption, 1 },
                { BoardPieceId.SmallCorruption, 1 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 1 },
                { BoardPieceId.TheUnspoken, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 2 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 2 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.ElvenMystic, 1 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.ElvenMarauder, 2 },
                { BoardPieceId.IceElemental, 2 },
                { BoardPieceId.GiantSpider, 2 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.Gorgon, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myMonsterDeckConfig = new MyMonsterDeckOverriddenRule.MyDeckConfig
            {
                EntranceDeckFloor1 = myEntranceDeckFloor1,
                ExitDeckFloor1 = myExitDeckFloor1,
                EntranceDeckFloor2 = myEntranceDeckFloor2,
                ExitDeckFloor2 = myExitDeckFloor2,
                BossDeck = myBossDeck,
                KeyHolderFloor1 = BoardPieceId.SilentSentinel,
                KeyHolderFloor2 = BoardPieceId.Mimic,
            };
            var myMonsterDeckRule = new MyMonsterDeckOverriddenRule(myMonsterDeckConfig);

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingPush, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingSmash, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingTotem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PlayerLeap, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GuidingLight, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Implode, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MissileSwarm, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Deflect, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MagicMissile, ReplenishFrequency = 1 },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CourageShanty, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingVoice, ReplenishFrequency = 0 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingThrow, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Charge, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WarCry, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ReplenishArmor, ReplenishFrequency = 1 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Arrow, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Lure, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CursedDagger, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.FlashBomb, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Freeze, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Vortex, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Banish, ReplenishFrequency = 0 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroWarlock, warlockCards },
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
                { BoardPieceId.HeroBarbarian, barbarianCards },
            });

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyKnockbackMelee, AbilityKey.EarthShatter, AbilityKey.Grapple } },
                { BoardPieceId.Mimic, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.AcidSpit, AbilityKey.Grapple, AbilityKey.PlayerLeap, AbilityKey.EnemyFrostball, AbilityKey.Zap } },
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyStealGold, AbilityKey.Net } },
                { BoardPieceId.SilentSentinel, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.Leap, AbilityKey.Grab, AbilityKey.LightningBolt } },
                { BoardPieceId.ElvenArcher, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyArrowSnipe, AbilityKey.EnemyFrostball } },
                { BoardPieceId.ElvenQueen, new List<AbilityKey> { AbilityKey.SummonBossMinions, AbilityKey.LightningBolt, AbilityKey.Shockwave, AbilityKey.EnemyFrostball } },
                { BoardPieceId.GoblinFighter, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyFlashbang } },
                { BoardPieceId.Rat, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.DiseasedBite } },
                { BoardPieceId.Spider, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.DiseasedBite } },
                { BoardPieceId.GiantSpider, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.SpiderWebshot } },
                { BoardPieceId.Cavetroll, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.ElvenKingMeleeWhip } },
                { BoardPieceId.ScabRat, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.DiseasedBite } },
            });

            var pieceBehaviourListRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.EarthElemental, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.EarthShatter, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Mimic, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.ChestGoblin, new List<Behaviour> { Behaviour.Patrol, Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackAndRetreat, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.SilentSentinel, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedSpellCaster } },
                { BoardPieceId.ElvenArcher, new List<Behaviour> { Behaviour.Patrol, Behaviour.RangedSpellCaster, Behaviour.FollowPlayerRangedAttacker } },
                { BoardPieceId.GoblinFighter, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.GiantSpider, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Cavetroll, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.ScabRat, new List<Behaviour> { Behaviour.Patrol, Behaviour.Swarm, Behaviour.AttackPlayer } },
            });

            var piecePieceTypeRule = new PiecePieceTypeListOverriddenRule(new Dictionary<BoardPieceId, List<PieceType>>
            {
                { BoardPieceId.Mimic, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ReptileArcher, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Reptile, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ReptileMutantWizard, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Reptile, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.GeneralRonthian, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.RootBeast, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.RootCreeper, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.RootHound, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.RootVine, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.DruidArcher, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.DruidHoundMaster, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.ElvenArcher, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ElvenHound, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.Canine, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.TheUnspoken, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.GoblinChieftan, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Goblin, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.GoblinMadUn, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Goblin, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ScabRat, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Rat, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Spider, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Rat, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.Rat, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.TheUnheard, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Rat, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Slimeling, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.SmallSlime } },
                { BoardPieceId.Thug, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Thief, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ElvenMystic, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.NonTeleportable } },
                { BoardPieceId.ElvenPriest, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.ElvenSkirmisher, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ElvenSpearman, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.EarthElemental, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.Cavetroll, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.BigBoiMutant, new List<PieceType> { PieceType.GiantSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Reptile, PieceType.ValidCorruptionTarget, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.SilentSentinel, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.ServantOfAlfaragh, new List<PieceType> { PieceType.GiantSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.NonTeleportable, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.GiantSlime, new List<PieceType> { PieceType.GiantSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.GiantSlime, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.GiantSpider, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.ElvenMarauder, new List<PieceType> { PieceType.GiantSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Gorgon, new List<PieceType> { PieceType.GiantSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Sigataur, new List<PieceType> { PieceType.GiantSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.IceElemental, new List<PieceType> { PieceType.SmallSlime, PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Brittle } },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyDropStolenGoods, AbilityKey.DropChest } },
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.Explosion } },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Torch,
                    durationTurns = 5,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 4,
                    damagePerTurn = 0,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
            });

            var lampTypesRule = new LampTypesOverriddenRule(new Dictionary<int, List<BoardPieceId>>
            {
                {
                    1, new List<BoardPieceId>
                    {
                        BoardPieceId.SporeFungus,
                        BoardPieceId.SporeFungus,
                        BoardPieceId.VortexLamp,
                        BoardPieceId.WaterLamp,
                        BoardPieceId.IceLamp,
                    }
                },
                {
                    2, new List<BoardPieceId>
                    {
                        BoardPieceId.SporeFungus,
                        BoardPieceId.SporeFungus,
                        BoardPieceId.VortexLamp,
                        BoardPieceId.WaterLamp,
                        BoardPieceId.IceLamp,
                    }
                },
                {
                    3, new List<BoardPieceId>
                    {
                        BoardPieceId.SporeFungus,
                        BoardPieceId.SporeFungus,
                        BoardPieceId.VortexLamp,
                        BoardPieceId.WaterLamp,
                        BoardPieceId.IceLamp,
                    }
                },
            });

            var tileEffectDuration = new TileEffectDurationOverriddenRule(new Dictionary<TileEffect, int>
            {
                { TileEffect.Gas, 3 },
                { TileEffect.Acid, 15 },
                { TileEffect.Web, 3 },
                { TileEffect.Water, 4 },
                { TileEffect.Corruption, 5 },
                { TileEffect.Target, 0 },
            });

            var darknessRule = new DarknessRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroGuardian, 1 },
                { BoardPieceId.HeroHunter, 1 },
                { BoardPieceId.HeroBard, 1 },
                { BoardPieceId.HeroBarbarian, 1 },
                { BoardPieceId.HeroRogue, 1 },
                { BoardPieceId.HeroWarlock, 1 },
                { BoardPieceId.HeroSorcerer, 1 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                piecesAdjustedRule,
                myMonsterDeckRule,
                startingCardsRule,
                pieceAbilityRule,
                pieceBehaviourListRule,
                piecePieceTypeRule,
                pieceUseWhenKilledRule,
                statusEffectRule,
                lampTypesRule,
                tileEffectDuration,
                darknessRule);
        }
    }
}
