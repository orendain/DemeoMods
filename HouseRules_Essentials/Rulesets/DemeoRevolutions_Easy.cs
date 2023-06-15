﻿namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using Boardgame.Board;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class DemeoRevolutions_Easy
    {
        internal static Ruleset Create()
        {
            const string name = "Demeo Revolutions -<(EASY)>-";
            const string description = "Everything that has a beginning has an EASY ending.";

            var piecesAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Torch, Property = "StartHealth", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SellswordArbalestierActive, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SellswordArbalestierActive, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.PoisonousRat, Property = "StartHealth", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBarbarian, Property = "CriticalHitDamage", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "CriticalHitDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroWarlock, Property = "CriticalHitDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "AttackDamage", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "CriticalHitDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "CriticalHitDamage", Value = 7 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "CriticalHitDamage", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "CriticalHitDamage", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "AttackDamage", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "MoveRange", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "MoveRange", Value = 10 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WarlockMinion, Property = "ActionPoint", Value = 3 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "StartHealth", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Barricade, Property = "StartHealth", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Lure, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "StartHealth", Value = 30 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "MoveRange", Value = 4 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "AttackDamage", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.EarthElemental, Property = "PowerIndex", Value = 2 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Wyvern, Property = "PowerIndex", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Mimic, Property = "PowerIndex", Value = 9 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.RootGolem, Property = "PowerIndex", Value = 6 },
            });

            /*var spawnCategoriesRule = new SpawnCategoryOverriddenRule(new Dictionary<BoardPieceId, List<int>>
            {
                { BoardPieceId.ScarabSandPile, new List<int> { 1, 1, 1 } },
                { BoardPieceId.LargeCorruption, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ReptileArcher, new List<int> { 3, 1, 1 } },
                { BoardPieceId.ReptileMutantWizard, new List<int> { 2, 1, 1 } },
                { BoardPieceId.GoldSandPile, new List<int> { 2, 1, 1 } },
                { BoardPieceId.SmallCorruption, new List<int> { 3, 2, 1 } },
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
                { BoardPieceId.SpiderEgg, new List<int> { 3, 1, 1 } },
                { BoardPieceId.SporeFungus, new List<int> { 3, 1, 1 } },
                { BoardPieceId.RatNest, new List<int> { 3, 1, 1 } },
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
                { BoardPieceId.ServantOfAlfaragh, new List<int> { 2, 1, 1 } },
                { BoardPieceId.EmptySandPile, new List<int> { 3, 1, 1 } },
            });*/

            var myEntranceDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ScarabSandPile, 1 },
                { BoardPieceId.EmptySandPile, 2 },
                { BoardPieceId.ScorpionSandPile, 1 },
                { BoardPieceId.LargeCorruption, 1 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.GoldSandPile, 1 },
                { BoardPieceId.SmallCorruption, 1 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.TheUnseen, 2 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 2 },
                { BoardPieceId.RootHound, 2 },
                { BoardPieceId.TheUnspoken, 2 },
                { BoardPieceId.Bandit, 2 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 1 },
                { BoardPieceId.Spider, 2 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.SpiderEgg, 1 },
                { BoardPieceId.RatNest, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 2 },
                { BoardPieceId.ElvenMystic, 1 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 2 },
                { BoardPieceId.CultMemberElder, 2 },
                { BoardPieceId.RootMage, 1 },
                { BoardPieceId.KillerBee, 2 },
                { BoardPieceId.ChestGoblin, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.FireElemental, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.IceElemental, 1 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.SilentSentinel, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myExitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ScarabSandPile, 1 },
                { BoardPieceId.ScorpionSandPile, 1 },
                { BoardPieceId.ElvenSpearman, 2 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.SmallCorruption, 1 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.TheUnseen, 2 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 1 },
                { BoardPieceId.Bandit, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 2 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 1 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 1 },
                { BoardPieceId.SpiderEgg, 1 },
                { BoardPieceId.RatNest, 1 },
                { BoardPieceId.CultMemberElder, 1 },
                { BoardPieceId.RootMage, 1 },
                { BoardPieceId.KillerBee, 1 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.RootGolem, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.SilentSentinel, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myEntranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ScarabSandPile, 1 },
                { BoardPieceId.EmptySandPile, 1 },
                { BoardPieceId.ScorpionSandPile, 1 },
                { BoardPieceId.ElvenSpearman, 2 },
                { BoardPieceId.LargeCorruption, 1 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.GoldSandPile, 1 },
                { BoardPieceId.SmallCorruption, 1 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.TheUnseen, 2 },
                { BoardPieceId.ElvenArcher, 2 },
                { BoardPieceId.ElvenHound, 2 },
                { BoardPieceId.RootHound, 2 },
                { BoardPieceId.TheUnspoken, 2 },
                { BoardPieceId.Bandit, 2 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 2 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.SpiderEgg, 1 },
                { BoardPieceId.RatNest, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 2 },
                { BoardPieceId.ElvenMystic, 2 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 2 },
                { BoardPieceId.CultMemberElder, 2 },
                { BoardPieceId.RootMage, 1 },
                { BoardPieceId.KillerBee, 2 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.FireElemental, 1 },
                { BoardPieceId.IceElemental, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.SilentSentinel, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myExitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ScarabSandPile, 1 },
                { BoardPieceId.ScorpionSandPile, 1 },
                { BoardPieceId.ElvenSpearman, 2 },
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.SmallCorruption, 1 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.TheUnseen, 1 },
                { BoardPieceId.ElvenArcher, 2 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 1 },
                { BoardPieceId.Bandit, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 2 },
                { BoardPieceId.RootBeast, 1 },
                { BoardPieceId.ScabRat, 2 },
                { BoardPieceId.Spider, 1 },
                { BoardPieceId.Rat, 1 },
                { BoardPieceId.TheUnheard, 2 },
                { BoardPieceId.Slimeling, 1 },
                { BoardPieceId.Thug, 1 },
                { BoardPieceId.ElvenPriest, 1 },
                { BoardPieceId.ElvenSkirmisher, 1 },
                { BoardPieceId.GoblinFighter, 1 },
                { BoardPieceId.GoblinRanger, 1 },
                { BoardPieceId.CultMemberElder, 1 },
                { BoardPieceId.RootMage, 1 },
                { BoardPieceId.KillerBee, 2 },
                { BoardPieceId.ChestGoblin, 1 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Sigataur, 2 },
                { BoardPieceId.GiantSlime, 2 },
                { BoardPieceId.FireElemental, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.IceElemental, 1 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.RootGolem, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.SilentSentinel, 1 },
                { BoardPieceId.BigBoiMutant, 1 },
                { BoardPieceId.ServantOfAlfaragh, 1 },
            };
            var myBossDeck = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.ReptileArcher, 1 },
                { BoardPieceId.ReptileMutantWizard, 1 },
                { BoardPieceId.GeneralRonthian, 1 },
                { BoardPieceId.TheUnseen, 1 },
                { BoardPieceId.ElvenArcher, 1 },
                { BoardPieceId.ElvenHound, 1 },
                { BoardPieceId.RootHound, 1 },
                { BoardPieceId.TheUnspoken, 1 },
                { BoardPieceId.DruidArcher, 1 },
                { BoardPieceId.DruidHoundMaster, 1 },
                { BoardPieceId.GoblinChieftan, 1 },
                { BoardPieceId.GoblinMadUn, 2 },
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
                { BoardPieceId.CultMemberElder, 2 },
                { BoardPieceId.RootMage, 1 },
                { BoardPieceId.KillerBee, 1 },
                { BoardPieceId.EarthElemental, 1 },
                { BoardPieceId.Sigataur, 1 },
                { BoardPieceId.GiantSlime, 1 },
                { BoardPieceId.FireElemental, 1 },
                { BoardPieceId.ElvenMarauder, 1 },
                { BoardPieceId.IceElemental, 1 },
                { BoardPieceId.GiantSpider, 1 },
                { BoardPieceId.Cavetroll, 1 },
                { BoardPieceId.RootGolem, 1 },
                { BoardPieceId.Brookmare, 1 },
                { BoardPieceId.Gorgon, 1 },
                { BoardPieceId.SilentSentinel, 1 },
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
                KeyHolderFloor1 = BoardPieceId.Mimic,
                KeyHolderFloor2 = BoardPieceId.Wyvern,
            };
            var myMonsterDeckRule = new MyMonsterDeckOverriddenRule(myMonsterDeckConfig);

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grapple, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingPush, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingSmash, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.GrapplingTotem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PlayerLeap, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ExplodingLampPlaceholder, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Net, ReplenishFrequency = 3 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.MinionCharge, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
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
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SongOfRecovery, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.ShatteringVoice, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PiercingVoice, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.EnemyFlashbang, ReplenishFrequency = 3 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Grab, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
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
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HailOfArrows, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Lure, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.EnemyFireball, ReplenishFrequency = 1 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonBomb, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CursedDagger, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.FlashBomb, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.DiseasedBite, ReplenishFrequency = 3 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Zap, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Torch, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Freeze, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Vortex, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Banish, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
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

            /*var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Teleportation,
                        AbilityKey.WaterBottle,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
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
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Teleportation,
                        AbilityKey.WaterBottle,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
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
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Teleportation,
                        AbilityKey.WaterBottle,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
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
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Teleportation,
                        AbilityKey.WaterBottle,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.BeastWhisperer,
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
                        AbilityKey.HailOfArrows,
                        AbilityKey.CallCompanion,
                        AbilityKey.Lure,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Teleportation,
                        AbilityKey.WaterBottle,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.OneMoreThing,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
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
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Teleportation,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
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
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.Regroup,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.HealingPotion,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.WaterBottle,
                        AbilityKey.EnergyPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.LuckPotion,
                        AbilityKey.MagicPotion,
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
            });*/

            var allowedChestCardsRule = new CardChestAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.DetectEnemies,
                        AbilityKey.HealingPotion,
                        AbilityKey.Teleportation,
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
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
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
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
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
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
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.ScrollOfCharm,
                        AbilityKey.BeastWhisperer,
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
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
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
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.MagicPotion,
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
                        AbilityKey.Rejuvenation,
                        AbilityKey.DamageResistPotion,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.VigorPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.HeavensFury,
                        AbilityKey.MagicPotion,
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

            var allowedEnergyCardsRule = new CardEnergyAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.WaterBottle,
                        AbilityKey.StrengthPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.MarkOfVerga,
                    }
                },
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.WaterBottle,
                        AbilityKey.StrengthPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.WhirlwindAttack,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.BottleOfLye,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.WaterBottle,
                        AbilityKey.StrengthPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.ScrollOfCharm,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.WaterBottle,
                        AbilityKey.StrengthPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.HuntersMark,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.WaterBottle,
                        AbilityKey.StrengthPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.Blink,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.MagicPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.Fireball,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.EnergyPotion,
                        AbilityKey.Bone,
                        AbilityKey.WebBomb,
                        AbilityKey.Barricade,
                        AbilityKey.RepeatingBallista,
                        AbilityKey.OneMoreThing,
                        AbilityKey.PanicPowder,
                        AbilityKey.BottleOfLye,
                        AbilityKey.IceImmunePotion,
                        AbilityKey.FireImmunePotion,
                        AbilityKey.ScrollTsunami,
                        AbilityKey.Regroup,
                        AbilityKey.WaterBottle,
                        AbilityKey.MagicPotion,
                        AbilityKey.LuckPotion,
                        AbilityKey.ScrollElectricity,
                        AbilityKey.Portal,
                    }
                },
            });

            var allowedPotionsRule = new PotionAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                {
                    BoardPieceId.HeroBarbarian, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
                {
                    BoardPieceId.HeroBard, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
                {
                    BoardPieceId.HeroGuardian, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
                {
                    BoardPieceId.HeroHunter, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
                {
                    BoardPieceId.HeroRogue, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.StrengthPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
                {
                    BoardPieceId.HeroSorcerer, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.MagicPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
                {
                    BoardPieceId.HeroWarlock, new List<AbilityKey>
                    {
                        AbilityKey.DamageResistPotion,
                        AbilityKey.Rejuvenation,
                        AbilityKey.InvisibilityPotion,
                        AbilityKey.AdamantPotion,
                        AbilityKey.ExtraActionPotion,
                        AbilityKey.MagicPotion,
                        AbilityKey.SwiftnessPotion,
                        AbilityKey.VigorPotion,
                    }
                },
            });

            var statusEffectRule = new StatusEffectConfigRule(new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Torch,
                    durationTurns = 30,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 30,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.FireImmunity,
                    durationTurns = 18,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.IceImmunity,
                    durationTurns = 18,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.ExtraEnergy,
                    durationTurns = 3,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    healthBoost = 2,
                    applyAfterDissipate = EffectStateType.Thorns,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Netted,
                    durationTurns = 1,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.PlayerBerserk,
                    durationTurns = 1,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.Thorns,
                    durationTurns = 2,
                    damagePerTurn = 0,
                    stacks = false,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            });

            var pieceAbilityRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.ElvenMarauder, new List<AbilityKey> { AbilityKey.EnemyKnockbackMelee, AbilityKey.EnemyHeal, AbilityKey.Grab } },
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyKnockbackMelee, AbilityKey.EarthShatter } },
                { BoardPieceId.Mimic, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.AcidSpit, AbilityKey.Zap } },
                { BoardPieceId.RootMage, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.TeleportEnemy, AbilityKey.EnemyFlashbang } },
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyStealGold } },
                { BoardPieceId.KillerBee, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.ThornPowder } },
                { BoardPieceId.CultMemberElder, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.Weaken } },
                { BoardPieceId.Wyvern, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.DiseasedBite, AbilityKey.Leap } },
                { BoardPieceId.GoblinFighter, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.EnemyFlashbang } },
                { BoardPieceId.PoisonousRat, new List<AbilityKey> { AbilityKey.EnemyMelee, AbilityKey.DiseasedBite } },
            });

            var pieceBehaviourListRule = new PieceBehavioursListOverriddenRule(new Dictionary<BoardPieceId, List<Behaviour>>
            {
                { BoardPieceId.EarthElemental, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.EarthShatter, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Mimic, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.Wyvern, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.RootMage, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.CastOnTeam, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.KillerBee, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.ChestGoblin, new List<Behaviour> { Behaviour.Patrol, Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackAndRetreat, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.CultMemberElder, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackAndRetreat, Behaviour.RangedSpellCaster } },
                { BoardPieceId.GoblinFighter, new List<Behaviour> { Behaviour.Patrol, Behaviour.AttackPlayer, Behaviour.RangedAttackHighPrio } },
                { BoardPieceId.SandScorpion, new List<Behaviour> { Behaviour.Patrol, Behaviour.FollowPlayerMeleeAttacker, Behaviour.AttackAndRetreat } },
                { BoardPieceId.JeweledScarab, new List<Behaviour> { Behaviour.Patrol, Behaviour.FleeToFOW } },
            });

            var pieceImmunityRule = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>>
            {
                { BoardPieceId.HeroBarbarian, new List<EffectStateType> { EffectStateType.Petrified } },
                { BoardPieceId.HeroSorcerer, new List<EffectStateType> { EffectStateType.Stunned } },
                { BoardPieceId.HeroHunter, new List<EffectStateType> { EffectStateType.Frozen } },
                { BoardPieceId.Verochka, new List<EffectStateType> { EffectStateType.Frozen } },
                { BoardPieceId.HeroGuardian, new List<EffectStateType> { EffectStateType.Weaken1Turn, EffectStateType.Weaken2Turns } },
                { BoardPieceId.HeroBard, new List<EffectStateType> { EffectStateType.Diseased, EffectStateType.Blinded } },
                { BoardPieceId.HeroRogue, new List<EffectStateType> { EffectStateType.Tangled, EffectStateType.Netted } },
                { BoardPieceId.HeroWarlock, new List<EffectStateType> { EffectStateType.CorruptedRage, EffectStateType.Undefined } },
                { BoardPieceId.WarlockMinion, new List<EffectStateType> { EffectStateType.Blinded, EffectStateType.Disoriented, EffectStateType.Confused, EffectStateType.ConfusedPermanentVisualOnly, EffectStateType.Panic, EffectStateType.CorruptedRage, EffectStateType.Undefined } },
                { BoardPieceId.ElvenQueen, new List<EffectStateType> { EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Panic, EffectStateType.Blinded, EffectStateType.Confused, EffectStateType.Disoriented, EffectStateType.AbilityBlocked, EffectStateType.Netted } },
                { BoardPieceId.RatKing, new List<EffectStateType> { EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Panic, EffectStateType.Blinded, EffectStateType.Confused, EffectStateType.Diseased, EffectStateType.Disoriented, EffectStateType.AbilityBlocked, EffectStateType.Netted } },
                { BoardPieceId.RootLord, new List<EffectStateType> { EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Panic, EffectStateType.Blinded, EffectStateType.MarkOfAvalon, EffectStateType.Confused, EffectStateType.Disoriented, EffectStateType.AbilityBlocked, EffectStateType.Netted } },
                { BoardPieceId.MotherCy, new List<EffectStateType> { EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Panic, EffectStateType.Blinded, EffectStateType.MarkOfAvalon, EffectStateType.Confused, EffectStateType.Disoriented, EffectStateType.AbilityBlocked, EffectStateType.Netted } },
                { BoardPieceId.WizardBoss, new List<EffectStateType> { EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Panic, EffectStateType.Blinded, EffectStateType.MarkOfAvalon, EffectStateType.Diseased, EffectStateType.CorruptedRage, EffectStateType.Corruption, EffectStateType.Confused, EffectStateType.Disoriented, EffectStateType.AbilityBlocked, EffectStateType.Netted } },
                { BoardPieceId.BossTown, new List<EffectStateType> { EffectStateType.Stunned, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Panic, EffectStateType.Blinded, EffectStateType.MarkOfAvalon, EffectStateType.Confused, EffectStateType.Weaken1Turn, EffectStateType.ConfusedPermanentVisualOnly, EffectStateType.Petrified, EffectStateType.Disoriented, EffectStateType.AbilityBlocked, EffectStateType.Netted } },
            });

            var piecePieceTypeRule = new PiecePieceTypeListOverriddenRule(new Dictionary<BoardPieceId, List<PieceType>>
            {
                { BoardPieceId.Mimic, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ReptileArcher, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Reptile, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ReptileMutantWizard, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Reptile, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.GeneralRonthian, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.TheUnseen, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.ElvenArcher, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ElvenHound, new List<PieceType> { PieceType.Creature, PieceType.Canine, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.TheUnspoken, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.GoblinChieftan, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Goblin, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.GoblinMadUn, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Goblin, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ScabRat, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Rat, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Spider, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Rat, new List<PieceType> { PieceType.Creature, PieceType.Rat, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.TheUnheard, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Rat, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Slimeling, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.SmallSlime } },
                { BoardPieceId.Thug, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Thief, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.ElvenMystic, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.NonTeleportable } },
                { BoardPieceId.ElvenPriest, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.ElvenSkirmisher, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.EarthElemental, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature } },
                { BoardPieceId.Cavetroll, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.BigBoiMutant, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Reptile, PieceType.ValidCorruptionTarget, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.SilentSentinel, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.ServantOfAlfaragh, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.NonTeleportable, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.GiantSlime, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.GiantSlime, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.GiantSpider, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget, PieceType.TooHeavyToGrapple } },
                { BoardPieceId.ElvenMarauder, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.Gorgon, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.FireElemental, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.ValidCorruptionTarget } },
                { BoardPieceId.IceElemental, new List<PieceType> { PieceType.Creature, PieceType.ForestCreature, PieceType.DesertCreature, PieceType.Brittle } },
            });

            var applyEffectOnHitRule = new ApplyEffectOnHitAdjustedRule(new Dictionary<BoardPieceId, EffectStateType>
            {
                { BoardPieceId.HealingBeacon, EffectStateType.Diseased },
                { BoardPieceId.SwordOfAvalon, EffectStateType.Diseased },
                { BoardPieceId.Lure, EffectStateType.Thorns },
                { BoardPieceId.SmiteWard, EffectStateType.Diseased },
                { BoardPieceId.Verochka, EffectStateType.Panic },
                { BoardPieceId.HeroWarlock, EffectStateType.ExposeEnergy },
                { BoardPieceId.WarlockMinion, EffectStateType.ExposeEnergy },
                { BoardPieceId.Barricade, EffectStateType.Thorns },
                { BoardPieceId.EyeOfAvalon, EffectStateType.Confused },
                { BoardPieceId.Torch, EffectStateType.Panic },
                { BoardPieceId.SellswordArbalestierActive, EffectStateType.Panic },
            });

            var targetEffectRule = new AbilityTargetEffectsRule(new Dictionary<AbilityKey, List<EffectStateType>>
            {
                { AbilityKey.SigataurianJavelin, new List<EffectStateType> { EffectStateType.Weaken1Turn } },
                { AbilityKey.PVPBlink, new List<EffectStateType> { EffectStateType.Weaken1Turn, EffectStateType.Disoriented } },
                { AbilityKey.PanicPowderArrow, new List<EffectStateType> { EffectStateType.Panic, EffectStateType.Netted } },
                { AbilityKey.TurretDamageProjectile, new List<EffectStateType> { EffectStateType.Tangled } },
                { AbilityKey.EnemyTurretDamageProjectile, new List<EffectStateType> { EffectStateType.Tangled } },
                { AbilityKey.TurretHighDamageProjectile, new List<EffectStateType> { EffectStateType.Panic, EffectStateType.Blinded } },
                { AbilityKey.AcidSpit, new List<EffectStateType> { EffectStateType.Diseased } },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.ChestGoblin, new List<AbilityKey> { AbilityKey.EnemyDropStolenGoods, AbilityKey.DropChest } },
                { BoardPieceId.EarthElemental, new List<AbilityKey> { AbilityKey.Explosion } },
            });

            var abilityBreaksStealth = new AbilityBreaksStealthAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.PoisonBomb, false },
                { AbilityKey.FlashBomb, false },
                { AbilityKey.DiseasedBite, false },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
                { AbilityKey.LightningBolt, false },
                { AbilityKey.Sneak, false },
                { AbilityKey.Grab, false },
                { AbilityKey.Arrow, false },
                { AbilityKey.CourageShanty, false },
                { AbilityKey.MinionCharge, false },
                { AbilityKey.SpellPowerPotion, false },
                { AbilityKey.PVPBlink, false },
                { AbilityKey.WeakeningShout, false },
                { AbilityKey.LeapHeavy, false },
                { AbilityKey.SpawnRandomLamp, false },
                { AbilityKey.DeathBeam, false },
                { AbilityKey.FretsOfFire, false },
                { AbilityKey.Grapple, false },
                { AbilityKey.Net, true },
                { AbilityKey.ImplosionExplosionRain, false },
            });

            var abilityHealOverriddenRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.HealingPotion, 5 },
                { AbilityKey.Rejuvenation, 8 },
                { AbilityKey.AltarHeal, 8 },
                { AbilityKey.WaterBottle, 2 },
                { AbilityKey.TurretHealProjectile, 2 },
            });

            var abilityDamageAllRule = new AbilityDamageAllOverriddenRule(new Dictionary<AbilityKey, List<int>>
            {
                { AbilityKey.ShatteringVoice, new List<int> { 3, 6, 3, 6 } },
                { AbilityKey.PiercingVoice, new List<int> { 2, 4, 2, 4 } },
                { AbilityKey.Arrow, new List<int> { 3, 8, 3, 8 } },
                { AbilityKey.Electricity, new List<int> { 3, 5, 1, 2 } },
                { AbilityKey.PVPBlink, new List<int> { 9, 18, 9, 18 } },
                { AbilityKey.FretsOfFire, new List<int> { 3, 6, 3, 6 } },
                { AbilityKey.GrapplingPush, new List<int> { 2, 4, 2, 4 } },
                { AbilityKey.Petrify, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.WeakeningShout, new List<int> { 2, 2, 2, 2 } },
                { AbilityKey.LeapHeavy, new List<int> { 5, 5, 5, 5 } },
                { AbilityKey.ImplosionExplosionRain, new List<int> { 5, 5, 5, 5 } },
            });

            var turnOrderRule = new TurnOrderOverriddenRule(new TurnOrderOverriddenRule.Scores
            { Bard = 18, Guardian = 14, Warlock = 13, Sorcerer = 12, Barbarian = 11, Hunter = 10, Assassin = 9, Downed = -10, Javelin = 20, Deflect = 2, Mark = 7, Varga = 5 });

            var freeAbilityOnCritRule = new FreeAbilityOnCritRule(new Dictionary<BoardPieceId, AbilityKey>
            {
                { BoardPieceId.HeroHunter, AbilityKey.Bone },
                { BoardPieceId.HeroSorcerer, AbilityKey.WaterBottle },
                { BoardPieceId.HeroBard, AbilityKey.PanicPowder },
                { BoardPieceId.HeroBarbarian, AbilityKey.SpawnRandomLamp },
            });

            var freeHealOnHitRule = new FreeHealOnHitRule(new List<BoardPieceId> { BoardPieceId.HeroRogue, BoardPieceId.HeroWarlock });
            var freeHealOnCritRule = new FreeHealOnCritRule(new List<BoardPieceId> { BoardPieceId.HeroRogue, BoardPieceId.HeroWarlock, BoardPieceId.HeroBard });
            var freeActionPointsOnCritRule = new FreeActionPointsOnCritRule(new List<BoardPieceId> { BoardPieceId.HeroGuardian, BoardPieceId.HeroRogue });
            var freeReplenishablesOnCritRule = new FreeReplenishablesOnCritRule(new List<BoardPieceId> { BoardPieceId.HeroBarbarian, BoardPieceId.HeroBard, BoardPieceId.HeroRogue, BoardPieceId.HeroGuardian, BoardPieceId.HeroSorcerer, BoardPieceId.HeroHunter, BoardPieceId.HeroWarlock });
            var backstabConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId> { BoardPieceId.HeroBard, BoardPieceId.HeroRogue });
            var abilityBackstabRule = new AbilityBackstabAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.PiercingVoice, true },
                { AbilityKey.ShatteringVoice, true },
                { AbilityKey.DiseasedBite, true },
            });

            var abilityStealthDamageRule = new AbilityStealthDamageOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.DiseasedBite, 2 },
                { AbilityKey.PlayerMelee, 2 },
                { AbilityKey.FretsOfFire, 1 },
            });

            var enemyCooldownRule = new EnemyCooldownOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.Zap, 2 },
                { AbilityKey.LightningBolt, 3 },
                { AbilityKey.LeapHeavy, 2 },
                { AbilityKey.EnemyFrostball, 2 },
                { AbilityKey.Shockwave, 3 },
                { AbilityKey.EnemyFireball, 2 },
                { AbilityKey.EnemyFlashbang, 2 },
                { AbilityKey.Petrify, 2 },
                { AbilityKey.Net, 2 },
                { AbilityKey.Grapple, 2 },
                { AbilityKey.ElvenSummonerDeflect, 5 },
                { AbilityKey.PlayerLeap, 2 },
            });

            var aoeAdjustedRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.SongOfRecovery, 2 },
                { AbilityKey.SongOfResilience, 2 },
                { AbilityKey.FlashBomb, 1 },
                { AbilityKey.WarCry, 1 },
                { AbilityKey.WhirlwindAttack, 1 },
                { AbilityKey.Deflect, 2 },
                { AbilityKey.BlindingLight, 1 },
                { AbilityKey.BlockAbilities, 1 },
                { AbilityKey.LeapHeavy, 1 },
                { AbilityKey.Leap, 1 },
                { AbilityKey.Net, 0 },
            });

            var pieceExtraImmunitiesRule = new PieceExtraImmunitiesRule(true);
            var partyElectricityRule = new PartyElectricityDamageOverriddenRule(true);
            var petsFocusHuntersMarkRule = new PetsFocusHunterMarkRule(true);
            var enemyRespawnDisabledRule = new EnemyRespawnDisabledRule(true);
            var cardEnergyFromRecyclingRule = new CardEnergyFromRecyclingMultipliedRule(1.4f);
            var enemyHealthScaledRule = new EnemyHealthScaledRule(0.8f);
            var levelSequenceOverriddenRule = new LevelSequenceOverriddenRule(new List<string>
            {
                "ElvenFloor17",
                "SewersFloor08",
                "SewersFloor11",
                "ForestFloor02",
                "ForestFloor01",
                "ElvenFloor14",
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 1 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneMerchant", 0 },
                { "FloorOneLootChests", 4 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorOneSellswords", 1 },
                { "FloorOneVillagers", 1 },
                { "FloorTwoHealingFountains", 2 },
                { "FloorTwoPotionStand", 1 },
                { "FloorTwoMerchant", 1 },
                { "FloorTwoVillagers", 1 },
                { "FloorTwoLootChests", 4 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeHealingFountains", 1 },
                { "FloorThreePotionStand", 0 },
                { "FloorThreeMerchant", 0 },
                { "FloorThreeLootChests", 2 },
                { "FloorThreeElvenSummoners", 0 },
            });

            var abilityRandomPieceRule = new AbilityRandomPieceListRule(new Dictionary<AbilityKey, List<BoardPieceId>>
            {
                { AbilityKey.BeastWhisperer, new List<BoardPieceId> { BoardPieceId.PoisonousRat, BoardPieceId.Spider } },
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
                        BoardPieceId.IceLamp,
                    }
                },
                {
                    2, new List<BoardPieceId>
                    {
                        BoardPieceId.GasLamp,
                        BoardPieceId.OilLamp,
                        BoardPieceId.VortexLamp,
                        BoardPieceId.WaterLamp,
                        BoardPieceId.IceLamp,
                    }
                },
                {
                    3, new List<BoardPieceId>
                    {
                        BoardPieceId.GasLamp,
                        BoardPieceId.OilLamp,
                        BoardPieceId.VortexLamp,
                        BoardPieceId.WaterLamp,
                        BoardPieceId.IceLamp,
                    }
                },
            });

            var tileEffectDuration = new TileEffectDurationOverriddenRule(new Dictionary<TileEffect, int>
            {
                { TileEffect.Gas, 3 },
                { TileEffect.Acid, 2 },
                { TileEffect.Web, 3 },
                { TileEffect.Water, 4 },
                { TileEffect.Corruption, 5 },
                { TileEffect.Target, 0 },
            });

            /* This doesn't work right in multiplayer (yet)
            var merchantOverriddenRule = new MerchantOfferRarityOverriddenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.ScrollTsunami, 0 },
                { AbilityKey.SummonElemental, 0 },
                { AbilityKey.Teleportation, 0 },
                { AbilityKey.RepeatingBallista, 0 },
                { AbilityKey.ScrollElectricity, 0 },
                { AbilityKey.SwiftnessPotion, 0 },
                { AbilityKey.DamageResistPotion, 50 },
                { AbilityKey.InvisibilityPotion, 50 },
            });*/

            var statModifiersRule = new StatModifiersOverridenRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.ReplenishArmor, 4 },
            });

            var pieceExtraStatsRule = new PieceExtraStatsAdjustedRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroGuardian, 5 },
                { BoardPieceId.HeroHunter, 5 },
                { BoardPieceId.HeroBard, 5 },
                { BoardPieceId.HeroBarbarian, 5 },
                { BoardPieceId.HeroRogue, 5 },
                { BoardPieceId.HeroWarlock, 5 },
                { BoardPieceId.HeroSorcerer, 5 },
            });

            var pieceDamageResistRule = new PieceDamageResistRule(new List<BoardPieceId>
            {
                { BoardPieceId.WarlockMinion },
            });

            var pieceCounterDamageRule = new PieceCounterDamageRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroGuardian, 1 },
            });

            var pieceMagicStatsRule = new PieceMagicStatAddedRule(new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.HeroSorcerer, 1 },
            });

            var revolutionsRule = new RevolutionsRule(true);
            var goldPickupRule = new GoldPickedUpMultipliedRule(1.25f);
            var reviveEffectsRule = new ReviveRemovesEffectsRule(true);
            var courageShantyRule = new CourageShantyAddsHpRule(1);
            var tickRule = new TickAdjustedRule(true);
            var grappleUnhooked = new GrappleUnhookedRule(true);

            return Ruleset.NewInstance(
                name,
                description,
                tickRule,
                revolutionsRule,
                grappleUnhooked,
                pieceMagicStatsRule,
                pieceCounterDamageRule,
                pieceDamageResistRule,
                pieceExtraStatsRule,
                statModifiersRule,
                goldPickupRule,
                piecePieceTypeRule,
                piecesAdjustedRule,
                courageShantyRule,
                reviveEffectsRule,
                tileEffectDuration,
                myMonsterDeckRule,
                startingCardsRule,
                allowedChestCardsRule,
                allowedEnergyCardsRule,
                allowedPotionsRule,
                statusEffectRule,
                pieceAbilityRule,
                pieceBehaviourListRule,
                pieceImmunityRule,
                applyEffectOnHitRule,
                targetEffectRule,
                pieceUseWhenKilledRule,
                abilityBreaksStealth,
                abilityActionCostRule,
                abilityHealOverriddenRule,
                backstabConfigRule,
                turnOrderRule,
                freeHealOnHitRule,
                freeHealOnCritRule,
                freeReplenishablesOnCritRule,
                freeActionPointsOnCritRule,
                freeAbilityOnCritRule,
                abilityBackstabRule,
                abilityStealthDamageRule,
                enemyCooldownRule,
                aoeAdjustedRule,
                abilityDamageAllRule,
                partyElectricityRule,
                pieceExtraImmunitiesRule,
                petsFocusHuntersMarkRule,
                enemyRespawnDisabledRule,
                cardEnergyFromRecyclingRule,
                enemyHealthScaledRule,
                abilityRandomPieceRule,
                lampTypesRule,
                levelSequenceOverriddenRule,
                levelPropertiesRule);
        }
    }
}