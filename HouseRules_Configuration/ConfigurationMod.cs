namespace HouseRules.Configuration
{
    using System;
    using System.Collections.Generic;
    using DataKeys;
    using HouseRules.Types;
    using MelonLoader;

    internal class ConfigurationMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("HouseRules:Configuration");
        private static readonly ConfigManager ConfigManager = ConfigManager.NewInstance();

        public override void OnApplicationLateStart()
        {
            // TODO(orendain): Remove when this demo code is no longer necessary.
            // DemoWriteRuleset();

            var rulesetName = ConfigManager.GetRuleset();
            if (string.IsNullOrEmpty(rulesetName))
            {
                return;
            }

            var loadFromConfig = ConfigManager.GetLoadFromConfig();
            if (loadFromConfig)
            {
                try
                {
                    var ruleset = ConfigManager.ReadRuleset(rulesetName);
                    HR.Rulebook.Register(ruleset);
                    Logger.Msg($"Loaded and registered ruleset from config: {rulesetName}");
                }
                catch (Exception e)
                {
                    Logger.Warning($"Failed to load and register ruleset [{rulesetName}] from config: {e}");
                }
            }

            try
            {
                HR.SelectRuleset(rulesetName);
            }
            catch (ArgumentException e)
            {
                Logger.Warning($"Failed to select ruleset [{rulesetName}]: {e}");
            }
        }

        public override void OnApplicationQuit()
        {
            var rulesetName = HR.SelectedRuleset != null ? HR.SelectedRuleset.Name : string.Empty;
            ConfigManager.SetRuleset(rulesetName);
            ConfigManager.Save();
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
            var apa = new Essentials.Rules.ActionPointsAdjustedRule(new Dictionary<string, int>
            {
                { "HeroSorcerer", 13 },
                { "HeroGuardian", 2 },
            });
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
            var gpus = new Essentials.Rules.GoldPickedUpScaledRule(2f);
            var pca = new Essentials.Rules.PieceConfigAdjustedRule(new List<List<string>>
            {
                new List<string> { "HeroSorcerer", "MoveRange", "1" }, // 1 more movement range
                new List<string> { "HeroSorcerer", "StartHealth", "10" }, // 10 extra HP
                new List<string> { "HeroGuardian", "MoveRange", "1" },
                new List<string> { "HeroGuardian", "StartHealth", "10" },
                new List<string> { "HeroHunter", "MoveRange", "1" },
                new List<string> { "HeroHunter", "StartHealth", "10" },
                new List<string> { "HeroBard", "MoveRange", "1" },
                new List<string> { "HeroBard", "StartHealth", "10" },
                new List<string> { "HeroRouge", "MoveRange", "1" },
                new List<string> { "HeroRouge", "StartHealth", "10" },
                new List<string> { "WolfCompanion", "StartHealth", "20" }, // Wolf wastes this many HP wandering through gas
                new List<string> { "SwordOfAvalon", "StartHealth", "10" },
                new List<string> { "BeaconOfSmite", "StartHealth", "10" },
                new List<string> { "BeaconOfSmite", "ActionPoint", "1" }, // Behemoth gets to fire two rounds
                new List<string> { "MonsterBait", "StartHealth", "20" }, // Lure needs to last a little longer
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
            var sha = new Essentials.Rules.StartHealthAdjustedRule(new Dictionary<string, int>
            {
                { "HeroSorcerer", 50 },
                { "HeroGuardian", 20 },
                { "Rat", 100 },
            });

            var customRuleset = Ruleset.NewInstance("DemoConfigurableRuleset", "Just a random description.", new List<Rule>
            {
                aaca, aaa, ada, apa, cefam1, cefam2, cefam3, cefam4, cefrm, csvm, eas, edod, ehs, erd, gpus, pca, rnsg, sscm, sha,
            });

            ConfigManager.WriteRuleset(customRuleset);
        }
    }
}
