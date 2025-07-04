﻿namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class EarthWindAndFire
    {
        internal static Ruleset Create()
        {
            const string name = "Earth Wind & Fire";
            const string description = "Not the band. Let's get Elemental";
            const string longdesc = "";

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>> { { AbilityKey.Zap, new List<int> { 2, 5 } } });

            var barbarianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var warlockCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Heal, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Tornado, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Stealth, ReplenishFrequency = 0 },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroBarbarian, barbarianCards },
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
                { BoardPieceId.HeroBard, bardCards },
                { BoardPieceId.HeroWarlock, warlockCards },
            });

            var allowedCards = new List<AbilityKey>
            {
                AbilityKey.Bone,
                AbilityKey.Fireball,
                AbilityKey.Teleport,
                AbilityKey.RevealPath,
                AbilityKey.Freeze,
                AbilityKey.Strength,
                AbilityKey.Speed,
                AbilityKey.DamageResistPotion,
                AbilityKey.VigorPotion,
                AbilityKey.ExtraActionPotion,
                AbilityKey.CallCompanion,
                AbilityKey.Whirlwind,
                AbilityKey.LetItRain,
                AbilityKey.WaterDive,
                AbilityKey.GodsFury,
                AbilityKey.DamageResistPotion,
                AbilityKey.Heal,
                AbilityKey.WoodBone,
                AbilityKey.TorchLight,
            };
            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBarbarian, allowedCards },
                { BoardPieceId.HeroGuardian, allowedCards },
                { BoardPieceId.HeroHunter, allowedCards },
                { BoardPieceId.HeroRogue, allowedCards },
                { BoardPieceId.HeroSorcerer, allowedCards },
                { BoardPieceId.HeroBard, allowedCards },
                { BoardPieceId.HeroWarlock, allowedCards },
            });

            var pieceConfigAdjustedRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartArmor", Value = 0 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Sigataur, Property = "StartHealth", Value = 38 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "StartHealth", Value = 75 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "StartArmor", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "AttackDamage", Value = 3 },
            });

            var backstabPieceConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId>
            {
                BoardPieceId.HeroBarbarian,
                BoardPieceId.HeroGuardian,
                BoardPieceId.HeroSorcerer,
                BoardPieceId.HeroRogue,
                BoardPieceId.HeroBard,
                BoardPieceId.HeroWarlock,
            });

            var abilityBackstabRule = new AbilityBackstabAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, true },
                { AbilityKey.HunterArrow, true },
                { AbilityKey.PiercingSpear, true },
                { AbilityKey.PoisonedTip, true },
                { AbilityKey.Fireball, true },
                { AbilityKey.Freeze, true },
                { AbilityKey.Bone, true },
                { AbilityKey.Whirlwind, true },
                { AbilityKey.Grapple, true },
                { AbilityKey.GrapplingPush, true },
            });

            var entranceDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 }, // Unlimited spiders.
                { BoardPieceId.IceElemental, 2 },
                { BoardPieceId.ChestGoblin, 3 },
                { BoardPieceId.FireElemental, 2 },
            };
            var exitDeckFloor1 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Rat, 20 },
                { BoardPieceId.Spider, 20 },
                { BoardPieceId.IceElemental, 2 },
                { BoardPieceId.ChestGoblin, 3 },
                { BoardPieceId.Mimic, 1 },
                { BoardPieceId.GoblinMadUn, 1 },
                { BoardPieceId.DruidArcher, 1 },
            };
            var entranceDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 0 },
                { BoardPieceId.GoblinFighter, 0 },
                { BoardPieceId.ScabRat, 2 },
                { BoardPieceId.SporeFungus, 5 },
                { BoardPieceId.SpiderEgg, 3 },
                { BoardPieceId.FireElemental, 2 },
                { BoardPieceId.ElvenArcher, 2 },
            };
            var exitDeckFloor2 = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.Spider, 20 },
                { BoardPieceId.Rat, 30 },
                { BoardPieceId.Bandit, 2 },
                { BoardPieceId.Mimic, 5 },
                { BoardPieceId.ElvenPriest, 4 },
                { BoardPieceId.ElvenMarauder, 2 },
            };
            var bossDeck = new Dictionary<BoardPieceId, int>
            {
                { BoardPieceId.SpiderEgg, 10 },
                { BoardPieceId.TheUnseen, 0 },
                { BoardPieceId.TheUnheard, 0 },
                { BoardPieceId.TheUnspoken, 0 },
                { BoardPieceId.Slimeling, 0 },
                { BoardPieceId.Sigataur, 2 },
            };
            var monsterDeckConfig = new MonsterDeckOverriddenRule.DeckConfig
            {
                EntranceDeckFloor1 = entranceDeckFloor1,
                ExitDeckFloor1 = exitDeckFloor1,
                EntranceDeckFloor2 = entranceDeckFloor2,
                ExitDeckFloor2 = exitDeckFloor2,
                BossDeck = bossDeck,
                KeyHolderFloor1 = BoardPieceId.Cavetroll,
                KeyHolderFloor2 = BoardPieceId.Sigataur,
                Boss = BoardPieceId.Brookmare,
            };

            var monsterDeckRule = new MonsterDeckOverriddenRule(monsterDeckConfig);

            var abilityAoeRule = new AbilityAoeAdjustedRule(new Dictionary<AbilityKey, int>
            {
                { AbilityKey.StrengthenCourage, 1 },
                { AbilityKey.ReplenishArmor, 1 },
                { AbilityKey.Strength, 1 },
                { AbilityKey.Speed, 1 },
                { AbilityKey.Antidote, 1 },
                { AbilityKey.DamageResistPotion, 1 },
                { AbilityKey.VigorPotion, 1 },
                { AbilityKey.ExtraActionPotion, 1 },
                { AbilityKey.Invulnerability, 1 },
                { AbilityKey.Heal, 1 },
                { AbilityKey.FreeAP, 1 },
            });

            var abilityHealRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int> { { AbilityKey.Heal, 3 } });

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Strength, false },
                { AbilityKey.Speed, false },
                { AbilityKey.DamageResistPotion, false },
                { AbilityKey.VigorPotion, false },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Spider, new List<AbilityKey> { AbilityKey.Heal, AbilityKey.ExtraActionPotion } },
                { BoardPieceId.IceElemental, new List<AbilityKey> { AbilityKey.LetItRain } },
            });

            var pieceAbilityListOverriddenRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBarbarian, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroGuardian, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroHunter, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroSorcerer, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroRogue, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroBard, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroWarlock, new List<AbilityKey> { AbilityKey.ExtraActionPotion, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.Tornado, AbilityKey.AbsorbMySoul } },
            });

            var enemyRespawnRule = new EnemyRespawnDisabledRule(true);

            var lampTypesRule = new LampTypesOverriddenRule(new Dictionary<int, List<BoardPieceId>>
            {
                {
                    1, new List<BoardPieceId>
                    {
                        BoardPieceId.OilLamp,
                        BoardPieceId.OilLamp,
                        BoardPieceId.OilLamp,
                        BoardPieceId.VortexLamp,
                    }
                },
                {
                    2, new List<BoardPieceId>
                    {
                        BoardPieceId.GasLamp,
                        BoardPieceId.GasLamp,
                        BoardPieceId.GasLamp,
                        BoardPieceId.VortexLamp,
                    }
                },
                {
                    3, new List<BoardPieceId>
                    {
                        BoardPieceId.IceLamp,
                        BoardPieceId.IceLamp,
                        BoardPieceId.IceLamp,
                        BoardPieceId.VortexLamp,
                    }
                },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                abilityDamageRule,
                startingCardsRule,
                allowedCardsRule,
                pieceConfigAdjustedRule,
                backstabPieceConfigRule,
                abilityBackstabRule,
                monsterDeckRule,
                abilityAoeRule,
                abilityHealRule,
                abilityMaxedRule,
                pieceUseWhenKilledRule,
                pieceAbilityListOverriddenRule,
                enemyRespawnRule,
                lampTypesRule);
        }
    }
}
