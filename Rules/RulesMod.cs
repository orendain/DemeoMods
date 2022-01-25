namespace Rules
{
    using MelonLoader;

    internal class RulesMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            RegisterNewRuleTypes();
            RegisterNewRulesets();
        }

        private static void RegisterNewRuleTypes()
        {
            var registrar = RulesAPI.Registrar.Instance();
            registrar.Register(typeof(Rule.SampleRule));
        }

        private static void RegisterNewRulesets()
        {
            var registrar = RulesAPI.Registrar.Instance();
            registrar.Register(new Ruleset.SampleRuleset());
        }
    }
}
