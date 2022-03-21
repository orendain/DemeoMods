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

            var abilityDamageRule = new AbilityDamageAdjustedRule(new Dictionary<AbilityKey, int> { { AbilityKey.Zap, 1 } });

            var guardianCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WaterDive, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var hunterCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WaterDive, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var sorcererCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WaterDive, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var assassinCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WaterDive, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Sneak, IsReplenishable = false },
            };
            var bardCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HealingPotion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HurricaneAnthem, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Electricity, IsReplenishable = true },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Fireball, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WaterDive, IsReplenishable = false },
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
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "StartHealth", Value = 75 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Brookmare, Property = "StartArmor", Value = 5 },
            });

            var backstabPieceConfigRule = new BackstabConfigOverriddenRule(new List<BoardPieceId>
            { 
                BoardPieceId.HeroGuardian,
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
            });

            var monsterDeckRule = new MonsterDeckOverriddenRule(new List<BoardPieceId> { { BoardPieceId.ChestGoblin } });

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

            var abilityHealRule = new AbilityHealOverriddenRule(new Dictionary<AbilityKey, int> { { AbilityKey.HealingPotion, 2 } });

            var abilityMaxedRule = new RegainAbilityIfMaxxedOutOverriddenRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.StrengthPotion, false },
                { AbilityKey.SwiftnessPotion, false },
            });

            var pieceUseWhenKilledRule = new PieceUseWhenKilledOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.Spider, new List<AbilityKey> { AbilityKey.HealingPotion, AbilityKey.OneMoreThing } },
            });

            var pieceAbilityListOverriddenRule = new PieceAbilityListOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>> {
                { BoardPieceId.HeroGuardian, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroHunter, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroSorcerer, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroRogue, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
                { BoardPieceId.HeroBard, new List<AbilityKey> { AbilityKey.OneMoreThing, AbilityKey.EarthShatter, AbilityKey.Fireball, AbilityKey.HurricaneAnthem, AbilityKey.AbsorbMySoul } },
            });

            var levelProperties = new Dictionary<string, int>
            {
                { "FloorOneBudget", 200 },
                { "FloorOneOuterRingZoneBudget", 190 },
                { "FloorOneBudgetPostSpike", 300 },
                { "FloorTwoBudget", 200 },
                { "FloorTwoOuterRingZoneBudget", 190 },
                { "FloorTwoBudgetPostSpike", 260 },
                { "FloorThreeBudget", 40 },
                { "FloorThreeOuterRingZoneBudget", 40 },
                { "FloorThreeBudgetPostSpike", 80 },
                { "PacingSpikeSegmentFloorOneBudget", 10 },
                { "PacingSpikeSegmentFloorTwoBudget", 30 },
                { "PacingSpikeSegmentFloorThreeBudget", 10 },
            };
            var levelPropertiesRule = new LevelPropertiesModifiedRule(levelProperties);

            var enemyRespawnRule = new EnemyRespawnDisabledRule(true);

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
                levelPropertiesRule,
                enemyRespawnRule);
        }
    }
}
