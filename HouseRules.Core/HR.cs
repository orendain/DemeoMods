namespace HouseRules.Core
{
    using System;
    using System.Linq;
    using HouseRules.Core.Types;

    public static class HR
    {
        public static readonly Rulebook Rulebook = Rulebook.NewInstance();

        public static Ruleset SelectedRuleset { get; private set; } = Ruleset.None;

        internal static bool IsRulesetActive => LifecycleDirector.IsRulesetActive;

        public static void ScheduleResync() => BoardSyncer.ScheduleSync();

        public static void SelectRuleset(string ruleset)
        {
            if (IsRulesetActive)
            {
                throw new InvalidOperationException("May not select a new ruleset while one is currently active.");
            }

            if (Ruleset.None.Name.Equals(ruleset, StringComparison.OrdinalIgnoreCase))
            {
                SelectedRuleset = Ruleset.None;
                HouseRulesCoreBase.LogInfo("Cleared selected ruleset.");
                return;
            }

            if (!Rulebook.IsRulesetRegistered(ruleset))
            {
                throw new ArgumentException("Ruleset must first be registered.");
            }

            SelectedRuleset = Rulebook.Rulesets.First(r => string.Equals(r.Name, ruleset, StringComparison.OrdinalIgnoreCase));
            HouseRulesCoreBase.LogInfo($"Selected ruleset: {SelectedRuleset.Name}");
        }

        public static void ScheduleBoardSync()
        {
            BoardSyncer.ScheduleSync();
        }
    }
}
