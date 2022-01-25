namespace Rules
{
    using MelonLoader;

    internal class RulesMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            RegisterNewRuleTypes();
            RegisterNewRuleSets();
        }

        private static void RegisterNewRuleTypes()
        {
            var registrar = RulesAPI.Registrar.Instance();
        }

        private static void RegisterNewRuleSets()
        {
            var registrar = RulesAPI.Registrar.Instance();
        }
    }
}
