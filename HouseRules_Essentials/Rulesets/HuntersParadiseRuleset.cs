namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class HuntersParadiseRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Hunter's Paradise";
            const string description = "Pets, pets, pets! And hunter's mark.";
            const string longdesc = "";

            var startCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, ReplenishFrequency = 0 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SummonElemental, ReplenishFrequency = 1 },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HuntersMark, ReplenishFrequency = 1 },
            };
            var startingCardsRule = new StartCardsModifiedRule(
                new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
                {
                    { BoardPieceId.HeroBarbarian, startCards },
                    { BoardPieceId.HeroBard, startCards },
                    { BoardPieceId.HeroGuardian, startCards },
                    { BoardPieceId.HeroHunter, startCards },
                    { BoardPieceId.HeroRogue, startCards },
                    { BoardPieceId.HeroSorcerer, startCards },
                    { BoardPieceId.HeroWarlock, startCards },
                });

            var noGuardianArmorRule = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartArmor", Value = 0 },
            });

            var statusEffects = new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.MarkOfAvalon,
                    durationTurns = 2,
                    damagePerTurn = 0,
                    clearOnNewLevel = true,
                    tickWhen = StatusEffectsConfig.TickWhen.EndTurn,
                },
            };
            var statusEffectsRule = new StatusEffectConfigRule(statusEffects);

            var hunterMarkRule = new PetsFocusHunterMarkRule(true);

            var allowedCards = new List<AbilityKey>
            {
                AbilityKey.CallCompanion,
                AbilityKey.SummonElemental,
                AbilityKey.BeastWhisperer,
                AbilityKey.PiercingVoice,
                AbilityKey.Bone,
                AbilityKey.StrengthPotion,
                AbilityKey.SwiftnessPotion,
            };
            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBarbarian, allowedCards },
                { BoardPieceId.HeroBard, allowedCards },
                { BoardPieceId.HeroGuardian, allowedCards },
                { BoardPieceId.HeroHunter, allowedCards },
                { BoardPieceId.HeroRogue, allowedCards },
                { BoardPieceId.HeroSorcerer, allowedCards },
                { BoardPieceId.HeroWarlock, allowedCards },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 6 },
                { "FloorOneLootChests", 5 },
                { "FloorOnePotionStand", 1 },
                { "FloorOneElvenSummoners", 0 },
                { "FloorTwoHealingFountains", 6 },
                { "FloorTwoLootChests", 10 },
                { "FloorTwoPotionStand", 2 },
                { "FloorTwoElvenSummoners", 0 },
                { "FloorThreeHealingFountains", 6 },
                { "FloorThreeLootChests", 11 },
                { "FloorThreePotionStand", 1 },
                { "FloorThreeElvenSummoners", 0 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                longdesc,
                startingCardsRule,
                statusEffectsRule,
                noGuardianArmorRule,
                hunterMarkRule,
                allowedCardsRule,
                levelPropertiesRule);
        }
    }
}
