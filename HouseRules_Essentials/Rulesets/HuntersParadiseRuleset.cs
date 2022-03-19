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

            var startCards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.CallCompanion, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.SummonElemental, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.HuntersMark, IsReplenishable = true },
            };
            var startingCardsRule = new StartCardsModifiedRule(
                new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>>
                {
                    { BoardPieceId.HeroBard, startCards },
                    { BoardPieceId.HeroGuardian, startCards },
                    { BoardPieceId.HeroHunter, startCards },
                    { BoardPieceId.HeroRogue, startCards },
                    { BoardPieceId.HeroSorcerer, startCards },
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
                    stacks = false,
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
                { BoardPieceId.HeroBard, allowedCards },
                { BoardPieceId.HeroGuardian, allowedCards },
                { BoardPieceId.HeroHunter, allowedCards },
                { BoardPieceId.HeroRogue, allowedCards },
                { BoardPieceId.HeroSorcerer, allowedCards },
            });

            var levelPropertiesRule = new LevelPropertiesModifiedRule(new Dictionary<string, int>
            {
                { "FloorOneHealingFountains", 6 },
                { "FloorOneLootChests", 6 },
                { "FloorTwoHealingFountains", 6 },
                { "FloorTwoLootChests", 12 },
                { "FloorThreeHealingFountains", 6 },
                { "FloorThreeLootChests", 12 },
            });

            return Ruleset.NewInstance(
                name,
                description,
                startingCardsRule,
                statusEffectsRule,
                noGuardianArmorRule,
                hunterMarkRule,
                allowedCardsRule,
                levelPropertiesRule);
        }
    }
}
