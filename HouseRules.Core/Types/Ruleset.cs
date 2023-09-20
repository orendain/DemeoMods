namespace HouseRules.Core.Types
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
        /// Gets the long description of the ruleset.
        /// </summary>
        public string Longdesc { get; }

        /// <summary>
        /// Gets the rules of the ruleset.
        /// </summary>
        public List<Rule> Rules { get; }

        /// <summary>
        /// Gets a value indicating whether the ruleset is safe to use in multiplayer environments.
        /// </summary>
        public bool IsSafeForMultiplayer { get; }

        /// <summary>
        /// Gets the syncable types that this rule modifies.
        /// </summary>
        public SyncableTrigger ModifiedSyncables { get; }

        /// <summary>
        /// Represents the empty/missing ruleset.
        /// </summary>
        public static readonly Ruleset None = NewInstance("None", "No custom ruleset.", string.Empty);

        public static Ruleset NewInstance(string name, string description, string longdesc, params Rule[] rules)
        {
            return NewInstance(name, description, longdesc, rules.ToList());
        }

        public static Ruleset NewInstance(string name, string description, string longdesc, List<Rule> rules)
        {
            var safeForMultiplayer = rules.All(r => r is IMultiplayerSafe);
            var syncables = rules.Aggregate(SyncableTrigger.None, (data, rule) => data | rule.ModifiedSyncables);
            return new Ruleset(name, description, longdesc, rules, safeForMultiplayer, syncables);
        }

        private Ruleset(string name, string description, string longdesc, List<Rule> rules, bool isSafeForMultiplayer, SyncableTrigger modifiedSyncables)
        {
            Name = name;
            Description = description;
            Longdesc = longdesc;
            Rules = rules;
            IsSafeForMultiplayer = isSafeForMultiplayer;
            ModifiedSyncables = modifiedSyncables;
        }
    }
}
