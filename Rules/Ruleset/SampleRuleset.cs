namespace Rules.Ruleset
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents a set of rules to activate together.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///     It is recommended to use the suffix <c>Ruleset</c> for ruleset classes.
    ///     </para>
    /// </remarks>
    public sealed class SampleRuleset : RulesAPI.Ruleset
    {
        /// <summary>
        /// Gets the description of the ruleset.
        /// </summary>
        public override string Description => "Just a sample set of rules.";

        /// <summary>
        /// Gets the rules of the ruleset.
        /// </summary>
        protected override HashSet<RulesAPI.Rule> Rules { get; } = new HashSet<RulesAPI.Rule>
        {
            new Rule.SampleRule(),
        };
    }
}
