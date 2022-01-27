namespace RulesAPI
{
    using System;
    using System.Collections.Generic;

    public class Ruleset
    {
        /// <summary>
        /// Gets the name of the ruleset.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the ruleset.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the rules of the ruleset.
        /// </summary>
        public HashSet<Rule> Rules { get; }

        public static Ruleset NewInstance(string name, string description, HashSet<Rule> rules)
        {
            return new Ruleset(name, description, rules);
        }

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
