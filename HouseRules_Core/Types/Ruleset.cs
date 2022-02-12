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
        /// Gets a value indicating whether this ruleset is safe to use in multiplayer environments.
        /// </summary>
        public bool IsSafeForMultiplayer { get; }

        public static Ruleset NewInstance(string name, string description, params Rule[] rules)
        {
            return NewInstance(name, description, rules.ToList());
        }

        public static Ruleset NewInstance(string name, string description, List<Rule> rules)
        {
            var safeForMultiplayer = rules.All(r => r is IMultiplayerSafe);
            return new Ruleset(name, description, rules, safeForMultiplayer);
        }

        private Ruleset(string name, string description, List<Rule> rules, bool isSafeForMultiplayer)
        {
            Name = name;
            Description = description;
            Rules = rules;
            IsSafeForMultiplayer = isSafeForMultiplayer;
        }
    }
}
