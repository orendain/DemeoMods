namespace HouseRules.Types
{
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
        public List<Rule> Rules { get; }

        public static Ruleset NewInstance(string name, string description, List<Rule> rules)
        {
            return new Ruleset(name, description, rules);
        }

        private Ruleset(string name, string description, List<Rule> rules)
        {
            Name = name;
            Description = description;
            Rules = rules;
        }
    }
}
