namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class EarthWindAndFire
    {
        internal static Ruleset Create()
        {
            const string name = "Earth Wind & Fire";
            const string description = "Not the band. Let's get Elemental";

            var abilityDamageRule = new AbilityDamageOverriddenRule(new Dictionary<AbilityKey, List<int>> { { AbilityKey.Zap, new List<int> { 2, 5 } } });

            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var startingCardsRule = new StartCardsModifiedRule(new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
            {
                { BoardPieceId.HeroGuardian, guardianCards },
                { BoardPieceId.HeroHunter, hunterCards },
                { BoardPieceId.HeroRogue, assassinCards },
                { BoardPieceId.HeroSorcerer, sorcererCards },
                { BoardPieceId.HeroBard, bardCards },
            });

            var allowedCards = new List<AbilityKey>
            {
                AbilityKey.Bone,
                AbilityKey.Fireball,
                AbilityKey.Teleportation,
                AbilityKey.RevealPath,
                AbilityKey.Freeze,
                AbilityKey.StrengthPotion,
                AbilityKey.SwiftnessPotion,
                AbilityKey.CallCompanion,
                AbilityKey.WhirlwindAttack,
                AbilityKey.LetItRain,
                AbilityKey.WaterDive,
                AbilityKey.HeavensFury,
                AbilityKey.AdamantPotion,
                AbilityKey.HealingPotion,
                AbilityKey.WoodenBone,
                AbilityKey.Torch,
            };
            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroGuardian, allowedCards },
                { BoardPieceId.HeroHunter, allowedCards },
                { BoardPieceId.HeroRogue, allowedCards },
                { BoardPieceId.HeroSorcerer, allowedCards },
                { BoardPieceId.HeroBard, allowedCards },
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
                BoardPieceId.HeroGuardian,
                BoardPieceId.HeroSorcerer,
                BoardPieceId.HeroRogue,
                BoardPieceId.HeroBard,
            });

            var abilityBackstabRule = new AbilityBackstabAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, true },
                { AbilityKey.Arrow, true },
                { AbilityKey.PiercingThrow, true },
                { AbilityKey.PoisonedTip, true },
                { AbilityKey.Fireball, true },
                { AbilityKey.Freeze, true },
                { AbilityKey.Bone, true },
                { AbilityKey.WhirlwindAttack, true },
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
                { AbilityKey.CourageShanty, 1 },
                { AbilityKey.ReplenishArmor, 1 },
                { AbilityKey.StrengthPotion, 1 },
                { AbilityKey.SwiftnessPotion, 1 },
                { AbilityKey.Antitoxin, 1 },
                { AbilityKey.AdamantPotion, 1 },
                { AbilityKey.HealingPotion, 1 },
                { AbilityKey.OneMoreThing, 1 },
            });

            var abilityHealRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int> { { AbilityKey.HealingPotion, 3 } });

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.StrengthPotion, false },
                { AbilityKey.SwiftnessPotion, false },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Spider, new List<AbilityKey> { AbilityKey.HealingPotion, AbilityKey.OneMoreThing } },
                { BoardPieceId.IceElemental, new List<AbilityKey> { AbilityKey.LetItRain } },
            });

            var pieceAbilityListOverriddenRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroGuardian, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroHunter, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroSorcerer, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroRogue, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroBard, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
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
