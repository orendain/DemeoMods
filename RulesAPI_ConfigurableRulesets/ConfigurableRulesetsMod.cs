namespace RulesAPI.ConfigurableRulesets
{
    using System.Collections.Generic;
    using DataKeys;
    using MelonLoader;
    using Rules.Rule;

    internal class ConfigurableRulesetsMod : MelonMod
    {
        internal static readonly MelonLogger.Instance Logger = new MelonLogger.Instance("RulesAPI:ConfigurableRulesets");

        public override void OnApplicationStart()
        {
            // DemoWriteRuleset();
            // DemoReadRuleset();
        }

        private static void DemoWriteRuleset()
        {
            var aaca = new AbilityActionCostAdjustedRule(new Dictionary<string, bool>
            {
                { "Zap", false }, // 0 casting cost for zap
                { "StrengthenCourage", false }, // 0 casting cost for Courage
            });
            var aaa = new AbilityAoeAdjustedRule(new Dictionary<string, int>
            {
                { "Fireball", 1 }, // 5x5 fireball
                { "Zap", 1 }, // Just testing
                { "StrengthenCourage", 1 }, // Everyone should hear the bard sing the courage song
                { "Strength", 1 }, // Everyone nearby should share the strength potion
                { "Speed", 1 }, // Everyone nearby should share the speed potion
            });
            var ada = new AbilityDamageAdjustedRule(new Dictionary<string, int> { { "Zap", 1 } });
            var apa = new ActionPointsAdjustedRule(new Dictionary<string, int>
            {
                { "HeroSorcerer", 13 },
                { "HeroGuardian", 2 },
            });
            var cefam1 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefam2 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefam3 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefam4 = new CardEnergyFromAttackMultipliedRule(2f);
            var cefrm = new CardEnergyFromRecyclingMultipliedRule(5f);
            var csvm = new CardSellValueMultipliedRule(2f);
            var eas = new EnemyAttackScaledRule(0.5f);
            var edod = new EnemyDoorOpeningDisabledRule(default);
            var ehs = new EnemyHealthScaledRule(2);
            var erd = new EnemyRespawnDisabledRule(default);
            var gpus = new GoldPickedUpScaledRule(2f);
            var pca = new PieceConfigAdjustedRule(new List<List<string>>
            {
                new List<string> { "HeroSorcerer", "MoveRange", "1" }, // 1 more movement range
                new List<string> { "HeroSorcerer", "StartHealth", "10" }, // 10 extra HP
                new List<string> { "HeroGuardian", "MoveRange", "1" },
                new List<string> { "HeroGuardian", "StartHealth", "10" },
                new List<string> { "HeroHunter", "MoveRange", "1" },
                new List<string> { "HeroHunter", "StartHealth", "10" },
                new List<string> { "HeroBard", "MoveRange", "1" },
                new List<string> { "HeroBard", "StartHealth", "10" },
                new List<string> { "HeroRogue", "MoveRange", "1" },
                new List<string> { "HeroRogue", "StartHealth", "10" },
                new List<string> { "WolfCompanion", "StartHealth", "20" }, // Wolf wastes this many HP wandering through gas
                new List<string> { "SwordOfAvalon", "StartHealth", "10" },
                new List<string> { "BeaconOfSmite", "StartHealth", "10" },
                new List<string> { "BeaconOfSmite", "ActionPoint", "1" }, // Behemoth gets to fire two rounds
                new List<string> { "MonsterBait", "StartHealth", "20" }, // Lure needs to last a little longer
            });
            var rnsg = new RatNestsSpawnGoldRule(8);
            var sscm = new SorcererStartCardsModifiedRule(new List<SorcererStartCardsModifiedRule.CardConfig>
            {
                new SorcererStartCardsModifiedRule.CardConfig { Card = AbilityKey.Blink, IsReplenishable = false },
                new SorcererStartCardsModifiedRule.CardConfig { Card = AbilityKey.Whirlwind, IsReplenishable = false },
                new SorcererStartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = false },
                new SorcererStartCardsModifiedRule.CardConfig { Card = AbilityKey.PoisonedTip, IsReplenishable = false },
            });
            var sha = new StartHealthAdjustedRule(new Dictionary<string, int>
            {
                { "HeroSorcerer", 50 },
                { "HeroGuardian", 20 },
                { "Rat", 100 },
            });

            var customRuleset = Ruleset.NewInstance("DemoConfigurableRuleset", "Just a random description.", new List<Rule>
            {
                aaca, aaa, ada, apa, cefam1, cefam2, cefam3, cefam4, cefrm, csvm, eas, edod, ehs, erd, gpus, pca, rnsg, sscm, sha,
            });

            ConfigManager.WriteRuleset("SavedRuleset1", customRuleset);
        }

        private static void DemoReadRuleset()
        {
            var readRuleset = ConfigManager.ReadRuleset("SavedRuleset1");
            Registrar.Instance().Register(readRuleset);
            RulesAPI.SelectRuleset(readRuleset.Name);
        }
    }
}
