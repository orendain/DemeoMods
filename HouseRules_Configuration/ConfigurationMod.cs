namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;
    using MelonLoader;
    using UnityEngine;
    using StatusEffectData = global::Types.StatusEffectData;

    internal class ConfigurationMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        private static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();
        private const int LobbySceneIndex = 1;

        private GameObject _rulesetSelectionUI;

        public override void OnApplicationLateStart()
        {
            // TODO(orendain): Remove when this demo code is no longer necessary.
            // DemoWriteRuleset();

            var loadRulesetsFromConfig = ConfigManager.GetLoadRulesetsFromConfig();
            if (loadRulesetsFromConfig)
            {
                LoadRulesetsFromConfig();
            }

            var rulesetName = ConfigManager.GetDefaultRuleset();
            if (string.IsNullOrEmpty(rulesetName))
            {
                return;
            }

            try
            {
                HR.SelectRuleset(rulesetName);
            }
            catch (ArgumentException e)
            {
                Logger.Warning($"Failed to select default ruleset [{rulesetName}] specified in config: {e}");
            }
        }

        public override void OnSceneWasInitialized(int buildIndex, string sceneName)
        {
            if (buildIndex == LobbySceneIndex)
            {
                _rulesetSelectionUI = new GameObject("RulesetSelectionUI", typeof(UI.RulesetSelectionUI));
            }
        }

        private static void LoadRulesetsFromConfig()
        {
            var hadIssues = ConfigManager.TryImportRulesets(out var rulesets);
            foreach (var ruleset in rulesets)
            {
                HR.Rulebook.Register(ruleset);
                Logger.Msg($"Loaded and registered ruleset from config: {ruleset.Name}");
            }
        }

        // TODO(orendain): Remove when this demo code is no longer necessary.
        public static void DemoWriteRuleset()
        {
            var aaca = new Essentials.Rules.AbilityActionCostAdjustedRule(new Dictionary<string, bool>
            {
                { "Zap", false }, // 0 casting cost for zap
                { "StrengthenCourage", false }, // 0 casting cost for Courage
            });
            var aaa = new Essentials.Rules.AbilityAoeAdjustedRule(new Dictionary<string, int>
            {
                { "Fireball", 1 }, // 5x5 fireball
                { "Zap", 1 }, // Just testing
                { "StrengthenCourage", 1 }, // Everyone should hear the bard sing the courage song
                { "Strength", 1 }, // Everyone nearby should share the strength potion
                { "Speed", 1 }, // Everyone nearby should share the speed potion
            });
            var ada = new Essentials.Rules.AbilityDamageAdjustedRule(new Dictionary<string, int> { { "Zap", 1 } });
            var cefam1 = new Essentials.Rules.CardEnergyFromAttackMultipliedRule(2f);
            var cefam2 = new Essentials.Rules.CardEnergyFromAttackMultipliedRule(2f);
            var cefam3 = new Essentials.Rules.CardEnergyFromAttackMultipliedRule(2f);
            var cefam4 = new Essentials.Rules.CardEnergyFromAttackMultipliedRule(2f);
            var cefrm = new Essentials.Rules.CardEnergyFromRecyclingMultipliedRule(5f);
            var csvm = new Essentials.Rules.CardSellValueMultipliedRule(2f);
            var eas = new Essentials.Rules.EnemyAttackScaledRule(0.5f);
            var edod = new Essentials.Rules.EnemyDoorOpeningDisabledRule(true);
            var ehs = new Essentials.Rules.EnemyHealthScaledRule(2);
            var erd = new Essentials.Rules.EnemyRespawnDisabledRule(true);
            var gpus = new Essentials.Rules.GoldPickedUpMultipliedRule(2f);
            var pca = new Essentials.Rules.PieceConfigAdjustedRule(new List<PieceConfigAdjustedRule.PieceProperty>
            {
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "MoveRange", Value = 6 }, // Increased from the default of 4
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroSorcerer, Property = "StartHealth", Value = 20 }, // 10 extra HP
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroGuardian, Property = "StartHealth", Value = 15 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "MoveRange", Value = 6 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroHunter, Property = "StartHealth", Value = 18 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "MoveRange", Value = 8 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroBard, Property = "StartHealth", Value = 12 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRouge, Property = "MoveRange", Value = 5 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.HeroRouge, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.WolfCompanion, Property = "StartHealth", Value = 20 }, // Wolf lots of HP wandering through gas
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.SwordOfAvalon, Property = "StartHealth", Value = 20 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.BeaconOfSmite, Property = "StartHealth", Value = 25 },
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.BeaconOfSmite, Property = "ActionPoint", Value = 2 }, // Behemoth gets to fire two rounds
                new PieceConfigAdjustedRule.PieceProperty { Piece = BoardPieceId.MonsterBait, Property = "StartHealth", Value = 25 }, // Lure needs to last a little longer
            });
            var rnsg = new Essentials.Rules.RatNestsSpawnGoldRule(8);
            var cards = new List<Essentials.Rules.StartCardsModifiedRule.CardConfig>
            {
                new Essentials.Rules.StartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, IsReplenishable = false },
                new Essentials.Rules.StartCardsModifiedRule.CardConfig { Card = AbilityKey.Whirlwind, IsReplenishable = false },
                new Essentials.Rules.StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = false },
                new Essentials.Rules.StartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = false },
            };
            var sorcCards = new Dictionary<BoardPieceId, List<Essentials.Rules.StartCardsModifiedRule.CardConfig>> { { BoardPieceId.HeroSorcerer, cards } };
            var sscm = new Essentials.Rules.StartCardsModifiedRule(sorcCards);

            BoardPieceId[] bps = { BoardPieceId.ChestGoblin, BoardPieceId.Slime };
            var arpl = new Essentials.Rules.AbilityRandomPieceListRule(new Dictionary<string, BoardPieceId[]>
            {
                { "NaturesCall", bps },
            });

            EffectStateType[] effs = { EffectStateType.Diseased, EffectStateType.Stunned, EffectStateType.MarkOfAvalon, EffectStateType.Weaken, EffectStateType.Frozen, EffectStateType.Tangled, EffectStateType.Petrified };
            var pila = new Essentials.Rules.PieceImmunityListAdjustedRule(new Dictionary<BoardPieceId, EffectStateType[]> { { BoardPieceId.HeroSorcerer, effs } } );

            var sec = new List<StatusEffectData>
            {
                new StatusEffectData {
                    effectStateType = EffectStateType.TorchPlayer,
                    durationTurns = 15,
                    damagePerTurn = 0,
                    stacks = true,
                    clearOnNewLevel = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
                new StatusEffectData {
                    effectStateType = EffectStateType.HealingSong,
                    durationTurns = 4,
                    healPerTurn = 3,
                    stacks = false,
                    tickWhen = StatusEffectsConfig.TickWhen.StartTurn,
                },
            };
            var seca = new Essentials.Rules.StatusEffectConfigRule(sec);
            var customRuleset = Ruleset.NewInstance("DemoConfigurableRuleset", "Just a random description.", new List<Rule>
            {
                aaca, aaa, ada, cefam1, cefam2, cefam3, cefam4, cefrm, csvm, eas, edod, ehs, erd, gpus, pca, rnsg, sscm, arpl, pila, seca,
            });

            ConfigManager.ExportRuleset(customRuleset);
        }
    }
}
