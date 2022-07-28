namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class PotionCommotionRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Potion Commotion";
            const string description = "Nothing but potions in the cards you get given. Enemies do not respawn.";

            var allowedCards = new List<AbilityKey>
            {
                AbilityKey.AdamantPotion,
                AbilityKey.BottleOfLye,
                AbilityKey.DamageResistPotion,
                AbilityKey.ExtraActionPotion,
                AbilityKey.FireImmunePotion,
                AbilityKey.HealingPotion,
                AbilityKey.IceImmunePotion,
                AbilityKey.LuckPotion,
                AbilityKey.MagicPotion,
                AbilityKey.SpellPowerPotion,
                AbilityKey.StrengthPotion,
                AbilityKey.SwiftnessPotion,
                AbilityKey.VigorPotion,
                AbilityKey.WaterBottle,
            };

            var allowedCardsRule = new CardAdditionOverriddenRule(new Dictionary<BoardPieceId, List<AbilityKey>>
            {
                { BoardPieceId.HeroBard, allowedCards },
                { BoardPieceId.HeroGuardian, allowedCards },
                { BoardPieceId.HeroHunter, allowedCards },
                { BoardPieceId.HeroRogue, allowedCards },
                { BoardPieceId.HeroSorcerer, allowedCards },
                { BoardPieceId.HeroWarlock, allowedCards },
            });

            var abilityActionCostRule = new AbilityActionCostAdjustedRule(new Dictionary<AbilityKey, bool>
            {
                { AbilityKey.Zap, false },
            });

            var enemyRespanDisabled = new EnemyRespawnDisabledRule(true);

            return Ruleset.NewInstance(
                name,
                description,
                allowedCardsRule,
                abilityActionCostRule,
                enemyRespanDisabled);
        }
    }
}
