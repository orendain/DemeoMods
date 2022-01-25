namespace RulesAPI
{
    using System;
    using System.Collections.Generic;

    public class Registrar
    {
        private static Registrar _instance;

        internal HashSet<Type> RuleTypes { get; }

        internal HashSet<RuleSet> RuleSets { get; }

        public static Registrar Instance()
        {
            if (_instance != null)
            {
                return _instance;
            }

            _instance = new Registrar();
            return _instance;
        }

        private Registrar()
        {
            RuleTypes = new HashSet<Type>();
            RuleSets = new HashSet<RuleSet>();
        }

        public bool IsRegistered(Type ruleType)
        {
            return RuleTypes.Contains(ruleType);
        }

        public bool IsRegistered(RuleSet ruleSet)
        {
            return RuleSets.Contains(ruleSet);
        }

        // TODO(orendain): Disallow registration after a certain point in init process.
        public void Register(Type ruleType)
        {
            RulesAPIMod.Logger.Msg($"Registering rule type: {ruleType}");
            if (RuleTypes.Contains(ruleType))
            {
                throw new ArgumentException("Rule type already registered.");
            }

            if (!typeof(Rule).IsAssignableFrom(ruleType))
            {
                throw new ArgumentException("Rule type incompatible.");
            }

            RuleTypes.Add(ruleType);
        }

        public void Register(RuleSet ruleSet)
        {
            RulesAPIMod.Logger.Msg($"Registering rule set: {ruleSet.GetType()} (with {ruleSet.Rules.Count} rules)");

            if (RuleSets.Contains(ruleSet))
            {
                throw new ArgumentException("Rule set already registered.");
            }

            foreach (var rule in ruleSet.Rules)
            {
                if (!IsRegistered(rule.GetType()))
                {
                    throw new ArgumentException($"Rule set includes unregistered rule type: {rule.GetType()}");
                }
            }

            RuleSets.Add(ruleSet);
        }
    }
}
