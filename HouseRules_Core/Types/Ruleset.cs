namespace HouseRules.Types
{
    using System.Collections.Generic;
    using System.Linq;

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
        public List<Rule> Rules { get; }

        /// <summary>
        /// Gets a value indicating whether the ruleset is safe to use in multiplayer environments.
        /// </summary>
        public bool IsSafeForMultiplayer { get; }

        /// <summary>
        /// Gets the type of data that the rule makes modifications to.
        /// </summary>
        public SyncableTrigger ModifiedSyncables { get; }

        /// <summary>
        /// Represents the empty/missing ruleset.
        /// </summary>
        public static readonly Ruleset None = NewInstance("None", "No custom ruleset.");

        public static Ruleset NewInstance(string name, string description, params Rule[] rules)
        {
            return NewInstance(name, description, rules.ToList());
        }

        public static Ruleset NewInstance(string name, string description, List<Rule> rules)
        {
            var safeForMultiplayer = rules.All(r => r is IMultiplayerSafe);
            var syncables = rules.Aggregate(SyncableTrigger.None, (data, rule) => data | rule.ModifiedSyncables);
            return new Ruleset(name, description, rules, safeForMultiplayer, syncables);
        }

        private Ruleset(string name, string description, List<Rule> rules, bool isSafeForMultiplayer, SyncableTrigger modifiedSyncables)
        {
            Name = name;
            Description = description;
            Rules = rules;
            IsSafeForMultiplayer = isSafeForMultiplayer;
            ModifiedSyncables = modifiedSyncables;
        }
    }
}
