﻿namespace HouseRules.Essentials.Rulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Core.Types;
    using HouseRules.Essentials.Rules;

    internal static class PotionCommotionRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "Potion Commotion";
            const string description = "All your cards are potions. 1.5x CardEnergy. Free Sneak on Crit. Enemies do not respawn.";
            const string longdesc = "";

            var allowedCards = new List<AbilityKey>
            {
                AbilityKey.DamageResistPotion,
                AbilityKey.DamageResistPotion,
                AbilityKey.SodiumHydroxide,
                AbilityKey.SodiumHydroxide,
                AbilityKey.DamageResistPotion,
                AbilityKey.ExtraActionPotion,
                AbilityKey.ExtraActionPotion,
                AbilityKey.VialOfFireImmunity,
                AbilityKey.VialOfFireImmunity,
                AbilityKey.Heal,
                AbilityKey.Heal,
                AbilityKey.VialOfIceImmunity,
                AbilityKey.VialOfIceImmunity,
                AbilityKey.LuckPotion,
                AbilityKey.LuckPotion,
                AbilityKey.MagicPotion,
                AbilityKey.MagicPotion,
                AbilityKey.Antidote,
                AbilityKey.Antidote,
                AbilityKey.Strength,
                AbilityKey.Strength,
                AbilityKey.Speed,
                AbilityKey.Speed,
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
                longdesc,
                allowedCardsRule,
                abilityActionCostRule,
                enemyRespanDisabled,
                cardEnergyRule);
        }
    }
}
