namespace RulesAPI
{
    using System;
    using System.Collections.Generic;

    public class Ruleset
    {
        public string Name { get; }

        public string Description { get; }

        public HashSet<Rule> Rules { get; }

        private Ruleset(string name, string description, HashSet<Rule> rules)
        {
            Name = name;
            Description = description;
            Rules = rules;
        }

        internal void Activate()
        {
            RulesAPI.Logger.Msg($"Activating ruleset: {Name} (with {Rules.Count} rules)");

            foreach (var rule in Rules)
            {
                try
                {
                    rule.Activate();
                }
                catch (Exception e)
                {
                    // TODO(orendain): Rollback activation.
                    RulesAPI.Logger.Warning($"Failed to activate rule [{Name}]: {e}");
                }
            }
        }

        internal void Deactivate()
        {
            RulesAPI.Logger.Msg($"Deactivating ruleset: {Name} (with {Rules.Count} rules)");
            foreach (var rule in Rules)
            {
                try
                {
                    rule.Deactivate();
                }
                catch (Exception e)
                {
                    RulesAPI.Logger.Warning($"Failed to deactivate rule [{Name}]: {e}");
                }
            }
        }
    }
}
