namespace HouseRules.Essentials.Rulesets
{
    using HouseRules.Essentials.Rules;
    using HouseRules.Types;

    internal static class SampleRuleset
    {
        internal static Ruleset Create()
        {
            const string name = "SampleRuleset";
            const string description = "Just a sample ruleset.";

            var sampleRule = new SampleRule();

            return Ruleset.NewInstance(name, description, sampleRule);
        }
    }
}
