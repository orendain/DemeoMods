// TODO(orendain): Remove when writing this example ruleset is no longer necessary.

namespace HouseRules.Configuration
{
    using System.Collections.Generic;
    using DataKeys;
    using global::Types;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class ExampleRulesetExporter
    {
        internal static void ExportExampleRulesetIfNeeded()
        {
            // Uncomment to export example ruleset.
            // ExportExampleRuleset();
        }

        private static void ExportExampleRuleset()
        {
            var aaca = new AbilityActionCostAdjustedRule(new Dictionary<string, bool>
            {
                { "Zap", false }, // 0 casting cost for zap
                { "CourageShanty", false }, // 0 casting cost for Courage
            });
            var aaa = new AbilityAoeAdjustedRule(new Dictionary<string, int>
            {
                { "Fireball", 1 }, // 5x5 fireball
                { "Zap", 1 }, // Just testing
                { "CourageShanty", 1 }, // Everyone should hear the bard sing the courage song
                { "StrengthPotion", 1 }, // Everyone nearby should share the strength potion
                { "SwiftnessPotion", 1 }, // Everyone nearby should share the speed potion
            });
            var ada = new AbilityDamageAdjustedRule(new Dictionary<string, int> { { "Zap", 1 } });
            var cefam1 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefam2 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefam3 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefam4 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefrm = new CardEnergyFromRecyclingMultipliedRule(5f);
            var csvm = new CardSellValueMultipliedRule(2f);
            var eas = new EnemyAttackScaledRule(0.5f);
            var edod = new EnemyDoorOpeningDisabledRule(true);
            var ehs = new EnemyHealthScaledRule(2);
            var erd = new EnemyRespawnDisabledRule(true);
            var gpus = new GoldPickedUpMultipliedRule(2f);
            var pca = new PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "MoveRange", Value = 6 }, // Increased from the default of 4
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 20 }, // 10 extra HP
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "MoveRange", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRogue, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Verochka, Property = "StartHealth", Value = 20 }, // Wolf lots of HP wandering through gas
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "StartHealth", Value = 25 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SmiteWard, Property = "ActionPoint", Value = 2 }, // Behemoth gets to fire two rounds
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.Lure, Property = "StartHealth", Value = 25 }, // Lure needs to last a little longer
            });
            var rnsg = new RatNestsSpawnGoldRule(8);
            var cards = new List<StartCardsModifiedRule.CardConfig>
            {
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.WhirlwindAttack, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = false },
                new StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = false },
            };
            var sorcCards = new Dictionary<BoardPieceId, List<StartCardsModifiedRule.CardConfig>> { { BoardPieceId.HeroSorcerer, cards } };
            var sscm = new StartCardsModifiedRule(sorcCards);

            BoardPieceId[] bps = { BoardPieceId.ChestGoblin, BoardPieceId.Slimeling };
            var arpl = new AbilityRandomPieceListRule(new Dictionary<string, BoardPieceId[]>
            {
                { "BeastWhisperer", bps },
            });

            List<EffectStateType> effs = new List<EffectStateType>() { EffectStateType.Diseased, EffectStateType.Stunned, EffectStateType.MarkOfAvalon, EffectStateType.Weaken, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Petrified };
            var pila = new PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, List<EffectStateType>> { { BoardPieceId.HeroSorcerer, effs } });

            var sec = new List<StatusEffectData>
            {
                new StatusEffectData
                {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 15,
                    damagePerTurn = 0,
                    stacks = true,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData
                {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 4,
                    healPerTurn = 3,
                    stacks = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
            };
            var seca = new StatusEffectConfigRule(sec);
            var customRuleset = Ruleset.NewInstance("DemoConfigurableRuleset", "Just a random description.", new List<Rule>
            {
                aaca, aaa, ada, cefam1, cefam2, cefam3, cefam4, cefrm, csvm, eas, edod, ehs, erd, gpus, pca, rnsg, sscm, arpl, pila, seca,
            });

            ConfigurationMod.ConfigManager.ExportRuleset(customRuleset);
        }
    }
}
