namespace HouseRules
{
    using System;
    using System.Linq;
    using HouseRules.Types;

    public static class HR
    {
        public static readonly Rulebook Rulebook = Rulebook.NewInstance();

        public static Ruleset SelectedRuleset { get; private set; } = Ruleset.None;

        internal static bool IsRulesetActive => LifecycleDirector.IsRulesetActive;

        internal static bool IsReconnect => LifecycleDirector.IsReconnect;

        public static void ScheduleResync() => BoardSyncer.ScheduleSync();

        public static void SelectRuleset(string ruleset)
        {
            if (IsRulesetActive)
            {
                if (IsReconnect)
                {
                    LifecycleDirector.DeactivateReconnect();
                }
                else
                {
                    throw new InvalidOperationException("May not select a new ruleset while one is currently active.");
                }
            }

            if (Ruleset.None.Name.Equals(ruleset, StringComparison.OrdinalIgnoreCase))
            {
                SelectedRuleset = Ruleset.None;
                CoreMod.Logger.Msg("Cleared selected ruleset.");
                return;
            }

            if (!Rulebook.IsRulesetRegistered(ruleset))
            {
                throw new ArgumentException("Ruleset must first be registered.");
            }

            SelectedRuleset = Rulebook.Rulesets.First(r => string.Equals(r.Name, ruleset, StringComparison.OrdinalIgnoreCase));
            var setName = SelectedRuleset.Name;
            if (setName.Contains("<b>None</b>"))
            {
                setName = "None";
            }
            else if (setName.Contains("Demeo Revolutions"))
            {
                if (setName.Contains("(EASY"))
                {
                    setName = "Demeo Revolutions (EASY)";
                }
                else if (setName.Contains("(HARD"))
                {
                    setName = "Demeo Revolutions (HARD)";
                }
                else if (setName.Contains("(LEGENDARY"))
                {
                    setName = "Demeo Revolutions (LEGENDARY)";
                }
                else if (setName.Contains("(PROGRESSIVE"))
                {
                    setName = "Demeo Revolutions (PROGRESSIVE)";
                }
                else
                {
                    setName = "Demeo Revolutions";
                }
            }

            CoreMod.Logger.Msg($"Selected ruleset: {setName}");
        }

        public static void ScheduleBoardSync()
        {
            BoardSyncer.ScheduleSync();
        }
    }
}
