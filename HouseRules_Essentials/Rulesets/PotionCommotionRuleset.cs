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
            const string description = "All your cards are potions. 1.5x CardEnergy. Free Sneak on Crit. Enemies do not respawn.";

            var allowedCards = new List<AbilityKey>
            {
                AbilityKey.AdamantPotion,
                AbilityKey.AdamantPotion,
                AbilityKey.BottleOfLye,
                AbilityKey.BottleOfLye,
                AbilityKey.DamageResistPotion,
                AbilityKey.ExtraActionPotion,
                AbilityKey.ExtraActionPotion,
                AbilityKey.FireImmunePotion,
                AbilityKey.FireImmunePotion,
                AbilityKey.HealingPotion,
                AbilityKey.HealingPotion,
                AbilityKey.IceImmunePotion,
                AbilityKey.IceImmunePotion,
                AbilityKey.LuckPotion,
                AbilityKey.LuckPotion,
                AbilityKey.MagicPotion,
                AbilityKey.MagicPotion,
                AbilityKey.Antitoxin,
                AbilityKey.Antitoxin,
                AbilityKey.StrengthPotion,
                AbilityKey.StrengthPotion,
                AbilityKey.SwiftnessPotion,
                AbilityKey.SwiftnessPotion,
                AbilityKey.VigorPotion,
                AbilityKey.VigorPotion,
                AbilityKey.WaterBottle,
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
                { AbilityKey.Overcharge, false },
            });

            var enemyRespanDisabled = new EnemyRespawnDisabledRule(true);

            var cardEnergyRule = new CardEnergyFromAttackMultipliedRule(1.5f);

            return Ruleset.NewInstance(
                name,
                description,
                allowedCardsRule,
                abilityActionCostRule,
                enemyRespanDisabled,
                cardEnergyRule);
        }
    }
}
